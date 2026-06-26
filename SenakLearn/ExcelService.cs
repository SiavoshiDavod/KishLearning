using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace SenakLearn
{
    public class ExcelService 
    {
        private const string _errorMessage = "فرمت فایل نادرست می باشد";
        private const string _notEmptyErrorMessage = "فایل نمی تواند خالی باشد";
        /// <summary>
        /// this method reads an Excel file and maps its data to a list of objects of a generic type T. 
        /// </summary>
        /// <typeparam name="T">
        /// T is a class type and has a parameterless constructor
        /// </typeparam>
        /// <param name="excelFile">
        /// This parameter represents the uploaded Excel file.
        /// </param>
        /// <returns>
        /// The method returns a list of objects of type T.
        /// </returns>
        /// <exception cref="InvalidOperationException"></exception>
        public List<T> ProcessExcelFile<T>(byte[] fileContents) where T : class, new()
        {
            var dataList = new List<T>();

            if (fileContents == null || fileContents.Length == 0)
                throw new InvalidOperationException(_notEmptyErrorMessage);

            using (var stream = new MemoryStream(fileContents))
            {

                using (var document = SpreadsheetDocument.Open(stream, false))
                {
                    var workbookPart = document.WorkbookPart;

                    if (workbookPart == null) throw new InvalidOperationException(_errorMessage);

                    var sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault();

                    if (sheet == null) throw new InvalidOperationException(_errorMessage);//("No sheet found in the Excel file.");

                    if (string.IsNullOrEmpty(sheet.Id)) throw new InvalidOperationException(_errorMessage);

                    var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);

                    var rows = worksheetPart.Worksheet.Descendants<Row>();

                    if (rows == null) throw new InvalidOperationException(_errorMessage);

                    //Get all properties of my generic data model (T)
                    var properties = typeof(T).GetProperties();

                    //Get headers of excel file
                    var headerCells = rows.First().Elements<Cell>();

                    if (headerCells == null) throw new InvalidOperationException(_errorMessage);

                    var headers = headerCells.Select(c => GetCellValue(document, c)).ToList();

                    if (headers.Count != properties.Length)
                        throw new InvalidOperationException(_errorMessage);

                    foreach (var row in rows.Skip(1)) // Skipping header row
                    {
                        var data = new T();

                        var cells = row.Descendants<Cell>().ToArray();

                        for (int i = 0; i < headers.Count; i++)
                        {
                            var property = properties[i];

                            string header = headers[i];

                            //var cell1 = cells.FirstOrDefault(c => GetColumnIndex(c.CellReference) == i + 1);
                            var cell = row.Elements<Cell>().ElementAtOrDefault(i);

                            var cellValue = cell != null ? GetCellValue(document, cell) : null;

                            var convertedValue = ConvertToType(cellValue, property.PropertyType);

                            property.SetValue(data, convertedValue);
                        }

                        dataList.Add(data);
                    }
                }
            }

            return dataList;
        }

        /// <summary>
        /// this method is designed to create an Excel file from a list of objects of a generic type T. 
        /// This method generates the Excel file in-memory and returns it as a MemoryStream, which can be used for downloading or further processing.
        /// </summary>
        /// <typeparam name="T">
        /// T is a class type and has a parameterless constructor
        /// </typeparam>
        /// <param name="data">
        ///  A list of objects of the generic type T that contains the data to be written to the Excel file.
        ///  </param>
        /// <returns>
        /// this method returns a MemoryStream containing the generated Excel file.
        /// </returns>
        public MemoryStream GenerateExcelFile<T>(List<T> data) where T : class
        {
            var stream = new MemoryStream();

            using (var spreadsheetDocument = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();
                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                var sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());
                var sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };
                sheets.Append(sheet);

                var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                // Add headers
                var headerRow = new Row();
                var properties = typeof(T).GetProperties();

                foreach (var prop in properties.Where(w=>w.CustomAttributes.Any(a=>a!=null && a.AttributeType== typeof(ExcelAttribute) )))
                {
                    var headerText = GetHeaderText(prop);
                    if (!string.IsNullOrEmpty(headerText))
                    {
                        headerRow.Append(new Cell
                        {
                            CellValue = new CellValue(headerText),
                            DataType = CellValues.String
                        });
                    }
                }

                sheetData.AppendChild(headerRow);

                // Add data rows
                foreach (var item in data)
                {
                    var dataRow = new Row();
                    foreach (var prop in properties.Where(w => w.CustomAttributes.Any(a => a != null && a.AttributeType == typeof(ExcelAttribute))))
                    {
                        var headerText = GetHeaderText(prop);
                        if (!string.IsNullOrEmpty(headerText))
                        {
                            var value = prop.GetValue(item);
                            dataRow.Append(new Cell
                            {
                                CellValue = new CellValue(value?.ToString() ?? string.Empty),
                                DataType = CellValues.String
                            });
                        }
                    }
                    sheetData.AppendChild(dataRow);
                }
            }

            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// This method calculates the zero-based column index from the cell reference (e.g., "A1" -> 0, "B1" -> 1).
        /// </summary>
        /// <param name="cellReference"></param>
        /// <returns></returns>
        private int GetColumnIndex(string cellReference)
        {
            if (string.IsNullOrEmpty(cellReference)) return -1;

            int columnIndex = 0;
            foreach (char c in cellReference)
            {
                if (char.IsLetter(c))
                {
                    columnIndex = (columnIndex * 26) + (c - 'A' + 1);
                }
                else
                {
                    break;
                }
            }
            return columnIndex;
        }

        /// <summary>
        /// this method is designed to retrieve the value of a cell from an Excel worksheet. 
        /// This method can handle different cell data types, including shared strings, dates, and booleans, and it returns the cell's value as a string.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="cell">
        /// the Excel cell from which the value is to be retrieved.
        /// </param>
        /// <returns></returns>
        private string GetCellValue(SpreadsheetDocument doc, Cell cell)
        {
            var value = cell.CellValue?.InnerText;
            if (cell.DataType != null && cell.DataType == CellValues.SharedString)
            {
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(int.Parse(value)).InnerText;
            }
            return value;
        }

        /// <summary>
        /// this method uses the CellValues enumeration to determine how to convert the cell value to the appropriate type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        private object ConvertToType(string value, Type targetType)
        {
            try
            {
                if (string.IsNullOrEmpty(value)) return GetDefault(targetType);

                switch (targetType)
                {
                    case Type t when t == typeof(bool):
                        return bool.Parse(value);
                    case Type t when t == typeof(bool?):
                        return bool.Parse(value);
                    case Type t when t == typeof(DateTime):
                        return DateTime.FromOADate(double.Parse(value));
                    case Type t when t == typeof(DateTime?):
                        return DateTime.FromOADate(double.Parse(value));
                    case Type t when t == typeof(Guid):
                        return Guid.Parse(value);
                    case Type t when t == typeof(Guid?):
                        return Guid.Parse(value);
                    default:
                        return Convert.ChangeType(value, targetType);
                }
            }
            catch (Exception ex)
            {
                return GetDefault(targetType);
            }
        }

        /// <summary>
        /// this method provides default values for value types to avoid setting null to value types, which would cause exceptions.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private object GetDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
        private string GetHeaderText(PropertyInfo prop)
        {
            var headerText = prop.GetCustomAttribute<DescriptionAttribute>()?.Description ?? prop.Name;
            //var headerText = prop.GetCustomAttribute<Attr>()?.Title ?? "";
            return headerText;
        }
    }
}
