using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PathToMastery.Models.State
{
    public class Day : DayMeta
    {
        public int Dow { get; set; }
        public int MsId { get; set; } = -1;
        
        [JsonConverter(typeof(StringEnumConverter))]
        public DateType Type { get; set; } = DateType.N;
    }

    public enum DateType
    {
        N,
        Link,
        Done,
        Future,
        Break,
    }
}