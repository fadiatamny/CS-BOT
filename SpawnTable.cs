using System;
using System.Collections.Generic;
using System.Text;

namespace OurTinyBot
{
    public class SpawnTable
    {
        private List<string> bosses;
        private DayOfWeek day;

        public SpawnTable(DayOfWeek day, List<string> bosses)
        {
            this.day = day;
            this.bosses = bosses;
        }

        public DayOfWeek Day
        {
            get { return this.day; }
        }

        public string GetBoss(int index)
        {
            return this.bosses[index];
        }
    }
}
