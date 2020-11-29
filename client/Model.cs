using System.Collections.ObjectModel;
using Discord;

namespace client
{
    public class Model
    {
        public ObservableCollection<ITextChannel> Channels { get; }
        public ObservableCollection<IMessage> Messages { get; }

        public Model()
        {
            Channels = new ObservableCollection<ITextChannel>();
            Messages = new ObservableCollection<IMessage>();
        }
    }
}