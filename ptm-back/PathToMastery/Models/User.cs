using Newtonsoft.Json;

namespace PathToMastery.Models
{
    public class User : IIdentity
    {
        public string Id { get; set; }
        public long NotifyTime { get; set; }
        public string NotifyMessage { get; set; }
        
        [JsonIgnore]
        public PathData First { get; set; }
        [JsonIgnore]
        public PathData Second { get; set; }
        [JsonIgnore]
        public PathData Third { get; set; }
    }
}