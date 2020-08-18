using MongoDB.Bson.Serialization.Attributes;

namespace PathToMastery.Models
{
    [BsonSerializer(typeof(DayMetaSerializer))]
    public class DayMeta
    {
        public int D { get; set; }
        public int M { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return $"{Y}_{M}_{D}";
        }

        public static DayMeta FromString(string s)
        {
            var arr = s.Split("_");
            return new DayMeta
            {
                Y = int.Parse(arr[0]),
                M = int.Parse(arr[1]),
                D = int.Parse(arr[2])
            };
        }
    }
}