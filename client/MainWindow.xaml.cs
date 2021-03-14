using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using client.data;
using Discord;

namespace client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bot _bot;
        private RootModel _model;
        private TextBox _input;
        private ChannelModel _selectedChannel;
        private MessageModel _selectedMessage;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OnStartup(object sender, EventArgs e)
        {
            _model = FindResource("Model") as RootModel;
            if (_model == null) throw new Exception("Cannot find model");
            _input = FindName("Input") as TextBox;
            if (_input == null) throw new Exception("Cannot find input field");
            _bot = new Bot();
            var guilds = await _bot.Login();
            foreach (var guild in guilds)
            {
                var botUser = guild.GetUser(_bot.UserId);
                var role = guild.Roles.FirstOrDefault(role1 => role1.Name.Equals(botUser.Username));
                if (role == null) continue;
                var server = new ServerModel(guild);
                foreach (var channel in guild.Channels)
                {
                    if (channel is ITextChannel textChannel
                        && channel.PermissionOverwrites.Count > 0
                        && channel.PermissionOverwrites.Any(permission => permission.TargetId == role.Id))
                    {
                        server.Channels.Add(new ChannelModel(textChannel));
                    }
                }
                _model.Servers.Add(server);
            }
        }

        private void ClearUserText()
        {
            _selectedMessage = null;
            _input.Text = "";
        }

        private async void OnChannelSelected(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is ChannelModel channel)
            {
                _model.Messages.Clear();
                var messages = await channel.FetchMessages();
                var enumerator =  messages.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var message = enumerator.Current;
                    if (message is IUserMessage userMessage && message.Author.Id == _bot.UserId)
                    {
                        _model.Messages.Add(new MessageModel(userMessage));
                    }
                }
                enumerator.Dispose();
                _selectedChannel = channel;
                ClearUserText();
            }
        }

        private void OnMessageSelected(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.AddedItems.Count > 0 && e.AddedItems[0] is MessageModel message)) return;
            _selectedMessage = message;
            _input.Text = message.Text;
        }

        private void OnEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift))
            {
                OnSend(sender, e);
                e.Handled = true;
            }
        }
        
        private async void OnSend(object sender, RoutedEventArgs e)
        {
            var text = _input.Text;
            if (_selectedMessage != null)
            {
                // update message
                await _selectedMessage.Edit(text);
                // todo update message content automatically
            }
            else
            {
                // send new message
                var message = await _selectedChannel.SendMessage(text);
                _model.Messages.Add(new MessageModel(message));
                
            }
            ClearUserText();
        }

        private void OnClear(object sender, RoutedEventArgs e)
        {
            ClearUserText();
        }

        private async void OnDelete(object sender, RoutedEventArgs e)
        {
            if (_selectedMessage != null)
            { 
                _model.Messages.Remove(_selectedMessage);
                await _selectedMessage.Delete();
            }
            ClearUserText();
        }
    }
}
