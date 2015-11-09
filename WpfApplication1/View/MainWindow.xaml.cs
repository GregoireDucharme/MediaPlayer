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
/*        private void ListBox_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            // VIEWMODEL
            ListBoxItem item = (ListBoxItem)sender;
            Media current = (Media)item.Content;
            VM.MainMedia.Source = current.Uri;
            VM.Model.CurrentTab = 0;
            mediaElement.Play();
            VM.MainMedia.NextState = true;
            VM.MainMedia.PlayState = false;
        }
*/
        private void Action_File(object sender, RoutedEventArgs e)
        {
            // VIEWMODEL
            mediaElement.LoadedBehavior = MediaState.Manual;
            switch (sender.ToString())
            {
                case "System.Windows.Controls.Button: Play":
                    VM.MainMedia.PlayState = false;
                    mediaElement.Play();
                    break;
                case "System.Windows.Controls.Button: Pause":
                    VM.MainMedia.PauseState = false;
                    mediaElement.Pause();
                    break;
                case "System.Windows.Controls.Button: Stop":
                    VM.MainMedia.StopState = false;
                    mediaElement.Stop();
                    break;
                case "System.Windows.Controls.Button: prec":
                    VM.MainMedia.StopState = false;
                    mediaElement.Stop();
                    break;
                case "System.Windows.Controls.Button: next":
                    VM.MainMedia.StopState = false;
                    mediaElement.Stop();
                    break;
            }
        }
        private void Start_Timeline(object sender, DragStartedEventArgs args)
        {
            mediaElement.LoadedBehavior = MediaState.Manual;
            VM.MainMedia.PauseState = false;
            mediaElement.Pause();
        }

        private void End_Timeline(object sender, DragCompletedEventArgs args)
        {
            int t = (int)timeline.Value;

            mediaElement.Position = TimeSpan.FromMilliseconds(t);
            VM.MainMedia.PlayState = false;
            mediaElement.Play();
        }
        private void Mouse_Volume(object sender, MouseWheelEventArgs e)
        {
            //VIEWMODEL
            VM.MainMedia.Volume += (e.Delta  >0) ? 0.1 : -0.1;
            VM.MainMedia.Volume = (VM.MainMedia.Volume > 1) ? 1 : VM.MainMedia.Volume;
            VM.MainMedia.Volume = (VM.MainMedia.Volume < 0) ? 0 : VM.MainMedia.Volume;
        }
        private void Get_Len(object sender, RoutedEventArgs e)
        {
            //VIEWMODEL
            MediaElement tmp = (MediaElement)sender;
            if (tmp.NaturalDuration.HasTimeSpan)
            {
                VM.MainMedia.Len = tmp.NaturalDuration.TimeSpan.TotalMilliseconds;
                if (VM.MainMedia.Timer != 0)
                    tmp.Position = TimeSpan.FromMilliseconds(VM.MainMedia.Timer);
            }
        }
    }

}
