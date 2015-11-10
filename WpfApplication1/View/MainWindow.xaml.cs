using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
        ViewModelWMP VM = new ViewModelWMP();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = VM;
        }

        private void Action_File(object sender, RoutedEventArgs e)
        {
            mediaElement.LoadedBehavior = MediaState.Manual;
            Button tmp = (Button)sender;
            switch ((String)tmp.Content)
            {
                case "Play":
                    mediaElement.Play();
                    break;
                case "Pause":
                    mediaElement.Pause();
                    break;
                case "Stop":
                    mediaElement.Stop();
                    break;
                case "Prev":
                    mediaElement.Stop();
                    break;
                case "Next":
                    mediaElement.Stop();
                    break;
            }
        }
        private void Start_Timeline(object sender, DragStartedEventArgs args)
        {
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.Pause();
        }

        private void End_Timeline(object sender, DragCompletedEventArgs args)
        {
            int t = (int)timeline.Value;

            mediaElement.Position = TimeSpan.FromMilliseconds(t);
            mediaElement.Play();
        }
        private void Mouse_Volume(object sender, MouseWheelEventArgs e)
        {
            /* ViewModel */
            mediaElement.Volume += (e.Delta > 0) ? 0.1 : -0.1;
            mediaElement.Volume = (mediaElement.Volume > 1) ? 1 : mediaElement.Volume;
            mediaElement.Volume = (mediaElement.Volume < 0) ? 0 : mediaElement.Volume;
            volume.Value = mediaElement.Volume;
        }
        private void Get_Len(object sender, RoutedEventArgs e)
        {
            if (mediaElement.NaturalDuration.HasTimeSpan)
            {
                timeline.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
                timeline.Value = mediaElement.Position.TotalMilliseconds;
                //if (VM.MainMedia.Timer != 0)
                  //  mediaElement.Position = TimeSpan.FromMilliseconds(VM.MainMedia.Timer);
            }
        }
    }

}
