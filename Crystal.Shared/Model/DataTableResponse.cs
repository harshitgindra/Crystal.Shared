using Newtonsoft.Json;
using System.Collections.Generic;

namespace Crystal.Shared.Model
{
    public class DataTableResponse<TEntity> where TEntity : class
    {
        /// <summary>
        /// Total records, before filtering (i.e. the total number of records in the database)
        /// </summary>
        [JsonProperty("iTotalRecords")]
        public int TotalRecords { get; set; }

        /// <summary>
        /// Total records, after filtering (i.e. the total number of records after filtering has been applied -
        /// not just the number of records being returned for this page of data).
        /// </summary>
        [JsonProperty("recordsFiltered")]
        public int RecordsFiltered { get; set; }

        /// <summary>
        /// The data to be displayed in the table. This is an array of data source objects, one for each row, which will be used by DataTables.
        /// </summary>
        [JsonProperty("aaData")]
        public List<TEntity> Data { get; set; }

        /// <summary>
        /// Total records, before filtering (i.e. the total number of records in the database)
        /// </summary>
        [JsonProperty("iTotalDisplayRecords")]
        public int TotalDisplayRecords => TotalRecords;


        [JsonProperty("sEcho")] public string Echo { get; set; } = "sEcho";

        /// <summary>
        /// f an error occurs during the running of the server-side processing script, 
        /// you can inform the user of this error by passing back the error message to be displayed using this parameter. 
        /// </summary>
        [JsonProperty("error")]
        public int Error { get; set; }
    }
}