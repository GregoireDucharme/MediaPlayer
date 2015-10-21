using System;
using System.Reflection;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using System.Configuration;
namespace mediaPlayer
{
    public class ButtonMedia : INotifyPropertyChanged
    {
        private String timeTxt = "00:00:00";
        public String TimeTxt
        {
            get
            {
                return timeTxt;
            }
            set
            {
                timeTxt = TimeSpan.FromMilliseconds(timer).ToString(@"hh\:mm\:ss");
                OnPropertyChanged("TimeTxt");
            }
        }
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
        private double _len = 0;
        public double _Len
        {
            get
            {
                return _len;
            }
            set
            {
                if (chck == true)
                {
                    _len = value;
                    Timer = 0;
                    chck = false;
                }
            }
        }
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

        private bool chck;
        public Uri Source
        {
            get
            {
                return source;
            }
            set
            {
                if (value != source)
                {
                    chck = true;
                    source = value;
                    OnPropertyChanged("Source");
                    CurrentTab = 0;
                    OptionVisi = Visibility.Visible;
                }
                else
                    chck = false;
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
        private string RootRepo = ConfigurationManager.AppSettings.Get("RootRepo");
        private string PublicRepo = ConfigurationManager.AppSettings.Get("PublicRepo");
        public MainWindow()
        {
            InitializeComponent();
            DataContext = btn;
            handling_files();
        }

        private void _fill_list(string dir, char box)
        {
            StackPanel penel = new StackPanel();
            MediaElement media = new MediaElement();
            TextBlock infos = new TextBlock();
            MediaElement tn = new MediaElement();
            media.Source = new Uri(new Uri(dir).LocalPath);
            penel.Orientation = Orientation.Horizontal;
            penel.Margin = new Thickness(10, 10, 0, 10);
            infos.Margin = new Thickness(10, 10, 0, 10);
            infos.Height = 100;
            infos.Width = 700;
            penel.Height = 76;
            penel.Width = 700;
            int index = dir.LastIndexOf('\\');
            string filename = dir.Substring(index + 1);
            switch (box)
            {
                // IMAGES
                case 'i':
                    infos.Text = filename;
                    media.Width = 100;
                    media.Height = 120;
                    penel.Children.Add(media);
                    penel.Children.Add(infos);
                    listBoxImages.Items.Add(penel);
                    break;
                // VIDEOS
                case 'v':
                    infos.Text = filename;
                    media.Visibility = Visibility.Collapsed;
                    tn.Source = new Uri(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Images\film.jpg");
                    tn.Width = 100;
                    tn.Height = 120;
                    penel.Children.Add(media);
                    penel.Children.Add(tn);
                    penel.Children.Add(infos);
                    listBoxVideos.Items.Add(penel);
                    break;
                // MUSIQUES
                case 'm':
                    infos.Text = filename;
                    media.LoadedBehavior = MediaState.Pause;
                    media.Visibility = Visibility.Collapsed;
                    tn.Source = new Uri(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Images\music.jpg");
                    tn.Width = 76;
                    tn.Height = 100;
                    penel.Children.Add(media);
                    penel.Children.Add(tn);
                    penel.Children.Add(infos);
                    listBoxMusiques.Items.Add(penel);
                    break;
                }
            }

        private void _get_files(string filePM, string type0, string type1, string type2, char box)
        {
            var files = Directory.EnumerateFiles(RootRepo + Environment.UserName + filePM, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith("." + type0, StringComparison.OrdinalIgnoreCase) || s.EndsWith("." + type1, StringComparison.OrdinalIgnoreCase) ||
            s.EndsWith("." + type2, StringComparison.OrdinalIgnoreCase));
            foreach (string dir in files)
            {
                _fill_list(dir, box);
            }
            files = Directory.EnumerateFiles(RootRepo + PublicRepo + filePM, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith("." + type0, StringComparison.OrdinalIgnoreCase) || s.EndsWith("." + type1, StringComparison.OrdinalIgnoreCase) ||
            s.EndsWith("." + type2, StringComparison.OrdinalIgnoreCase));
            foreach (string dir in files)
            {
                _fill_list(dir, box);
            }
        }

        public void handling_files()
        {

            try
            {
                // IMAGES
                _get_files(@"\Pictures", "jpg", "png", "gif", 'i');

                // VIDEOS
                _get_files(@"\Videos", "mp4", "avi", "wmv", 'v');

                // MUSIQUES
                _get_files(@"\Music", "mp3", "wav", "wma", 'm');
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (PathTooLongException)
            {
            }
            catch (DirectoryNotFoundException)
            {
            }

        }
        private void ListBox_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)sender;
            StackPanel current = (StackPanel)item.Content;
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
            btn.TimeTxt = "set";
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
        private void Get_Len(object sender, RoutedEventArgs e)
        {
            if (mediaElement.NaturalDuration.HasTimeSpan)
            {
                timeline.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
                btn._Len = mediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
                if (btn.Timer != 0)
                    mediaElement.Position = TimeSpan.FromMilliseconds(btn.Timer);
            }
        }
    }
}
