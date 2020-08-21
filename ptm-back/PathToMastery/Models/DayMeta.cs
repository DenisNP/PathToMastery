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
        
        public int MsD { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public DateType Type { get; set; } = DateType.N;

        public override string ToString()
        {
            return $"{Y}_{M}_{D}_{((int)Type)}_{MsD}";
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
                MsD = int.Parse(arr[4])
            };
        }
        
        public DateTimeOffset ToDateTimeOffset(int offset)
        {
            var date = new DateTime(Y, M, D);
            return new DateTimeOffset(date, TimeSpan.FromHours(offset));
        }

        public bool IsDayOf(DateTimeOffset offset)
        {
            return D == offset.Day && M == offset.Month && Y == offset.Year;
        }
    }
    
    public enum DateType
    {
        N = 0,
        Link = 1,
        Done = 2,
        DoneBreak = 3,
        DoneLink = 4,
        Checkpoint = 5,
        Break = 6,
    }
}