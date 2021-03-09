using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Discord;
using Discord.WebSocket;

namespace client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bot _bot;
        private Model _model;
        private RichTextBox _input;
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
            _input = FindName("Input") as RichTextBox;
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
            var channel = e.AddedItems[0] as ITextChannel;
            _selectedChannel = channel;
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
        }

        private void OnMessageSelected(object sender, SelectionChangedEventArgs e)
        {
            var message = e.AddedItems[0] as IUserMessage;
            var document = _input.Document;
            var range = new TextRange(document.ContentStart, document.ContentEnd);
            range.Text = message.Content;
        }
        
        private async void OnSend(object sender, RoutedEventArgs e)
        {
            var document = _input.Document;
            var range = new TextRange(document.ContentStart, document.ContentEnd);
            var message = await _selectedChannel.SendMessageAsync(range.Text);
            _model.Messages.Add(message);
        }
    }
}
