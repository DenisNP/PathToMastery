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
            MsD = meta.MsD;

            Date = ToDateTimeOffset(offset);
        }

        public Day(DateTimeOffset date, DateType type, int milestoneDays, int offset)
        {
            D = date.Day;
            M = date.Month;
            Y = date.Year;
            Type = type;
            MsD = milestoneDays;

            Date = ToDateTimeOffset(offset);
        }
    }
}