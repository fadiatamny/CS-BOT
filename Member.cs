using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace OurTinyBot
{
    public class Member
    {
        private ulong id;

        public Member(ulong id)
        {
            this.id = id;
        }

        public ulong ID
        {
            set { }
            get { return this.id; }
        }

        public override string ToString()
        {
            return id.ToString();
        }
    }
}