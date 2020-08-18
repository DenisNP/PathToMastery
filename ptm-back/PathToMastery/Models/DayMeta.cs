using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PathToMastery.Models
{
    [BsonSerializer(typeof(DayMetaSerializer))]
    public class DayMeta
    {
        public int D { get; set; }
        public int M { get; set; }
        public int Y { get; set; }
        
        public int MsId { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public DateType Type { get; set; } = DateType.N;

        public override string ToString()
        {
            return $"{Y}_{M}_{D}_{((int)Type)}_{MsId}";
        }

        public static DayMeta FromString(string s)
        {
            var arr = s.Split("_");
            return new DayMeta
            {
                Y = int.Parse(arr[0]),
                M = int.Parse(arr[1]),
                D = int.Parse(arr[2]),
                Type = (DateType)int.Parse(arr[3]),
                MsId = int.Parse(arr[4])
            };
        }
    }
    
    public enum DateType
    {
        N = 0,
        Link = 1,
        Done = 2,
        Future = 3,
        Break = 4,
    }
}