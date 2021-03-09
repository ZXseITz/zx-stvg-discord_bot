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
        private SocketGuild _guild;
        private SemaphoreSlim _signal;

        private ulong _guildId;
        public readonly ulong UserId;

        public Bot()
        {
            _client = new DiscordSocketClient();
            _guildId = ulong.Parse(File.ReadAllText("../../../secret/dev_id"));
            UserId = ulong.Parse(File.ReadAllText("../../../secret/user_id"));
            // var guildId = ulong.Parse(File.ReadAllText("../../../secret/guild_id"));  // todo enable prod
            _signal = new SemaphoreSlim(0, 1);

            _client.Ready += () =>
            {
                _signal.Release();
                return null;
            };
        }

        public async Task<IReadOnlyCollection<SocketGuildChannel>> Login()
        {
            var token = File.ReadAllText("../../../secret/bot_token");
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await _signal.WaitAsync();
            _guild = _client.GetGuild(_guildId);
            return _guild.Channels;
        }
    }
}