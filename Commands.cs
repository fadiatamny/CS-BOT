using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities; // to include the Discord Emoji n other entities 
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace OurTinyBot
{
    public class Commands
    {
	//init a instance of GUILD class with data files.
        GuildMembers guild = new GuildMembers(
            @".\Data.txt",
            @".\Requests.txt",
            @".\Annoyings.txt");

        const string BotManager = "BotManager";
        const string DecMaster = "DecMaster";
	    
	//prints commands
        [Command("Commands")]
        public async Task commandslist(CommandContext ctx)
        {
            string s = "```Bot Prefix : !```\n\n";

            string s1 =
                "```BotManager Commands:\n" +
                "1. BossSchedule - starts the boss timers (only do it once)\n" +
                "2. DayNight [day/night] [hours] [minutes] - sets the daynight cycle to thes (only do it once)\n" +
                "3. Purge - purges the chat texts.\n" +
                "4. ViewRequests - views all requests made for the bot.\n" +
                "6. RemoveRequest [index] - remove request at certain index```\n\n";

            string s2 =
                "```DecMaster Commands:\n" +
                "1. AddCuck [Guildname] [Channel] [Reason] - adds the cucks to the naughty list\n" +
                "2. RemoveCuck [Guildname] - removes the cucks from the naughty list\n" +
                "3. ViewCucks - shows all cucklist members\n" +
                "4. ModifyCuck [Guildname] [Channel] [Reason] - modify cucks.\n" +
                "5. RandomCuck - gives you a random cuck from the cucks list```\n\n";

            string s3 =
                "```Common Commands: \n" +
                "1. NotifyMe - this command will add you to role to be notified with bot notification. ex : boss timers\n" +
                "2. UnNotifyMe - removes you from notification list.\n" +
                "3. Timer [Duration] [Reason* {Optional}] - sets a timer for said duration and if given a reason it will remind you as to why you set that timer.\n" +
                "4. RandomChannel - Gives you a random channel in the game to hop on to maybe :P?\n" +
                "5. WishList - is made to add a request for the bot to maybe add in future as a function or something.\n" +
                "6. Reee - its just a reee.\n" +
				"7. Mirumok - gives you mirumok role." +
                "```\n";

            bool manager = false;
            bool dec = false;

            foreach (DiscordRole r in ctx.Member.Roles)
            {
                if (r.Name.CompareTo(BotManager) == 0)
                {
                    manager = true;
                }
                
                if(r.Name.CompareTo(DecMaster) == 0)
                {
                    dec = true;
                }
            }

            if (manager)
                s += s1;
            if (dec)
                s += s2;

            s += s3;

            await ctx.RespondAsync($"{s}");
        }

        #region BotManager
	//region for botmanager privlige functions
		
	//function that runs the time checker for ingame boss spawns
        [Command("BossSchedule")]
        [RequireRolesAttribute(BotManager)]
        public async Task BossCheck(CommandContext ctx)
        {
            if (ctx.Channel.Name != "boss-timer")
                return;
	
            #region initilize
		
            DateTime t;
            bool state = false;
            SpawnTimes st = new SpawnTimes();
            List<string> ls = null;

            int delay = 300000;

            var messages = await ctx.Channel.GetMessagesAsync(1);
            await ctx.Channel.DeleteMessageAsync(messages[0]);

            bool spawned = false;
            bool ftime = true;

            var roles = ctx.Guild.Roles;
            int count = 0;

            foreach (var role in roles)
            {
                if (role.Name == "BossSchedule")
                    break;
                count++;
            }

            string nextBoss = "";

	    //main loop function. it runs 24/7 and checks if a boss has spawned given the correct time
            while (true)
            {
                t = DateTime.Now;

                Console.WriteLine("Time now: " + t.TimeOfDay +" State:"+state.ToString());

                if (t.Second > 5)
                    state = false;

                if (!state)
                {
                    if (t.Second <= 2)
                    {
                        if (t.Minute % 5 == 0)
                        {
                            state = true; //var to makke sure the timer is dead center less than 2 seconds off. for more accurate bot.
                        }

                        else
                        {
                            await Task.Delay(60000);
                            //checks every min for correct time.
                        }
                    }
                }

                else
                {
		    //choose correct the day of week with corresponding spawnlist sheet 
                    switch (t.Date.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            ls = new List<string>
                        {
                            "Karanda , Kutum",
                            "Karanda",
                            "Kzarka",
                            "Kzarka",
                            "Offin",
                            "Kutum",
                            "Nouver",
                            "Kzarka"
                        };
                            nextBoss = "Karanda";
                            break;

                        case DayOfWeek.Tuesday:
                            ls = new List<string>
                        {
                            "Karanda",
                            "Kutum",
                            "Kzarka",
                            "Nouver",
                            "Kutum",
                            "Nouver",
                            "Karanda",
                            "Garmoth"
                        };
                            nextBoss = "Kutum , Kzarka";
                            break;

                        case DayOfWeek.Wednesday:
                            ls = new List<string>
                        {
                            "Kutum , Kzarka",
                            "Karanda",
                            "Kzarka",
                            "Karanda",
                            "NONE - Maintenance",
                            "Kutum",
                            "Offin",
                            "Karanda , Kzarka"
                        };
                            nextBoss = "Nouver";

                            break;

                        case DayOfWeek.Thursday:
                            ls = new List<string>
                        {
                            "Nouver",
                            "Kutum",
                            "Nouver",
                            "Kutum",
                            "Nouver",
                            "Kzarka",
                            "Kutum",
                            "Garmoth"
                        };
                            nextBoss = "Karanda , Kzarka";
                            break;

                        case DayOfWeek.Friday:
                            ls = new List<string>
                        {
                            "Karanda , Kzarka",
                            "Nouver",
                            "Karanda",
                            "Kutum",
                            "Karanda",
                            "Nouver",
                            "Kzarka",
                            "Kutum , Kzarka"
                        };
                            nextBoss = "Karanda";
                            break;

                        case DayOfWeek.Saturday:
                            ls = new List<string>
                        {
                            "Karanda",
                            "Offin",
                            "Nouver",
                            "Kutum",
                            "Nouver",
                            "Quint/Muraka",
                            "Karanda , Kzarka",
                            "NONE - Conquest War"
                        };
                            nextBoss = "Nouver , Kutum";
                            break;

                        case DayOfWeek.Sunday:
                            ls = new List<string>
                        {
                            "Nouver , Kutum",
                            "Kzarka",
                            "Kutum",
                            "Nouver",
                            "Kzarka",
                            "Vell",
                            "Garmoth",
                            "Kzarka , Nouver"
                        };
                            nextBoss = "Karanda , Kutum";
                            break;
                    }
                    #endregion

                    int mins = 60;
		    //check if its spawning in an hour of now
                    int x = st.GetIndexTime(t.TimeOfDay.Add(new TimeSpan(01, 00, 00)));

                    if (x == -1)
                    {
			//checks if its spawining in 15 minutes
                        x = st.GetIndexTime(t.TimeOfDay.Add(new TimeSpan(00, 15, 00)));
                        mins = 15;
                    }

                    if (x == -1)
                    {
			// check if it spawned.
                        x = st.GetIndexTime(t.TimeOfDay.Add(new TimeSpan(00, 00, 00)));
                        mins = 0;
                    }

                    if (x != -1)
                    {
                        if (!ftime)
                        {
                            if (spawned)
                            {
                                var messageslist = await ctx.Channel.GetMessagesAsync(2);
                                await messageslist[0].DeleteAsync();
                                await messageslist[1].DeleteAsync();
                            }
                            else
                            {
                                var messageslist = await ctx.Channel.GetMessagesAsync(1);
                                await messageslist[0].DeleteAsync();
                            }
                        }

                        spawned = false; // var for if it spawned or not
                        ftime = false; // var for first spawn of the day so it wont go deletting messages before.

                        if (ls[x].CompareTo("NONE - Conquest War") == 0)
                        {
                            await ctx.RespondAsync($"Conquest War Time - No Bosses");
                        }
                        else if (ls[x].CompareTo("NONE - Maintenance") == 0)
                        {
                            await ctx.RespondAsync($"Maintenance Time - No Bosses");
                        }
                        else
                        {
                            if (mins != 0)
                            {
                                await ctx.RespondAsync($"{roles[count].Mention} {ls[x]} Will Be Spawning in {mins} minutes");
                            }
                            else
                            {
                                await ctx.RespondAsync($"{roles[count].Mention} {ls[x]} Spawned !");
                                spawned = true;
                            }

                            if (spawned)
                            {
                                if (x + 1 != ls.Count) // if the nextboss to spawn is in the next day or not
                                {
                                    await ctx.RespondAsync($"Next Boss Will Be {ls[x + 1]}.");
                                }
                                else
                                {
                                    await ctx.RespondAsync($"Next Boss Will Be {nextBoss}.");
                                }
                            }
                        }
                    }

                    await Task.Delay(delay);
                }
            }
        }
	    
	   
	//timer for ingame day night cycle initilized with either day or night string 
        [Command("DayNight")]
        [RequireRolesAttribute(BotManager)]
        public async Task DayNight(CommandContext ctx, string time = "day", int hours = 0 , int minutes = 0)
        {
            var messages = await ctx.Channel.GetMessagesAsync(1);
            await ctx.Channel.DeleteMessageAsync(messages[0]);
            bool flag = false;

            if (time.CompareTo("day") == 0)
                flag = true;
            else
                flag = false;

            int delay = 60000;

            DateTime t;

            while (true)
            {

                if (!flag)
                {
                    goto DayNight;
                }             

                if(minutes == 0 && hours == 0)
                {
                    minutes = 20;
                    hours = 3;
                }

                t = DateTime.Now.Add(new TimeSpan(hours, minutes, 0));

                minutes = 0;
                hours = 0;

                while (DateTime.Now.CompareTo(t) < 0)
                {
                    TimeSpan date = t.Subtract(DateTime.Now);
                    await ctx.Client.UpdateStatusAsync(new DiscordGame("Night time in " + date.Hours + ":" + date.Minutes));
                    await Task.Delay(delay);
                }

                DayNight:

                if (minutes == 0 && hours == 0)
                {
                    minutes = 40;
                    hours = 0;
                }

                t = DateTime.Now.Add(new TimeSpan(hours, minutes, 0));

                hours = 0;
                minutes = 0;

                while (DateTime.Now.CompareTo(t) < 0)
                {
                    TimeSpan date = t.Subtract(DateTime.Now);
                    await ctx.Client.UpdateStatusAsync(new DiscordGame("Day Time in " + date.Hours + ":" + date.Minutes));
                    await Task.Delay(delay);
                }
            }
        }
	
	// just deletes messages in chat. 
        [Command("Purge")]
        [RequireRolesAttribute(BotManager)]
        public async Task PurgeChat(CommandContext ctx)
        {
            int delay = 500;

            var messages = await ctx.Channel.GetMessagesAsync(1);

            while (messages.Count != 0)
            {
                await ctx.Channel.DeleteMessageAsync(messages[0]);
                await Task.Delay(delay);
                messages = await ctx.Channel.GetMessagesAsync(1);
            }

            delay = 5000;
            var m = await ctx.RespondAsync($"Purge completed. This message will be commit sudoku in {delay / 1000} seconds.");
            await Task.Delay(delay);
            await m.DeleteAsync();
        }

	// for implemented requests list from users for bot commands / functions
        [Command("ViewRequests")]
        [RequireRolesAttribute(BotManager)]
        public async Task ViewRequests(CommandContext ctx)
        {
            string s = "```";

            s += guild.ViewRequests();

            s += "```";

            await ctx.RespondAsync($"{s}");
        }
	
	// to remove a request
        [Command("RemoveRequest")]
        [RequireRolesAttribute(BotManager)]
        public async Task RemoveRequest(CommandContext ctx, int i)
        {
            var messageslist = await ctx.Channel.GetMessagesAsync(2);
            guild.RemoveRequest(i-1);
            await messageslist[0].DeleteAsync();
            await messageslist[1].DeleteAsync();
        }

        #endregion

        #region DecMasters
		
	//adds a said cuck guild to a hitlist 
        [Command("AddCuck")]
        [RequireRolesAttribute(DecMaster)]
        public async Task AddCucks(CommandContext ctx)
        {
            int pos = ctx.Message.Content.IndexOf(" ");
            string s = ctx.Message.Content.Substring(pos+1);

            string[] text = s.Split(" ");

            string name = text[0];
            string channel = text[1];
            pos = s.IndexOf(" ");
            s = s.Substring(pos + 1);
            pos = s.IndexOf(" ");
            s = s.Substring(pos + 1);

            string reason = s;

            if (guild.AlreadyAnnoying(name))
            {
                await ctx.RespondAsync($"Already in Cucks list");
            }
            else
            {
                guild.AddAnnyoing(name, channel, reason);           
                await ctx.RespondAsync($"Added to Cucks list");
                await ctx.RespondAsync($"Current Cucks:\n");
                await ctx.RespondAsync($"```{guild.ViewAnnoyings()}```");
            }
        }
	    
	//removes a cuck from the hitlist
        [Command("RemoveCuck")]
        [RequireRolesAttribute(DecMaster)]
        public async Task RemoveCucks(CommandContext ctx, string g)
        {
            if (guild.AlreadyAnnoying(g))
            {
                guild.RemoveAnnoyings(g);
                await ctx.RespondAsync($"Removed from Cucks list.");
            }
            else
            {
                await ctx.RespondAsync($"Not in Cucks list");
            }
        }
	    
	//views all cucks in the hitlist
        [Command("ViewCucks")]
        [RequireRolesAttribute(DecMaster)]
        public async Task ViewCucks(CommandContext ctx)
        {
            await ctx.RespondAsync($"```{guild.ViewAnnoyings()}```");
        }
	    
	//change cuck detail 
        [Command("ModifyCuck")]
        [RequireRolesAttribute(DecMaster)]
        public async Task ModifyCuck(CommandContext ctx)
        {
            int pos = ctx.Message.Content.IndexOf(" ");
            string s = ctx.Message.Content.Substring(pos + 1);

            string[] text = s.Split(" ");

            string name = text[0];
            string channel = text[1];
            pos = s.IndexOf(" ");
            s = s.Substring(pos + 1);
            pos = s.IndexOf(" ");
            s = s.Substring(pos + 1);
            string reason = s;

            if (guild.AlreadyAnnoying(name))
            {
                AnnoyingFucks a = guild.GetAnnoying(name);

                a.Channel = channel;
                a.Reason = reason;

                await ctx.RespondAsync($"Cuck Modified.");
            }
            else
            {
                await ctx.RespondAsync($"Cuck doesnt exist in list. Please add it");
            }
        }

	//returns a random cuck from the hitlist to be slaughtered.
        [Command("RandomCuck")]
        [RequireRolesAttribute(DecMaster)]
        public async Task RandomCuck(CommandContext ctx)
        {
            await ctx.RespondAsync($"{guild.RandomAnnoying()}");
        }
        #endregion

        #region User Functions

	//gives role to be notified by the bot when a boss spawns
        [Command("NotifyMe")]
        public async Task NotifyMe(CommandContext ctx)
        {
            if (!guild.AlreadyAMember(ctx.User.Id))
            {
                var user = ctx.Member;
                var roles = ctx.Guild.Roles;
                int count = 0;
                foreach (var role in roles)
                {
                    if (role.Name == "BossSchedule")
                        break;
                    count++;
                }
                await user.GrantRoleAsync(roles[count]);
                guild.AddMember(user.Id);
                await ctx.RespondAsync($"Added to notification list.");
            }
            else
            {
                await ctx.RespondAsync($"Already added to notification list");
            }
        }

	//removes role from user
        [Command("UnNotifyMe")]
        public async Task UnNotifyMe(CommandContext ctx)
        {
            if (guild.AlreadyAMember(ctx.User.Id))
            {
                var user = ctx.Member;
                var roles = ctx.Guild.Roles;
                int count = 0;
                foreach (var role in roles)
                {
                    if (role.Name == "BossSchedule")
                        break;
                    count++;
                }
                await user.RevokeRoleAsync(roles[count]);
                guild.RemoveMember(user.Id);
                await ctx.RespondAsync($"Removed from notification list.");
            }
            else
            {
                await ctx.RespondAsync($"Not in notification list");
            }
        }

	// starts a given timer for a user with given reason for x amount of minutes
        [Command("Timer")]
        public async Task Time(CommandContext ctx, int minutes, string reason = "")
        {
            if (reason != "")
            {
                int index = ctx.Message.Content.IndexOf(' ');
                reason = ctx.Message.Content.Substring(index + 1);
                index = reason.IndexOf(' ');
                reason = reason.Substring(index + 1);

				if(reason[0] == ' ')
					reason = reason.Substring(1);

            }
            if (minutes < 0)
            {
                await ctx.RespondAsync($"Cant do that time tho:/ ");
            }

            else
            {
                Timer t = new Timer(minutes);

                await ctx.RespondAsync($"{ctx.User.Mention} I've started the clock for you with {minutes} minutes");

                while (true)
                {
                    if (t.CheckTime(DateTime.Now))
                    {
                        await ctx.RespondAsync($"{ ctx.User.Mention} the {reason} timer is up !");
                        break;
                    }
                }
            }
        }

	// returns a random channel number from ingame
        [Command("RandomChannel")]
        public async Task RandomChannel(CommandContext ctx)
        {
            List<string> list = new List<string>
            {
                "Balenos",
                "Kamasylvia",
                "Velia",
                "Serendia",
                "Calpheon",
                "Mediah",
                "Valencia"
            };

            Random r = new Random();

            int channel = r.Next(7);
            int number = 0;

            if (list[channel].CompareTo("Kamasylvia") == 0)
                number = r.Next(1, 5);
            else
                number = r.Next(1, 7);

            await ctx.RespondAsync($"{ctx.User.Mention} Go to {list[channel]}{number}");               
        }

	// form to be filled for people wanting something from the bot
        [Command("WishList")]
        public async Task WishList(CommandContext ctx)
        {
            int i = ctx.Message.Content.IndexOf(" ");
            string s = ctx.Message.Content.Substring(i+1);

            if (guild.AlreadyRequested(s))
            {
                await ctx.RespondAsync($"Request is already in reviewd.");
            }
            else
            {
                guild.AddRequest(s, ctx.User.Username);
                await ctx.RespondAsync($"Request has been added and will be reviewd");
            }
        }
	    
	//ultimate REEE command?
        [Command("Reee")]
        public async Task Reee(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            string s = "";

            Random r = new Random();

            if (r.Next(2) == 0)
                s += 'r';
            else
                s += 'R';

            int len = r.Next(3, 51);
            int x = 0;

            for (int i = 0; i < len; i++)
            {
                x = r.Next(2);

                if (x == 0)
                    s += "e";
                else
                    s += "E";
            }

            await ctx.RespondAsync($"{s}");
        }
	    
	//Gives user the mirumok role
	[Command("Mirumok")]
        public async Task Mirumok(CommandContext ctx)
        {

            var user = ctx.Member;
            var roles = ctx.Guild.Roles;
            int count = 0;

            foreach (var role in user.Roles)
            {
                if (role.Name == "Mirumok")
                {
                    await ctx.RespondAsync($"You already have the role.");
                    return;
                }
            }

            foreach (var role in roles)
            {
                if (role.Name == "Mirumok")
                    break;
                count++;
            }

            await user.GrantRoleAsync(roles[count]);
            guild.AddMember(user.Id);
            await ctx.RespondAsync($"Added mirumok role.");
        }

        #endregion

        #region Misc?

        ////work on this pls XD get better website and better img handler
        //[Command("Rule34")]
        //public async Task Rule34(CommandContext ctx)
        //{
        //    string url = "https://scrolller.com/media/a48ba.jpg";
        //    string html;
        //    using (WebClient client = new WebClient())
        //    {

        //        html = client.DownloadString(url);
        //    }
        //    string pattern = @"<img.*?src=""(?<url>.*?)"".*?>";
        //    Regex rx = new Regex(pattern);
        //    foreach (Match m in rx.Matches(html))
        //    {
        //        string s = m.Groups["url"].Value;

        //        //if(s.Contains("thumbnails"))
        //        await ctx.RespondAsync("URL: " + m.Groups["url"].Value);
        //        int delay = 2000;
        //        await Task.Delay(delay);

        //    }
        //}

        [Command("Hi")]
        public async Task Hi(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var emoji = DiscordEmoji.FromName(ctx.Client, ":wave:");

            await ctx.RespondAsync($"Hey , {ctx.User.Mention} {emoji}!");
        }

        [Command("Random")]
        public async Task Random(CommandContext ctx, double min, double max)
        {
            await ctx.TriggerTypingAsync();

            var rnd = new Random();
                
            await ctx.RespondAsync($"Your Number is :{rnd.Next((int)min, (int)max)}");
        }

        #endregion

    }
}
