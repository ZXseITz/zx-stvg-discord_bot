using System.Collections.Generic;
using System.Collections.ObjectModel;
using Discord.Rest;
using Discord.WebSocket;

namespace client
{
    public class Model
    {
        public ObservableCollection<SocketTextChannel> Channels { get; }
        public ObservableCollection<RestUserMessage> Messages { get; }

        public Model()
        {
            Channels = new ObservableCollection<SocketTextChannel>();
            Messages = new ObservableCollection<RestUserMessage>();
        }
    }
}