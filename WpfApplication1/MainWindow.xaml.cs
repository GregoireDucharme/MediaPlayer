using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;

namespace mediaPlayer
{
    public partial class MainWindow : Window
    {
        ModelWMP btn = new ModelWMP();
        public class Media
        {
            public string path;
            public string name;
        }
        List<Media> listMedia = new List<Media>();
        private string RootRepo = ConfigurationManager.AppSettings.Get("RootRepo");
        private string PublicRepo = ConfigurationManager.AppSettings.Get("PublicRepo");

        public MainWindow()
        {
            InitializeComponent();
            DataContext = btn;
        }
        private void ListBox_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)sender;
            ModelWMP.media current = (ModelWMP.media)item.Content;
            btn.Source = current.Uri;

            mediaElement.Play();
            btn.NextState = true;
            btn.PlayState = false;
        }

        private void Action_File(object sender, RoutedEventArgs e)
        {
            mediaElement.LoadedBehavior = MediaState.Manual;
            switch (sender.ToString())
            {
                case "System.Windows.Controls.Button: Play":
                    btn.PlayState = false;
                    mediaElement.Play();
                    break;
                case "System.Windows.Controls.Button: Pause":
                    btn.PauseState = false;
                    mediaElement.Pause();
                    break;
                case "System.Windows.Controls.Button: Stop":
                    btn.StopState = false;
                    mediaElement.Stop();
                    break;
                case "System.Windows.Controls.Button: prec":
                    btn.StopState = false;
                    mediaElement.Stop();
                    break;
                case "System.Windows.Controls.Button: next":
                    btn.StopState = false;
                    mediaElement.Stop();
                    break;
            }
        }
        private void Change_Timeline(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            btn.TimeTxt = "set";
        }

        private void End_Timeline(object sender, DragCompletedEventArgs args)
        {
            int t = (int)timeline.Value;

            mediaElement.Position = TimeSpan.FromMilliseconds(t);
        }
        private void Mouse_Volume(object sender, MouseWheelEventArgs e)
        {
            btn.Volume += (e.Delta > 0) ? 0.1 : -0.1;
            btn.Volume = (btn.Volume > 1) ? 1 : btn.Volume;
            btn.Volume = (btn.Volume < 0) ? 0 : btn.Volume;
        }

        private void validateName(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string rec = playlist_name.Text;
                btn.SetPlaylistNameVisibility = false;

                TextWriter writer = new StreamWriter(rec + ".xml");
                XmlSerializer ser = new XmlSerializer(typeof(XmlElement));
                XmlElement myElement = new XmlDocument().CreateElement("MyElement", "ns");
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
                    label.Content = dir.Substring(index + 1);
                    penel.Children.Add(media);
                    penel.Children.Add(label);
                    listBoxMediaForPlaylist.Items.Add(penel);
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
                    listBoxMediaForPlaylist.Items.Add(penel);
                }
                listBoxMediaForPlaylist.Visibility = Visibility.Visible;
                //Import.Visibility = Visibility.Collapsed;
                // btn.CurrentTab = 3;
                // btn.Visibility.visibility = hidden;
                XmlSerializer xs = new XmlSerializer(typeof(List<Media>));
                using (StreamWriter wr = new StreamWriter("contacteBook.xml"))
                {
                    xs.Serialize(wr, listMedia);
                }
                //listMedia.Clear();
            }
        }
        private void ListBoxMediaForPlaylist_MouseDoubleClick(object sender, RoutedEventArgs e)
        {

            StackPanel current = (StackPanel)listBoxPlaylist.Items[listBoxPlaylist.SelectedIndex];
            /*MediaElement media = (MediaElement)current.Children[0];
            string file = media.Source.AbsoluteUri;
            btn.Source = new Uri(new Uri(file).LocalPath);
            mediaElement.Play();*/
            Media newMedia = new Media();
            newMedia.path = "dir";
            newMedia.name = "cachou";
            listMedia.Add(newMedia);
        }
        private void ListBoxPlaylist_MouseDoubleClick(object sender, RoutedEventArgs e)
        {

        }
        private void Get_Len(object sender, RoutedEventArgs e)
        {
            MediaElement tmp = (MediaElement)sender;
            if (tmp.NaturalDuration.HasTimeSpan)
            {
                btn._Len = tmp.NaturalDuration.TimeSpan.TotalMilliseconds;
                if (btn.Timer != 0)
                    tmp.Position = TimeSpan.FromMilliseconds(btn.Timer);
            }
        }
    }
}
