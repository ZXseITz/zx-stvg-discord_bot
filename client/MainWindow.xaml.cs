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

namespace client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RichTextBox _textbox;
        private Bot _bot;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OnStartup(object sender, EventArgs e)
        {
            _textbox = FindName("Input") as RichTextBox;
            _bot = new Bot();
            await _bot.Login();
        }


        private async void OnSend(object sender, RoutedEventArgs e)
        {
            var doc = _textbox.Document;
            var range = new TextRange(doc.ContentStart, doc.ContentEnd);
            await _bot.Send(range.Text);
        }
    }
}
