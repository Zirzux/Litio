﻿using Discord;
using Discord.Gateway;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Litio
{
    public static class Extensions
    {
        public static void TimeoutUser(this DiscordSocketClient client, ulong guildId, ulong memberId, TimeSpan duration)
        {
            client.HttpClient.PatchAsync($"https://discord.com/api/v9/guilds/{guildId}/members/{memberId}", JsonConvert.SerializeObject(new
            {
                communication_disabled_until = DateTime.Now + duration,
            }));
        }

        public static void DeleteThread(this DiscordSocketClient client, ulong threadId)
        {
            client.HttpClient.DeleteAsync($"https://discord.com/api/v9/channels/{threadId}");
        }

        public static IReadOnlyList<DiscordThread> GetChannelArchiviedThreads(this DiscordSocketClient client, ulong channelId)
        {
            return client.HttpClient.GetAsync($"https://discord.com/api/v9/channels/{channelId}/threads/archived/public").GetAwaiter().GetResult().Body.Value<JToken>("threads").ToObject<List<DiscordThread>>(); ;
        }
    }
}
