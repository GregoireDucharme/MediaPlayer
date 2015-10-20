using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WpfApplication1
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    
    public class ButtonMedia : INotifyPropertyChanged
    {
        private bool playState = false;
        private bool pauseState = true;
        private bool stopState = true;
        public ButtonMedia()
        {
            OnPropertyChanged("PlayState");
            OnPropertyChanged("PauseState");
            OnPropertyChanged("StopState");
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
        public MainWindow()
        {
            InitializeComponent();
            DataContext = btn;    
        }

        private void Open_File(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            char type;
            MenuItem item = (MenuItem)sender;
            type = '0';
            switch (item.Header as string)
            {
                case "Video":
                    openFileDialog.Filter = "Videos (*.mp4;*.avi;*.wmv)|*.mp4;*.avi;*.wmv|All files (*.*)|*.*";
                    type = 'v';
                    break;
                case "Photo":
                    openFileDialog.Filter = "Images (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
                    type = 'i';
                    break;
                case "Musique":
                    openFileDialog.Filter = "Musiques (*.mp3;*.wav)|*.mp3;*.wav|All files (*.*)|*.*";
                    type = 'm';
                    break;
            }
            player.MediaOpened += new RoutedEventHandler(Get_Len);

            if (openFileDialog.ShowDialog() == true)
            {
                player.Source = new Uri(openFileDialog.FileName);
                player.Visibility = Visibility.Visible;
                if (type == 'm' || type == 'v')
                    options.Visibility = Visibility.Visible;
                else
                    options.Visibility = Visibility.Hidden;
                player.LoadedBehavior = MediaState.Manual;
                player.Play();
                player.Pause();
                player.Play();
            }
        }
        private void Get_Len(object sender, RoutedEventArgs e)
        {
            timeline.Maximum = player.NaturalDuration.TimeSpan.TotalMilliseconds;
        }
        private void Open_Playlist(object sender, RoutedEventArgs e)
        {

        }

        private void Open_Dir(object sender, RoutedEventArgs e)
        {
            ListView tmp = (ListView)sender;
            res.Visibility = Visibility.Visible;
            tmp.SelectedIndex = 0;
            res.Content = tmp.SelectedValue.ToString();
            GetFiles(tmp.SelectedValue.ToString(), "*.png", (ListView)sender);
        }

        private void GetFiles(string path, string pattern, ListView listPM)
        {
            var files = new List<string>();

            try
            {
                files.AddRange(Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly));
                foreach (string name in files)
                {
                    MediaElement media = new MediaElement();
                    media.Source = new Uri(new Uri(name).LocalPath);
                    media.Width = 20;
                    media.Height = 20;
                    listPM.Items.Add(media);
                }
                foreach (var directory in Directory.GetDirectories(path))
                {
                    if (!directory.Contains("."))
                    {
                        var newList = new ListView();
                        newList.MouseLeftButtonDown += Open_Dir;
                        // newList.Name = "paul";
                        newList.Items.Add(directory);
                        newList.Width = 115;
                        newList.Height = 35;
                        listPM.Items.Add(newList);
                        //GetFiles(directory, pattern, newList);
                    }
                }
            }
            catch (UnauthorizedAccessException) {
            }
            catch (PathTooLongException) {
            }
        }
        private void Import_Image(object sender, RoutedEventArgs e)
        {
            string userName = Environment.UserName;
            string path = @"c:\users\" + userName;
            string searchPattern = "*.png";
            GetFiles(path, searchPattern, list);
        }
        private void Action_File(object sender, RoutedEventArgs e)
        {
            player.LoadedBehavior = MediaState.Manual;
            switch (sender.ToString())
            {
                case "System.Windows.Controls.Button: Play":
                    btn.PlayState = false;
                    player.Play();
                    break;
                case "System.Windows.Controls.Button: Pause":
                    btn.PauseState = false;
                    player.Pause();
                    break;
                case "System.Windows.Controls.Button: Stop":
                    btn.StopState = false;
                    player.Stop();
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

            //TimeSpan ts = new TimeSpan(0, 0, 0, 0, t);
            player.Position = TimeSpan.FromMilliseconds(t);
        }
        private void Change_Volume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            player.Volume = (double)volume.Value;
        }


    }
}
