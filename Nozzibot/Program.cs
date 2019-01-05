using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Nozzibot.Helpers;
using Nozzibot.Models;

namespace Nozzibot
{
    class Program
    {
        private static DiscordClient _discord;
        private static CommandsNextModule _commands;

        private List<Event> Events;
        
        // MAIN
        static void Main(string[] args)
        {
            // Make bot listen
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        
        // ASYNC MAIN
        static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Bot is starting...");
            
            // Build Discord client
            _discord = new DiscordClient(new DiscordConfiguration {
                Token = Config.Token,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });
            
            _commands = _discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefix = Config.CommandPrefix
            });

            _commands.RegisterCommands<Commands>();
            _commands.RegisterCommands<AdminCommands>();
            
            await _discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
