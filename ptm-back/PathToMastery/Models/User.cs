using System.Text.Json.Serialization;

namespace PathToMastery.Models
{
    public class User
    {
        public string Id { get; set; }
        
        [JsonIgnore]
        public PathData First { get; set; }
        [JsonIgnore]
        public PathData Second { get; set; }
        [JsonIgnore]
        public PathData Third { get; set; }
    }
}