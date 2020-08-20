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

            Date = ToDateTimeOffset(offset);
        }

        public Day(DateTimeOffset date, DateType type, int milestoneId, int offset)
        {
            D = date.Day;
            M = date.Month;
            Y = date.Year;
            Type = type;
            MsId = milestoneId;

            Date = ToDateTimeOffset(offset);
        }
    }
}