using System;
using System.IO;
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

namespace WpfApplication1
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            handling_files();
        }
        public void handling_files()
        {   
            string[] dirs = Directory.GetFiles(@"C:\Users\" + Environment.UserName + @"\Pictures", "*.jpg", SearchOption.AllDirectories);
            foreach (string dir in dirs)
            {
                StackPanel penel = new StackPanel();
                MediaElement media = new MediaElement();
                Label label = new Label();
                media.Source = new Uri(new Uri(dir).LocalPath);
                media.Width = 100;
                media.Height = 120;
                penel.Orientation = Orientation.Horizontal;
                penel.Margin = new Thickness(10, 10, 0, 10);
                label.Margin = new Thickness(10, 20, 0, 0);
                penel.Height = 76;
                penel.Width = 700;
                int index = dir.LastIndexOf('\\');
                label.Content = dir.Substring(index+1);
                penel.Children.Add(media);
                penel.Children.Add(label);
                listBox.Items.Add(penel);
            }
            dirs = Directory.GetFiles(@"C:\Users\Public\Pictures", "*.jpg", SearchOption.AllDirectories);
            foreach (string dir in dirs)
            {
                StackPanel penel = new StackPanel();
                MediaElement media = new MediaElement();
                Label label = new Label();
                media.Source = new Uri(new Uri(dir).LocalPath);
                media.Width = 100;
                media.Height = 120;
                penel.Orientation = Orientation.Horizontal;
                penel.Margin = new Thickness(10, 10, 0, 10);
                label.Margin = new Thickness(10, 20, 0, 0);
                penel.Height = 76;
                penel.Width = 700;
                int index = dir.LastIndexOf('\\');
                label.Content = dir.Substring(index + 1);
                penel.Children.Add(media);
                penel.Children.Add(label);
                listBox.Items.Add(penel);
            }
        }
        private void ListBox_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            StackPanel current = (StackPanel)listBox.Items[listBox.SelectedIndex];
            MediaElement media = (MediaElement)current.Children[0];
            string file = media.Source.AbsoluteUri;
            mediaElement.Source = new Uri(new Uri(file).LocalPath);
            mediaElement.Play();
        }
    }
}
