using Discord;
using Discord.WebSocket;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Discord.Rest;

namespace client
{
    public class Bot
    {
        private readonly DiscordSocketClient _client;
        private readonly Model _model;
        private SocketGuild _guild;
        private ITextChannel _channel;

        private ulong _guildId;
        private ulong _userId;

        public Bot(Model model)
        {
            _client = new DiscordSocketClient();
            _model = model;
            _userId = ulong.Parse(File.ReadAllText("../../../secret/user_id"));
            _guildId = ulong.Parse(File.ReadAllText("../../../secret/dev_id"));
            // var guildId = ulong.Parse(File.ReadAllText("../../../secret/guild_id"));  // todo enable prod

            _client.Ready += () => OnReady();
        }

        private Task OnReady()
        {
            // Console.WriteLine("Bot is ready");
            _guild = _client.GetGuild(_guildId);
            var channels = _guild.Channels;
            Application.Current?.Dispatcher.InvokeAsync(() =>
            {
                foreach (var channel in channels)
                {
                    if (channel is ITextChannel textChannel)
                    {
                        _model.Channels.Add(textChannel);
                    }
                }
            });
            return null;
        }

        public async Task Login()
        {
            var token = File.ReadAllText("../../../secret/bot_token");
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
        }

        public async Task SetChannel(ITextChannel channel)
        {
            _channel = channel;
            _model.Messages.Clear();
            var collection = await channel.GetMessagesAsync().FlattenAsync();
            var enumerator = collection.Reverse().GetEnumerator();
            while (enumerator.MoveNext())
            {
                var message = enumerator.Current;
                if (message is IUserMessage && message.Author.Id == _userId)
                {
                    _model.Messages.Add(message);
                }
            }
            enumerator.Dispose();
        }

        public async Task Send(string text)
        {
            var msg = await _channel.SendMessageAsync(text);
            _model.Messages.Add(msg);
        }
    }
}