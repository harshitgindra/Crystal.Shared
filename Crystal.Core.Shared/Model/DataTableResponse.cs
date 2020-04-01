using Newtonsoft.Json;
using System.Collections.Generic;

namespace Crystal.Core.Shared.Model
{
    public class DataTableResponse<TEntity>
    {
        [JsonProperty("recordsTotal")]
        public int RecordsTotal { get; set; }

        [JsonProperty("iTotalRecords")]
        public int TotalRecords { get; set; }

        [JsonProperty("aaData")]
        public List<TEntity> Data { get; set; }

        [JsonProperty("iTotalDisplayRecords")]
        public int TotalDisplayRecords { get; set; }

        [JsonProperty("sEcho")]
        public string Echo { get; set; } = "sEcho";
    }
}
