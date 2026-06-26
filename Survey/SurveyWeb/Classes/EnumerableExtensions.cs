using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace SurveyWeb.Classes
{
    public static class EnumerableExtensions
    {
        public static async Task ToExcel(this IEnumerable enumerable, Stream outputStream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
                {
                    #region excel building
                    WorkbookPart workbookpart = spreadSheetDocument.AddWorkbookPart();
                    workbookpart.Workbook = new Workbook();

                    // Add a WorksheetPart to the WorkbookPart.
                    WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetViews(new SheetView() { WorkbookViewId = 0, RightToLeft = true }), new SheetData());

                    // Add Sheets to the Workbook.
                    Sheets sheets = spreadSheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                    // Append a new worksheet and associate it with the workbook.
                    Sheet sheet = new Sheet() { Id = spreadSheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "sheet" };
                    sheets.Append(sheet);

                    // Get the sheetData cell table.
                    SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                    #region Read data
                    //Type type = enumerable.GetType().GetGenericArguments()[0];
                   // Dictionary<string, ExcelAttribute> properties = GetProperty(type);

                   // string[] titles = properties.OrderBy(x => x.Value.Order).Select(x => x.Value.Title).ToArray();

                    #endregion

                    #region Insert titles
                   // Row rowTitle = new Row();
                    //for (int i = 1; i <= titles.Length; i++)
                    //{
                    //    rowTitle.Append(new Cell()
                    //    {
                    //        DataType = new EnumValue<CellValues>(CellValues.String),
                    //        CellValue = new CellValue { Text = titles[i - 1] }
                    //    });
                    //}
                   // sheetData.Append(rowTitle);
                    #endregion

                    #region Insert data

                    foreach (object row in enumerable)
                    {
                        Row rowItem = new Row();
                        foreach(var p in row.GetType().GetProperties())
                        {
                            rowItem.Append(new Cell()
                            {
                                CellValue = new CellValue
                                { Text = p.GetValue(row).ToString() }
                            });
                        }
                        //    foreach (var cell in properties.OrderBy(x => x.Value.Order))
                        //{
                        //    rowItem.Append(new Cell()
                        //    {
                        //        //DataType = new EnumValue<CellValues>(cell.Value.Type),
                        //        //CellValue = new CellValue { Text = row.GetVal(type, cell.Key) }
                        //       
                        //    });
                        //}
                        sheetData.Append(rowItem);
                    }

                    #endregion


                    #endregion

                    // Close the document.
                    spreadSheetDocument.Close();
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(outputStream);
            }
        }

        //private static Dictionary<string, ExcelAttribute> GetProperty(Type type)
        //{
        //    Dictionary<string, ExcelAttribute> dic = new Dictionary<string, ExcelAttribute>();

        //    PropertyInfo[] props = type.GetProperties();
        //    foreach (PropertyInfo prop in props)
        //    {
        //        object[] attrs = prop.GetCustomAttributes(true);
        //        foreach (object attr in attrs)
        //        {
        //            ExcelAttribute authAttr = attr as ExcelAttribute;
        //            if (authAttr != null)
        //            {
        //                string propName = prop.Name;
        //                var tempExcelAttribute = new ExcelAttribute
        //                {
        //                    Title = string.IsNullOrWhiteSpace(authAttr.Title) ? propName : authAttr.Title,
        //                    Order = authAttr.Order
        //                };

        //                dic.Add(propName, tempExcelAttribute);
        //            }
        //        }
        //    }

        //    return dic;
        //}

        //private static string GetVal(this object list, Type type, string propName)
        //{
        //    try
        //    {
        //        PropertyInfo propInfo = type.GetProperty(propName);
        //        return propInfo.GetValue(list).ToString();
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}
    }
}