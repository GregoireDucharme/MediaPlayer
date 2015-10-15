﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
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
                play.IsEnabled = false;
                pause.IsEnabled = true;
                stop.IsEnabled = true;
            }
        }
        private void Open_Playlist(object sender, RoutedEventArgs e)
        {

        }

        private void GetFiles(string path, string pattern)
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
                    listBox1.Items.Add(media);
                }
                foreach (var directory in Directory.GetDirectories(path))
                {
                    GetFiles(directory, pattern);
                }
            }
            catch (UnauthorizedAccessException) {
            }
            catch (PathTooLongException) {
            }
        }
        private void Create_Playlist(object sender, RoutedEventArgs e)
        {
            string userName = Environment.UserName;
            string path = @"c:\users\"+userName;
            string searchPattern = "*.png";
            GetFiles(path, searchPattern);
        }
        private void Action_File(object sender, RoutedEventArgs e)
        {
            player.LoadedBehavior = MediaState.Manual;
            switch (sender.ToString())
            {
                case "System.Windows.Controls.Button: Play":
                    player.Play();
                    play.IsEnabled = false;
                    pause.IsEnabled = true;
                    stop.IsEnabled = true;
                    break;
                case "System.Windows.Controls.Button: Pause":
                    player.Pause();
                    pause.IsEnabled = false;
                    play.IsEnabled = true;
                    break;
                case "System.Windows.Controls.Button: Stop":
                    pause.IsEnabled = false;
                    play.IsEnabled = true;
                    stop.IsEnabled = false;
                    player.Stop();
                    break;
            }
        }

    }
}
