using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Discord;

namespace client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bot _bot;
        private Model _model;
        private TextBox _input;
        private ITextChannel _selectedChannel;
        private IUserMessage _selectedMessage;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OnStartup(object sender, EventArgs e)
        {
            _model = FindResource("Model") as Model;
            if (_model == null) throw new Exception("Cannot find model");
            _input = FindName("Input") as TextBox;
            if (_input == null) throw new Exception("Cannot find input field");
            _bot = new Bot();
            var channels = await _bot.Login();
            foreach (var channel in channels)
            {
                if (channel is ITextChannel textChannel)
                {
                    _model.Channels.Add(textChannel);
                }
            }
        }

        private async void OnChannelSelected(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.AddedItems.Count > 0 && e.AddedItems[0] is ITextChannel channel)) return;
            _model.Messages.Clear();
            var collection = await channel.GetMessagesAsync().FlattenAsync();
            var enumerator = collection.Reverse().GetEnumerator();
            while (enumerator.MoveNext())
            {
                var message = enumerator.Current;
                if (message is IUserMessage && message.Author.Id == _bot.UserId)
                {
                    _model.Messages.Add(message);
                }
            }
            enumerator.Dispose();
            _selectedChannel = channel;
            ClearUserText();
        }

        private void OnClear(object sender, RoutedEventArgs e)
        {
            ClearUserText();
        }

        private void ClearUserText()
        {
            _selectedMessage = null;
            _input.Text = "";
        }

        private void OnMessageSelected(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.AddedItems.Count > 0 && e.AddedItems[0] is IUserMessage message)) return;
            _selectedMessage = message;
            _input.Text = message.Content;
        }
        
        private async void OnSend(object sender, RoutedEventArgs e)
        {
            var text = _input.Text;
            if (_selectedMessage != null)
            {
                // update message
                await _selectedMessage.ModifyAsync(message => message.Content = text);
                // todo update message content automatically
            }
            else
            {
                // send new message
                var message = await _selectedChannel.SendMessageAsync(text);
                _model.Messages.Add(message);
                
            }
            ClearUserText();
        }

        private void OnEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift))
            {
                OnSend(sender, e);
                e.Handled = true;
            }
        }
    }
}
