using Newtonsoft.Json;

namespace PathToMastery.Models.Vk
{
    public class VkUser
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        
        [JsonProperty("photo_200")]
        public string Photo { get; set; }

        public string FullName => $"{FirstName} {LastName}".Trim();

        public string OnlyName => !string.IsNullOrEmpty(FirstName) ? FirstName : LastName;
    }
}