using Newtonsoft.Json;

namespace PathToMastery.Models
{
    public class User : IIdentity
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