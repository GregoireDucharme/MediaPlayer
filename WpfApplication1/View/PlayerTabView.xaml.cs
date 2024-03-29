﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace mediaPlayer.View
{
    /// <summary>
    /// Logique d'interaction pour PlayerTabView.xaml
    /// </summary>
    public partial class PlayerTabView : UserControl
    {
        public PlayerTabView()
        {
            InitializeComponent();
        }

        private void Action_File(object sender, RoutedEventArgs e)
        {
            Button tmp = (Button)sender;
            try
            {
                switch (tmp.Name)
                {
                    case "Play":
                        mediaElement.SpeedRatio = speedratio.Value;
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
                        mediaElement.Play();
                        break;
                    case "Next":
                        mediaElement.Stop();
                        mediaElement.Play();
                        break;
                }
            }
            catch (Exception)
            {
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
            mediaElement.Volume += (e.Delta > 0) ? 0.1 : -0.1;
            mediaElement.Volume = (mediaElement.Volume > 1) ? 1 : mediaElement.Volume;
            mediaElement.Volume = (mediaElement.Volume < 0) ? 0 : mediaElement.Volume;
            volume.Value = mediaElement.Volume;
        }
        private void Speed_Ratio(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            mediaElement.SpeedRatio = speedratio.Value;
        }
        private void Get_Len(object sender, RoutedEventArgs e)
        {
            if (mediaElement.NaturalDuration.HasTimeSpan)
            {
                timeline.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
                timeline.Value = mediaElement.Position.TotalMilliseconds;
            }
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.Play();
        }
    }
}
