using System.Collections.ObjectModel;
using Discord;
using Discord.WebSocket;

namespace client.data
{
    public class ServerModel : AbstractModel
    {
        private readonly SocketGuild _guild;
        
        public ObservableCollection<ITextChannel> Channels { get; }
        
        public string Name => _guild.Name;

        public ServerModel(SocketGuild guild)
        {
            _guild = guild;
            Channels = new ObservableCollection<ITextChannel>();
        }
    }
}