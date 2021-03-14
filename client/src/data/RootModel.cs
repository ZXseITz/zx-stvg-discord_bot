using System.Collections.ObjectModel;
using System.Windows.Documents;
using Discord;
using Discord.WebSocket;

namespace client.data
{
    public class RootModel : AbstractModel
    {
        public ObservableCollection<ServerModel> Servers { get; }
        public ObservableCollection<IMessage> Messages { get; }

        public RootModel()
        {
            Servers = new ObservableCollection<ServerModel>();
            Messages = new ObservableCollection<IMessage>();
        }
    }
}