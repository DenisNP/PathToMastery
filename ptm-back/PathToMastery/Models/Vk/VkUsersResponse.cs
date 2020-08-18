using Newtonsoft.Json;

namespace PathToMastery.Models.Vk
{
    public class VkUsersResponse
    {
        [JsonProperty("response")]
        public VkUser[] Response { get; set; }
    }
}