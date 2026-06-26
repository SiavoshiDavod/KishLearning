namespace SenakLearn.JqGrid
{
    public class GridHelper
    {

        /// <summary>
        /// Text used for filtering
        /// </summary>
        public string SearchString { get; set; }

        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int ColumnCount { get; set; }

        /// <summary>
        /// Number of columns that are used in sorting
        /// </summary>
        public int SortingColsCount { get; set; }

        public int SortCol { get; set; }
        public string SortDirection { get; set; }
        public bool Sortable { get; set; }

        /// <summary>
        /// Comma separated list of column names
        /// </summary>
        public string ColumnNames { get; set; }


    }
}
