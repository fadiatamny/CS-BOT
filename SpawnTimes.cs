using System;
using System.Collections.Generic;
using System.Text;

namespace OurTinyBot
{
    public class SpawnTimes
    {
        private List<TimeSpan> times;

        public SpawnTimes()
        {
            //init spawntimes set by game for bosses
            times = new List<TimeSpan>
            {
                new TimeSpan(01, 15, 00),
                new TimeSpan(03, 00, 00),
                new TimeSpan(06, 00, 00),
                new TimeSpan(10, 00, 00),
                new TimeSpan(13, 00, 00),
                new TimeSpan(17, 00, 00),
                new TimeSpan(20, 00, 00),
                new TimeSpan(23, 15, 00)
            };
        }
        //checks if current time t equals to any given boss spawntime 
        public int GetIndexTime(TimeSpan t)
        {
            for (int i = 0; i < times.Count; i++)
            {
                double x = t.Subtract(times[i]).TotalMinutes;

                if (x <= 1 && x >= -1)
                    return i;
            }

            return -1;
        }
    }
}
