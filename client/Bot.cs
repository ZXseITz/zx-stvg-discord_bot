using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace client
{
    public class Bot
    {
        private readonly DiscordSocketClient _client;
        private readonly Model _model;
        private string _token;
        private ulong _guildId;
        private ulong _channelId;

        public Bot(Model model)
        {
            _client = new DiscordSocketClient();
            _model = model;
            _token = File.ReadAllText("../../../secret/bot_token");
            _guildId = ulong.Parse(File.ReadAllText("../../../secret/guild_id"));
            _channelId = ulong.Parse(File.ReadAllText("../../../secret/channel_id"));

            _client.Ready += () => OnReady();
        }

        private Task OnReady()
        {
            Console.WriteLine("Bot is ready");
            var guild = _client.GetGuild(_guildId);
            var channels = guild.Channels;
            Application.Current?.Dispatcher.InvokeAsync(() =>
            {
                foreach (var channel in channels)
                {
                    if (channel is SocketTextChannel textChannel)
                    {
                        _model.Channels.Add(textChannel);
                    }
                }
            });
            return null;
        }

        public async Task Login()
        {
            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
        }

        public async Task Send(string text)
        {
            var channel = _client.GetChannel(_channelId) as SocketTextChannel;
            await channel?.SendMessageAsync(text);
        }
    }
}
