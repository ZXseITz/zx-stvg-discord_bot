﻿using System;
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
            OnClear(this, new RoutedEventArgs());
        }

        private void OnClear(object sender, RoutedEventArgs e)
        {
            _selectedMessage = null;
            WriteUserText("");
        }

        private string ReadUserText()
        {
            var document = _input.Document;
            var range = new TextRange(document.ContentStart, document.ContentEnd);
            return range.Text;
        }
        
        private void WriteUserText(string text)
        {
            var document = _input.Document;
            var range = new TextRange(document.ContentStart, document.ContentEnd);
            range.Text = text;
        }

        private void OnMessageSelected(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.AddedItems.Count > 0 && e.AddedItems[0] is IUserMessage message)) return;
            _selectedMessage = message;
            WriteUserText(message.Content);
        }
        
        private async void OnSend(object sender, RoutedEventArgs e)
        {
            var text = ReadUserText();
            if (_selectedMessage != null)
            {
                // update message
                await _selectedMessage.ModifyAsync(message => message.Content = text);
                _selectedMessage = null;
                // todo update message content automatically
            }
            else
            {
                // send new message
                var message = await _selectedChannel.SendMessageAsync(text);
                _model.Messages.Add(message);
            }
        }
    }
}
