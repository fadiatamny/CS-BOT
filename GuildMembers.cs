using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OurTinyBot
{
    class GuildMembers
    {
        private string memberList; // 
        private string reqeustList; // 
        private string annoyingfuckstext; // 
        // lists of data files needed to maintain a semi database for the program. all functions are identical in behavior but for mutliple types
        private List<Member> members;
        private List<Request> requests;
        private List<AnnoyingFucks> annoyings;

        public GuildMembers(string memberlist, string requestlist , string annoying)
        {
            this.memberList = memberlist;
            this.reqeustList = requestlist;
            this.annoyingfuckstext = annoying;

            members = new List<Member>();
            requests = new List<Request>();
            annoyings = new List<AnnoyingFucks>();

            LoadMembers();
            LoadRequests();
            LoadAnnoyings();

        }

        #region members
        //loads members from datafile
        private void LoadMembers()
        {
            string[] lines = File.ReadAllLines(this.memberList);

            foreach (string s in lines)
            {
                members.Add(new Member(ulong.Parse(s)));
            }
        }
        // add a member to list
        public void AddMember(ulong id)
        {
            members.Add(new Member(id));
            SaveMembers();
        }
        //outputs list to datafile
        private void SaveMembers()
        {
            string text = "";
            foreach (Member p in members)
            {
                text += p.ID + "\n";
            }

            File.WriteAllText(memberList, text);
        }
        //removes member from list 
        public void RemoveMember(ulong id)
        {
            for (int i = 0; i < members.Count; i++)
            {
                if (id == members[i].ID)
                {
                    members.Remove(members[i]);
                    break;
                }
            }

            SaveMembers();
        }
        //checks if member already exists
        public bool AlreadyAMember(ulong id)
        {
            for (int i = 0; i < members.Count; i++)
            {
                if (id == members[i].ID)
                    return true;
            }

            return false;
        }
        //prints out all members in file.
        public string ViewMembers()
        {
            string s = "";

            if (members.Count == 0)
                s = "No Requests";
            else
            {
                int count = 1;
                s += "Request - Whoever Requested\n";

                foreach (Member m in members)
                {
                    s += count++ + ". " + m.ToString() + "\n";
                }
            }

            return s;
        }

        #endregion

        #region Request

        private void LoadRequests()
        {
            string[] lines = File.ReadAllLines(this.reqeustList);

            foreach (string s in lines)
            {
                string[] text = s.Split("-");

                requests.Add(new Request(text[0], text[1]));
            }
        }

        public void AddRequest(string request, string by)
        {

            requests.Add(new Request(request, by));
            SaveRequests();
        }

        private void SaveRequests()
        {
            string text = "";

            foreach (Request r in requests)
            {
                text += r.Task;
                text += " - ";
                text += r.By;
                text += "\n";
            }

            File.WriteAllText(reqeustList, text);
        }

        public void RemoveRequest(int i)
        {
            requests.RemoveAt(i);

            SaveRequests();
        }

        public string ViewRequests()
        {
            string s = "";

            if (requests.Count == 0)
                s = "No Requests";
            else
            {
                int count = 1;
                s += "Request - Whoever Requested\n";

                foreach (Request r in requests)
                {
                    s += count++ + ". " + r.ToString() + "\n";
                }
            }

            return s;
        }

        public bool AlreadyRequested(string request)
        {
            for (int i = 0; i < requests.Count; i++)
            {
                if (requests[i].Task.CompareTo(request) == 0)
                    return true;
            }

            return false;
        }

        #endregion

        #region annoyings

        private void LoadAnnoyings()
        {
            string[] lines = File.ReadAllLines(this.annoyingfuckstext);

            foreach (string s in lines)
            {
                string[] text = s.Split("-");

                annoyings.Add(new AnnoyingFucks(text[0], text[1], text[2]));
            }
        }

        public void AddAnnyoing(string g, string c, string reason)
        {
            annoyings.Add(new AnnoyingFucks(g, c, reason));

            SaveAnnyoing();
        }

        private void SaveAnnyoing()
        {
            string text = "";

            foreach (AnnoyingFucks a in annoyings)
            {
                text += a.ToString();
                text += "\n";
            }

            File.WriteAllText(annoyingfuckstext, text);
        }

        public bool AlreadyAnnoying(string g)
        {
            foreach(AnnoyingFucks a in annoyings)
            {
                if (a.Guild.ToLower().CompareTo(g.Trim().ToLower()) == 0)
                    return true;
            }

            return false;
        }

        public string RandomAnnoying()
        {
            Random r = new Random();


            return annoyings[r.Next(annoyings.Count)].ToString();
        }

        public void RemoveAnnoyings(string g)
        {
            for (int i = 0; i < annoyings.Count; i++)
            {
                if (annoyings[i].Guild.ToLower().CompareTo(g.Trim().ToLower()) == 0)
                {
                    annoyings.Remove(annoyings[i]);
                    break;
                }
            }

            SaveAnnyoing();
        }

        public AnnoyingFucks GetAnnoying(string g)
        {
            foreach (AnnoyingFucks a in annoyings)
            {
                if (a.Guild.ToLower().CompareTo(g.Trim().ToLower()) == 0)
                    return a;
            }

            return null;
        }

        public string ViewAnnoyings()
        {
            string s = "";

            if (annoyings.Count == 0)
                s = " No Cucks to show";
            else
            {
                int count = 1;
                s += "GuildName - Channel - Reason \n";

                foreach (AnnoyingFucks a in annoyings)
                {
                    s += count++ + ". " + a.ToString() + "\n";
                }
            }

            return s;
        }

        #endregion

    }
}
