using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;

namespace client.data
{
    public class ChannelModel : AbstractModel
    {
        private readonly ITextChannel _channel;

        public string Name => _channel.Name;

        public ChannelModel(ITextChannel channel)
        {
            _channel = channel;
        }

        public async Task<IUserMessage> SendMessage(string text)
        {
            return await _channel.SendMessageAsync(text);
        }

        public async Task<IEnumerable<IMessage>> FetchMessages()
        {
            var collection = await _channel.GetMessagesAsync().FlattenAsync();
            return collection.Reverse();
        }
    }
}