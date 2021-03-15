using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace client
{
    public class Bot
    {
        private readonly DiscordSocketClient _client;
        private SemaphoreSlim _signal;

        public readonly ulong UserId;

        public Bot()
        {
            _client = new DiscordSocketClient();
            UserId = ulong.Parse(File.ReadAllText("../../../secret/user_id"));
            _signal = new SemaphoreSlim(0, 1);

            _client.Ready += () =>
            {
                _signal.Release();
                return null;
            };
        }

        public async Task<IReadOnlyCollection<SocketGuild>> Login()
        {
            var token = File.ReadAllText("../../../secret/bot_token");
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await _signal.WaitAsync();
            return _client.Guilds;
        }
    }
}