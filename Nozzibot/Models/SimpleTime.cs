using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Nozzibot.Models
{
    public class SimpleTime
    {
        [JsonProperty("Hour")]
        public long Hour { get; set; }

        [JsonProperty("Minute")]
        public long Minute { get; set; }

        public SimpleTime(int hour, int minute)
        {
            if (hour > 24 || hour <= 0) 
                throw new ArgumentOutOfRangeException(nameof(hour), "Hour needs to be between 0 and 24");
            if (minute > 59 || minute <= 0) 
                throw new ArgumentOutOfRangeException(nameof(minute), "Minutes need to be between 0 and 60");

            Hour = hour;
            Minute = minute;
        }

        public override string ToString()
        {
            return $"{Hour}:{Minute}";
        }

        public static SimpleTime FromString(string time)
        {
            string[] tarr = time.Split(':');
            int[] timeArr = new int[2];

            timeArr[0] = int.Parse(tarr[0]);
            timeArr[1] = int.Parse(tarr[1]);

            return new SimpleTime(timeArr[0], timeArr[1]);
        }

        private sealed class HourMinuteEqualityComparer : IEqualityComparer<SimpleTime>
        {
            public bool Equals(SimpleTime x, SimpleTime y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Hour == y.Hour && x.Minute == y.Minute;
            }

            public int GetHashCode(SimpleTime obj)
            {
                unchecked
                {
                    return (obj.Hour.GetHashCode() * 397) ^ obj.Minute.GetHashCode();
                }
            }
        }

        public static IEqualityComparer<SimpleTime> HourMinuteComparer { get; } = new HourMinuteEqualityComparer();


        public static bool operator < (SimpleTime a, SimpleTime b)
        {
            var intA = int.Parse($"{a.Hour}{a.Minute}");
            var intB = int.Parse($"{b.Hour}{b.Minute}");

            return intA < intB;
        }

        public static bool operator > (SimpleTime a, SimpleTime b)
        {
            var intA = int.Parse($"{a.Hour}{a.Minute}");
            var intB = int.Parse($"{b.Hour}{b.Minute}");

            return intA > intB;
        }

        public static bool operator <= (SimpleTime a, SimpleTime b)
        {
            var intA = int.Parse($"{a.Hour}{a.Minute}");
            var intB = int.Parse($"{b.Hour}{b.Minute}");

            return intA <= intB;
        }

        public static bool operator >= (SimpleTime a, SimpleTime b)
        {
            var intA = int.Parse($"{a.Hour}{a.Minute}");
            var intB = int.Parse($"{b.Hour}{b.Minute}");

            return intA >= intB;
        }

        public static bool operator == (SimpleTime a, SimpleTime b)
        {
            var intA = int.Parse($"{a.Hour}{a.Minute}");
            var intB = int.Parse($"{b.Hour}{b.Minute}");

            return intA == intB;
        }

        public static bool operator !=(SimpleTime a, SimpleTime b)
        {
            return !(a == b);
        }
    }
}
