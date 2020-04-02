using Newtonsoft.Json;
using System.Collections.Generic;

namespace Crystal.Core.Shared.Model
{
    public class DataTableResponse<TEntity>
    {
        /// <summary>
        /// Total records in the dataset matching the filters without the pagination
        /// </summary>
        [JsonProperty("recordsTotal")]
        public int RecordsTotal { get; set; }
        /// <summary>
        /// Total records in the dataset matching the filters without the pagination
        /// </summary>
        [JsonProperty("iTotalRecords")]
        public int TotalRecords { get; set; }
        /// <summary>
        /// List of records
        /// </summary>
        [JsonProperty("aaData")]
        public List<TEntity> Data { get; set; }
        /// <summary>
        /// Total rows to be displayed
        /// </summary>
        [JsonProperty("iTotalDisplayRecords")]
        public int TotalDisplayRecords { get; set; }

        [JsonProperty("sEcho")]
        public string Echo { get; set; } = "sEcho";
    }
}
