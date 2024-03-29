﻿using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;

namespace Litio.Commands
{
    [SlashCommand("toggleantiraid", "Toggle anti-raid on/off.")]
    public class ToggleAntiRaidCmd : SlashCommand
    {
        [SlashParameter("enabled", "Value to specify if the anti-raid is enabled or not.", true)]
        public bool Toggled { get; private set; }

        private DiscordEmbed CreateEmbed(Color Color, string AuthorName, string Description)
        {
            return new EmbedMaker()
            {
                Color = Color,
                Author = new EmbedAuthor()
                {
                    Name = AuthorName,
                    IconUrl = Caller.Avatar != null ? Caller.Avatar.Url : Client.User.Avatar.Url
                },
                Description = Description,
                Footer = new EmbedFooter()
                {
                    Text = "Litio",
                },
                Timestamp = DateTime.Now
            };
        }

        public override InteractionResponseProperties Handle()
        {
            try
            {
                if (!CallerMember.GetPermissions().Has(DiscordPermission.ManageThreads))
                {
                    return new InteractionResponseProperties()
                    {
                        Embed = CreateEmbed(Utils.Error, "Missing permissions.", $"You don't have the required permissions to execute that command."),
                        Ephemeral = true
                    };
                }

                Utils.Guilds.Find(x => x.GuildId == Guild.Id.ToString()).Toggled = Toggled;
                File.WriteAllText("Database.txt", JsonConvert.SerializeObject(Utils.Guilds));
                return new InteractionResponseProperties()
                {
                    Embed = CreateEmbed(Utils.Success, Client.GetGuild(Guild.Id).Name, "Succesfully set the anti-raid `" + (Toggled ? "on" : "off") + "`."),
                    Ephemeral = false
                };
            }
            catch
            {
                return new InteractionResponseProperties()
                {
                    Embed = CreateEmbed(Utils.Error, "Error occurred.", "An unknown error has occurred, we apologize for the inconvenience."),
                    Ephemeral = true
                };
            }
        }
    }
}
