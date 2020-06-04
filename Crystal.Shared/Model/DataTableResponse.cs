using System.Collections.Generic;
using Newtonsoft.Json;

namespace Crystal.Shared.Model
{
    public class DataTableResponse<TEntity> where TEntity : class
    {
        /// <summary>
        ///     Total records in the dataset matching the filters without the pagination
        /// </summary>
        [JsonProperty("recordsTotal")]
        public int RecordsTotal { get; set; }

        /// <summary>
        ///     Total records in the dataset matching the filters without the pagination
        /// </summary>
        [JsonProperty("iTotalRecords")]
        public int TotalRecords { get; set; }

        /// <summary>
        ///     List of records
        /// </summary>
        [JsonProperty("aaData")]
        public List<TEntity> Data { get; set; }

        /// <summary>
        ///     Total rows to be displayed
        /// </summary>
        [JsonProperty("iTotalDisplayRecords")]
        public int TotalDisplayRecords { get; set; }

        [JsonProperty("sEcho")] public string Echo { get; set; } = "sEcho";
    }
}