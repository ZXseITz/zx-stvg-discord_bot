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
        private RichTextBox textbox;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnStartup(object sender, EventArgs e)
        {
            textbox = FindName("Input") as RichTextBox;
        }


        private void OnSend(object sender, RoutedEventArgs e)
        {
            var range = new TextRange(textbox.Document.ContentStart, textbox.Document.ContentEnd);
            var text = range.Text;
        }
    }
}
