using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nozzibot.Models
{
    public enum Day
    {
        Mon,
        Tue,
        Wed,
        Thu,
        Fri,
        Sat,
        Sun
    }

    public class Event
    {
        [JsonProperty("Day")]
        public Day Day {get;set;}

        [JsonProperty("Time")]
        public SimpleTime Time { get; set; }

        [JsonProperty("Description")]
        public string Description {get;set;}

        [JsonConstructor]
        public Event(Day day, SimpleTime time, string description)
        {
            Day = day;
            Time = time;
            Description = description.Replace("\"", "");
        }

        public Event(string day, string time, string description)
        {
            Day = Enum.TryParse(day, out Day d) ? d : Day.Fri;

            Time = SimpleTime.FromString(time);

            Description = description.Replace("\"", "");
        }

        public static List<Event> GetFromFile(string file = "events.json")
        {
            string inJson = System.IO.File.ReadAllText(file);
            List<Event> events = JsonConvert.DeserializeObject<List<Event>>(inJson);
            var sorted = events.OrderBy(d => d.Day)
                .ThenBy(h => h.Time.Hour)
                .ThenBy(m => m.Time.Minute)
                .ToList();

            return sorted;
        }        
        
        public static List<Event> GetFromFile(string file, string day)
        {
            return GetFromFile(file).Where(d => d.Day == Enum.Parse<Day>(day)).ToList();
        }
    }
}
