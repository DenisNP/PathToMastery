using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PathToMastery
{
    public static class Utils
    {
        public static readonly JsonSerializerSettings ConverterSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };
        
        public static long Now()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public static long ToUnixTimeMs(this DateTime dateTime)
        {
            var span = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (long)span.TotalMilliseconds;
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list)  
        {
            var rng = new Random();
            var buffer = list.ToList();
            for (var i = 0; i < buffer.Count; i++)
            {
                var j = rng.Next(i, buffer.Count);
                yield return buffer[j];
                buffer[j] = buffer[i];
            }
        }
        
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        
        public static string MakeRequest(string url, Dictionary<string, string> data, ILogger logger = null)
        {
            var kvList = new List<KeyValuePair<string, string>>();
            foreach (var (key, value) in data)
            {
                kvList.Add(new KeyValuePair<string, string>(key, value));
            }
            var formContent = new FormUrlEncodedContent(kvList);
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept-Language", "ru-RU");
            var response = client.PostAsync(url, formContent).Result;
            var bytes = response.Content.ReadAsByteArrayAsync().Result;

            var stringResponse = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            logger?.LogInformation(stringResponse);
            return stringResponse;
        }
        
        public static byte[] HashHMAC(string key, string message)
        {
            var encoding = new UTF8Encoding();
            var hash = new HMACSHA256(encoding.GetBytes(key));
            return hash.ComputeHash(encoding.GetBytes(message));
        }
        
        public static string ToBase64(byte[] hash)
        {
            return Convert.ToBase64String(hash)
                .TrimEnd(new []{'='})
                .Replace('+', '-')
                .Replace('/', '_');
        }

        public static List<T> Shift<T>(this List<T> list, int count)
        {
            var items = list.Take(count).ToList();
            list.RemoveRange(0, Math.Min(count, list.Count));
            return items;
        }

        public static string SafeSubstring(this string s, int len, string postfix = "...")
        {
            return s.Length <= len ? s : (s.Substring(0, len) + postfix);
        }

        public static int NormalDow(this DateTimeOffset date)
        {
            var dow = (int) date.DayOfWeek;
            return dow == 0 ? 7 : dow;
        }

        public static bool IsSameDayAs(this DateTimeOffset offset, DateTimeOffset second)
        {
            return offset.Day == second.Day && offset.Month == second.Month && offset.Year == second.Year;
        }
    }
}