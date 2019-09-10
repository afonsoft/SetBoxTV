using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Models
{
    public class GridPagedOutput<T>  where T : class
    {
        public GridPagedOutput(List<T> value) 
        {
            this.Rows = value;
        }

        [JsonProperty("current")]
        public int Current { get; set; }

        [JsonProperty("rowCount")]
        public int RowCount { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("rows")]
        public List<T> Rows { get; set; }

        //[JsonIgnore]
        //public JsonResult Output { get { return new JsonResult(this); } }

    }
    public class GridPagedInput
    {
        [JsonProperty("current")]
        public int Current { get; set; } = 1;

        [JsonProperty("rowCount")]
        public int RowCount { get; set; } = 10;

        [JsonProperty("id")]
        public string Id { get; set; } = "";

        [JsonProperty("searchPhrase")]
        public string SearchPhrase { get; set; } = "";

        [JsonProperty("sort")]
        public Dictionary<string, string> Sort { get; set; }
    }
}
