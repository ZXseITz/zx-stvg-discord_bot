using System.Collections.ObjectModel;
using Discord;

namespace client.data
{
    public class RootModel : AbstractModel
    {
        public ObservableCollection<ServerModel> Servers { get; }
        public ObservableCollection<MessageModel> Messages { get; }

        public RootModel()
        {
            Servers = new ObservableCollection<ServerModel>();
            Messages = new ObservableCollection<MessageModel>();
        }
    }
}