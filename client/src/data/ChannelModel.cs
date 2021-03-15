using System.Collections.Generic;
using System.IO;
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

        public async Task<IUserMessage> SendFile(string path, string text)
        {
            using (var fs = File.OpenRead(path))
            {
                return await _channel.SendFileAsync(fs, Path.GetFileName(path), text);
            }
        }

        public async Task<IEnumerable<IMessage>> FetchMessages()
        {
            var collection = await _channel.GetMessagesAsync().FlattenAsync();
            return collection.Reverse();
        }
    }
}