using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    class Bot
    {
        private DiscordSocketClient _client;
        private string _token;
        private ulong _channelId;

        public Bot()
        {
            _client = new DiscordSocketClient();
            _token = File.ReadAllText("../../../secret/bot_token");
            _channelId = ulong.Parse(File.ReadAllText("../../../secret/channel_id"));
        }

        public async Task Login()
        {
            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
        }

        public async Task Send(string text)
        {
            var channel = _client.GetChannel(_channelId) as SocketTextChannel;
            await channel.SendMessageAsync(text);
        } 
    }
}
