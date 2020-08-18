using System;
using Newtonsoft.Json;

namespace PathToMastery.Models.State
{
    public class Day : DayMeta
    {
        [JsonIgnore]
        public DateTimeOffset Date { get; set; }

        public int Dow => (int)Date.DayOfWeek;
        
        public Day(DayMeta meta, int offset)
        {
            D = meta.D;
            M = meta.M;
            Y = meta.Y;
            Type = meta.Type;
            MsId = meta.MsId;
            
            var date = new DateTime(Y, M, D);
            Date = new DateTimeOffset(date, TimeSpan.FromHours(offset));
        }
    }
}