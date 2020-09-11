using Newtonsoft.Json;
using System.Collections.Generic;

namespace Afonsoft.SetBox.Dto
{
    public class GridPagedOutput<T> where T : class
    {
        public GridPagedOutput(IEnumerable<T> value)
        {
            this.Rows = value;
        }

        [JsonProperty("current")]
        public int Current { get; set; }

        [JsonProperty("rowCount")]
        public int RowCount { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("rows")]
        public IEnumerable<T> Rows { get; set; }

    }
}
