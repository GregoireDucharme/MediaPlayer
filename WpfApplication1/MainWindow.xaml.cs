using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace WpfApplication1
{
    public class ButtonMedia : INotifyPropertyChanged
    {
        private int currentTab = 0;
        public int CurrentTab
        {
            get
            {
                return currentTab;
            }
            set
            {
                currentTab = value;
                OnPropertyChanged("CurrentTab");
            }
        }
        DispatcherTimer _timer = new DispatcherTimer();
        private int timer = 0;
        public double _len;
        private Uri source;
        private Visibility optionVisi = Visibility.Hidden;
        public Visibility OptionVisi
        {
            get
            {
                return optionVisi;
            }
            set
            {
                optionVisi = value;
                OnPropertyChanged("OptionVisi");
            }
        }
            
        public Uri Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
                OnPropertyChanged("Source");
                CurrentTab = 0;
                OptionVisi = Visibility.Visible;
            }
        }
        public int Timer
        {
            get
            {
                return timer;
            }
            set
            {
                if (value == 0)
                    timer = 0;
                if (timer == 0)
                    _timer.Start();
                timer = value;
                if (timer >= _len)
                {
                    timer = 0;
                    _timer.Stop();
                }
                OnPropertyChanged("Timer");
            }
        }
        private void _actualisation(object sender, EventArgs e)
        {
            Timer += 1000;
        }

        private bool playState = false;
        private bool pauseState = true;
        private bool stopState = true;
        public ButtonMedia()
        {
            OnPropertyChanged("PlayState");
            OnPropertyChanged("PauseState");
            OnPropertyChanged("StopState");
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += new EventHandler(_actualisation);
        }
        private void _trigger()
        {
            OnPropertyChanged("PlayState");
            OnPropertyChanged("PauseState");
            OnPropertyChanged("StopState");
        }
        public bool PlayState
        {
            get
            {
                return playState;
            }
            set
            {
                playState = value;
                pauseState = true;
                stopState = true;
                _timer.Start();
                _trigger();
            }
        }
        public bool PauseState
        {
            get
            {
                return pauseState;
            }
            set
            {
                playState = true;
                pauseState = value;
                stopState = true;
                _timer.Stop();
                _trigger();
            }
        }
        public bool StopState
        {
            get
            {
                return stopState;
            }
            set
            {
                playState = true;
                pauseState = value;
                stopState = value;
                _timer.Stop();
                timer = 0;
                OnPropertyChanged("Timer");
                _trigger();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }
    }
    public partial class MainWindow : Window
    {
        ButtonMedia btn = new ButtonMedia();
        public class Media
        {
            public string path;
            public string name;
        }
        List<Media> listMedia = new List<Media>();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = btn;
            handling_files();
            handling_filesXml();
        }
        public void handling_filesXml()
        {
            string folder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string[] dirs = Directory.GetFiles(folder, "*.xml" , SearchOption.AllDirectories);
            foreach (string dir in dirs)
            {
                StackPanel penel = new StackPanel();
                Label label = new Label();
                penel.Orientation = Orientation.Horizontal;
                penel.Margin = new Thickness(10, 10, 0, 10);
                label.Margin = new Thickness(10, 20, 0, 0);
                penel.Height = 50;
                penel.Width = 700;
                int index = dir.LastIndexOf('\\');
                label.Content = dir.Substring(index + 1);
                penel.Children.Add(label);
                listBoxPlaylist.Items.Add(penel);
            }
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
                label.Content = dir.Substring(index + 1);
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
            btn.Source = new Uri(new Uri(file).LocalPath);
            mediaElement.Play();
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
            }
        }
        private void Change_Timeline(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            time.Text = TimeSpan.FromMilliseconds(timeline.Value).ToString(@"hh\:mm\:ss");
        }

        private void End_Timeline(object sender, DragCompletedEventArgs args)
        {
            int t = (int)timeline.Value;

            mediaElement.Position = TimeSpan.FromMilliseconds(t);
        }
        private void Change_Volume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            mediaElement.Volume = (double)volume.Value;
        }
        private void Mouse_Volume(object sender, MouseWheelEventArgs e)
        {
            mediaElement.Volume += (e.Delta > 0) ? 0.1 : -0.1;
            mediaElement.Volume = (mediaElement.Volume > 1) ? 1 : mediaElement.Volume;
            mediaElement.Volume = (mediaElement.Volume < 0) ? 0 : mediaElement.Volume;
        }
        private void Create_Playlist(object sender, RoutedEventArgs e)
        {
            SetPlaylistName.Visibility = Visibility.Visible;
            Create_button.Visibility = Visibility.Hidden;
            Import_button.Visibility = Visibility.Hidden;
        }
        private void validateName(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string rec = playlist_name.Text;
                SetPlaylistName.Visibility = Visibility.Hidden;

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
    }
}
