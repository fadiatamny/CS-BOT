using System;
using System.Collections.Generic;
using System.Text;

namespace OurTinyBot
{
    class Request
    {
        private string task;
        private string by;

        public Request(string task, string by)
        {
            this.task = task;
            this.by = by;
        }

        public string By
        {
            get { return this.by; }
        }

        public string Task
        {
            get { return this.task; }
        }

        public override string ToString()
        {
            return task + " - " + by;
        }
    }
}
