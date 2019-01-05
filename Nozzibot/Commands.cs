using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using Nozzibot.Helpers;
using Nozzibot.Models;

namespace Nozzibot
{
    class Commands
    {
        // Quote command
        [Command("week")]
        [System.ComponentModel.Description("Displays a random quote")]
        public async Task Quote(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = "Here's the weekly schedule:",
                Color = new DiscordColor(0, 214, 0)
            };

            var events = Event.GetFromFile();

            foreach (Event e in events)
            {
                embed.AddField(
                    $"{e.Day} at {e.Time}", 
                    e.Description
                );
            }

            await ctx.RespondAsync(null, false, embed);

        }

        
        // Article search command
        [Command("day")]
        [System.ComponentModel.Description("Searches for an article")]
        public async Task Article(CommandContext ctx, [RemainingText] string query)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = "Here's the weekly schedule:",
                Color = new DiscordColor(0, 214, 0)
            };

            var events = Event.GetFromFile("events.json", query.Substring(0, 3).FirstCharToUpper());

            foreach (Event e in events)
            {
                embed.AddField(
                    $"{e.Day} at {e.Time}", 
                    e.Description
                );
            }

            await ctx.RespondAsync(null, false, embed);
        }
        
    }

//    [Group("admin")] // let's mark this class as a command group
//    [System.ComponentModel.Description("Administrative commands.")] // give it a description for help purposes
    [Hidden] // let's hide this from the eyes of curious users
    [RequirePermissions(Permissions.ManageGuild)]
    // and restrict this to users who have appropriate permissions
    public class AdminCommands
    {

        // Add event
        [Command("add")]
        [System.ComponentModel.Description("``DDD HH:MM Description`` (``Sat 16:30 \"Chillin'\"``)")]
        public async Task AddEvent(CommandContext ctx, [RemainingText] string query)
        {
            var events = Event.GetFromFile();

            var parts = Regex.Matches(query, @"[\""].+?[\""]|[^ ]+")
                .Select(m => m.Value)
                .ToList();

            var ev = new Event(parts[0], parts[1], parts[2]);   

            events.Add(ev);
            
            string json = JsonConvert.SerializeObject(events, Formatting.Indented);
            System.IO.File.WriteAllText("events.json", json);
            

            CConsole.WriteLine($"{ev.Day}, {ev.Time} event added: {ev.Description}", ConsoleColor.Cyan);
            await ctx.RespondAsync($"{ev.Day}, {ev.Time} event added: {ev.Description}");
        }

        // Add event
        [Command("del")]
        [System.ComponentModel.Description("``DDD HH:MM`` (``Sat 16:30``)")]
        public async Task DeleteEvent(CommandContext ctx, [RemainingText] string query)
        {
            var events = Event.GetFromFile();

            CConsole.WriteLine(events.Count().ToString(), ConsoleColor.DarkMagenta);

            try
            {
                var parts = query.Split(' ');

                CConsole.WriteLine(parts[0] + parts[1], ConsoleColor.DarkMagenta);

                var ev = events.Where(e => e.Day == Enum.Parse<Day>(parts[0]) && e.Time.ToString() == parts[1]).ToList()[0];

                
                CConsole.WriteLine(ev.Description, ConsoleColor.DarkMagenta);

                events.Remove(ev);
            
                string json = JsonConvert.SerializeObject(events, Formatting.Indented);
                System.IO.File.WriteAllText("events.json", json);
            

                CConsole.WriteLine($"{ev.Day}, {ev.Time} event removed: {ev.Description}", ConsoleColor.Yellow);
                await ctx.RespondAsync($"{ev.Day}, {ev.Time} event removed: {ev.Description}");
            }
            catch (Exception e)
            {
                CConsole.WriteLine(e.Message, ConsoleColor.DarkMagenta);
            }
        }

    }
}
