using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PathToMastery.Services.Abstract;

namespace PathToMastery.Services
{
    public class VkService : ISocialService
    {
        private readonly ILogger<VkService> _logger;
        private const string VkApiAddress = "https://api.vk.com";
        private const string VkApiVersion = "5.120";
        private readonly string _vkApiKey;
        private readonly string _vkApiSecret;

        public VkService(ILogger<VkService> logger)
        {
            _logger = logger;
            var key = Environment.GetEnvironmentVariable("PATH_TO_MASTERY_VK_TOKEN");
            _vkApiKey = key ?? throw new ArgumentException("No VK Service token!");

            var secret = Environment.GetEnvironmentVariable("PATH_TO_MASTERY_VK_SECRET");
            _vkApiSecret = secret ?? throw new ArgumentException("No VK Secret key!");
        }

        public void Notify(string[] allUserIds, string message, string hash = "")
        {
            var uids = allUserIds.ToList();
            while (uids.Count > 0)
            {
                var userIds = uids.Shift(100);
                
                MakeVkRequest("notifications.sendMessage", new Dictionary<string, string>
                {
                    {"user_ids", string.Join(",", userIds)},
                    {"fragment", hash},
                    {"message", message}
                });
            }
        }

        public bool IsSignValid(string userId, Dictionary<string, string> pars, string sign)
        {
#if DEBUG
            return true;
#endif            
            var parsString = string.Join(
                "&",
                pars.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}").OrderBy(x => x)
            );

            var calculatedSign = Utils.ToBase64(Utils.HashHMAC(_vkApiSecret, parsString));
            return calculatedSign == sign && pars.ContainsKey("vk_user_id") && pars["vk_user_id"] == userId;
        }

        private string MakeVkRequest(string method, Dictionary<string, string> data)
        {
            var address = $"{VkApiAddress}/method/{method}?v={VkApiVersion}&access_token={_vkApiKey}";
            return Utils.MakeRequest(address, data, _logger);
        }
    }
}