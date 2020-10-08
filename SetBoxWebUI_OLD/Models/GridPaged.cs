using Newtonsoft.Json;
using System.Collections.Generic;

namespace SetBoxWebUI.Models
{
    public class GridPagedOutput<T>  where T : class, new()
    {
        public GridPagedOutput()
        {
            this.Rows = new List<T>();
        }

        public GridPagedOutput(IEnumerable<T> value)
        {
            if (value != null)
                this.Rows = value;
            else
                this.Rows = new List<T>();
        }

        [JsonProperty("current")]
        public int Current { get; set; }

        [JsonProperty("rowCount")]
        public int RowCount { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("rows")]
        public IEnumerable<T> Rows { get; set; }

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
