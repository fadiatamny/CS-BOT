using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace OurTinyBot
{
    public class Timer
    {
        private DateTime offset;

        public Timer(int minutes)
        {
            this.offset = DateTime.Now.AddMinutes(minutes);
        }

        public bool CheckTime(DateTime t)
        {
            if (t.CompareTo(offset) >= 0)
                return true;
            return false;
        }

    }
}
