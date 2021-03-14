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

        public async Task<MessageModel> SendMessage(string text)
        {
            var userMessage = await _channel.SendMessageAsync(text);
            return new MessageModel(userMessage);
        }

        public async Task<List<MessageModel>> GetMessagesByUser(ulong userId)
        {
            var result = new List<MessageModel>();
            var collection = await _channel.GetMessagesAsync().FlattenAsync();
            var enumerator = collection.Reverse().GetEnumerator();
            while (enumerator.MoveNext())
            {
                var message = enumerator.Current;
                if (message is IUserMessage userMessage && message.Author.Id == userId)
                {
                    result.Add(new MessageModel(userMessage));
                }
            }
            enumerator.Dispose();
            return result;
        }
    }
}