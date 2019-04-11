using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using System.IO;

namespace OurTinyBot
{
    class Program
    {

        static DiscordClient discord;
        static CommandsNextModule commands; //commands
        static InteractivityModule interactivity; // interactive messaging 
        static string tok; // for getting the token for the bot from token files.

        static void Main(string[] args)
        { 
            tok = File.ReadAllLines("./token.txt")[0];
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = tok,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            commands = discord.UseCommandsNext(new CommandsNextConfiguration { StringPrefix = "!", CaseSensitive = false });  // sets prefix
            
            commands.RegisterCommands<Commands>(); // sets the class commands to active one 

            interactivity = discord.UseInteractivity(new InteractivityConfiguration());

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

    }
}
