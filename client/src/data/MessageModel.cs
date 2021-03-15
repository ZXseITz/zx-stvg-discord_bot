using System.Linq;
using System.Threading.Tasks;
using Discord;

namespace client.data
{
    public class MessageModel: AbstractModel
    {
        private readonly IUserMessage _message;

        public string Text => _message.Content;

        public MessageModel(IUserMessage message)
        {
            _message = message;
        }

        public async Task Edit(string text)
        {
            await _message.ModifyAsync(props => props.Content = text);
            OnPropertyChanged("Text");
        }

        public async Task Delete()
        {
            await _message.DeleteAsync();
        }
    }
}