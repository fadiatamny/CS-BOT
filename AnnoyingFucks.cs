using System;
using System.Collections.Generic;
using System.Text;

namespace OurTinyBot
{
    class AnnoyingFucks
    {
        private string guild;
        private string channel;
        private string reason;

        public AnnoyingFucks(string g, string c, string r)
        {
            this.guild = g.Trim();
            this.channel = c.Trim();
            this.reason = r.Trim();
        }

        public string Guild
        {
            get { return this.guild; }    
        }

        public string Channel
        {
            set { this.channel = value; }
            get { return this.channel; }
        }

        public string Reason
        {
            set { this.reason = value; }
            get { return this.reason; }
        }

        public override string ToString()
        {
            return guild + " - " + channel + " - " + reason;
        }
    }
}
