﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

class ViewModelWMP : BaseViewModel
{
    private ViewModelXML VMXML;
    public ViewModelXML XML
    {
        get
        {
            return VMXML;
        }
    }

    public ViewModelWMP()
    {
        if (!Directory.Exists(RootRepo + Environment.UserName + ProjectRepo))
        {
            try
            {
                Directory.CreateDirectory(RootRepo + Environment.UserName + ProjectRepo);
            }
            catch (Exception e)
            {
                MessageBoxResult result = MessageBox.Show("Erreur lors de la création du répertoire.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }
            }
        }
        VMXML = new ViewModelXML(mainMedia, model);
        ModelList.ListBoxPlaylist = getPlaylist();
        try
        {
            ModelList.ListBoxImage = _get_files(@"\Pictures", "jpg", "png", "gif");
            ModelList.ListBoxMusique = _get_files(@"\Music", "mp3", "wav", "wma");
            ModelList.ListBoxVideo = _get_files(@"\Videos", "mp4", "avi", "wmv");
        }
        catch (UnauthorizedAccessException)
        {
            MessageBoxResult result = MessageBox.Show("Erreur : Accès refusé.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        catch (PathTooLongException)
        {
            MessageBoxResult result = MessageBox.Show("Erreur : Chemin d'accès incorrecte.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        catch (DirectoryNotFoundException)
        {
            MessageBoxResult result = MessageBox.Show("Erreur : Fichier introuvable.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }

    private void _fill_list(string dir, IList<Media> tmp, string filePM)
    {
        int index = dir.LastIndexOf('\\');
        string filename = dir.Substring(index + 1);
        switch (filePM)
        {
            case @"\Videos":
                tmp.Add(new Media(filename, new Uri(new Uri(dir).LocalPath), new Uri(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Images\film.jpg"), ModelList.ListBoxPlaylist));
                break;
            case @"\Music":
                tmp.Add(new Media(filename, new Uri(new Uri(dir).LocalPath), new Uri(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Images\music.jpg"), ModelList.ListBoxPlaylist));
                break;
            default:
                tmp.Add(new Media(filename, new Uri(new Uri(dir).LocalPath), new Uri(new Uri(dir).LocalPath), ModelList.ListBoxPlaylist));
                break;
        }
    }

    private ObservableCollection<Media> _get_files(string filePM, string type0, string type1, string type2)
    {
        ObservableCollection<Media> tmp = new ObservableCollection<Media>();
        var files = Directory.EnumerateFiles(RootRepo + Environment.UserName + filePM, "*.*", SearchOption.AllDirectories)
        .Where(s => s.EndsWith("." + type0, StringComparison.OrdinalIgnoreCase) || s.EndsWith("." + type1, StringComparison.OrdinalIgnoreCase) ||
        s.EndsWith("." + type2, StringComparison.OrdinalIgnoreCase));
        foreach (string dir in files)
        {
            _fill_list(dir, tmp, filePM);
        }
        files = Directory.EnumerateFiles(RootRepo + PublicRepo + filePM, "*.*", SearchOption.AllDirectories)
        .Where(s => s.EndsWith("." + type0, StringComparison.OrdinalIgnoreCase) || s.EndsWith("." + type1, StringComparison.OrdinalIgnoreCase) ||
        s.EndsWith("." + type2, StringComparison.OrdinalIgnoreCase));
        foreach (string dir in files)
        {
            _fill_list(dir, tmp, filePM);
        }
        return tmp;
    }

    public void Switch(object parameter)
    {
        model.SetPlaylistNameVisibility = true;
        model.Create_buttonVisibility = false;
        model.Import_buttonVisibility = false;
        model.Cancel_buttonVisibility = true;
        model.Confirm_buttonVisibility = true;
    }

    private ICommand initialize_Creation;
    public ICommand Initialize_Creation
    {
        get
        {
            return initialize_Creation ?? (initialize_Creation = new CommandHandler(Switch, true));
        }
    }

    public void HandleButtonFirstTab(object parameter)
    {
        switch ((string)parameter)
        {
            case "Play":
                MainMedia.PlayState = false;
                break;
            case "Pause":
                MainMedia.PauseState = false;
                break;
            case "Stop":
                MainMedia.StopState = false;
                break;
            case "Prev":
                MainMedia.StopState = false;
                if (MainMedia.Index < VMXML.SelectedPlaylist.ListMedia.Count && MainMedia.Index > 0)
                {
                    MainMedia.Index -= 1;
                    mainMedia.Source = new Uri(VMXML.SelectedPlaylist.ListMedia[MainMedia.Index].UriXML);
                }
                MainMedia.PlayState = false;
                break;
            case "Next":
                MainMedia.StopState = false;
                if (MainMedia.Index +1 < VMXML.SelectedPlaylist.ListMedia.Count)
                {
                    MainMedia.Index += 1;
                    mainMedia.Source = new Uri(VMXML.SelectedPlaylist.ListMedia[MainMedia.Index].UriXML);
                }
                MainMedia.PlayState = false;
                break;
        }
    }

    private ICommand buttonFirstTab;
    public ICommand ButtonFirstTab
    {
        get
        {
            return buttonFirstTab ?? (buttonFirstTab = new CommandHandler(HandleButtonFirstTab, true));
        }
    }

    public void CancelPlaylist(object parameter)
    {
        model.SetPlaylistNameVisibility = false;
        model.Create_buttonVisibility = true;
        model.Import_buttonVisibility = true;
        model.Cancel_buttonVisibility = false;
        model.Confirm_buttonVisibility = false;
    }

    private ICommand cancel_Playlist;
    public ICommand Cancel_Playlist
    {
        get
        {
            return cancel_Playlist ?? (cancel_Playlist = new CommandHandler(CancelPlaylist, true));
        }
    }

    public void CheckName(object parameter)
    {
        String rec = (String)parameter + ".xml";
        Playlist playlist = new Playlist((String)parameter);
        Environment.CurrentDirectory = @"C:\";
        try
        {
            XmlSerializer xsl = new XmlSerializer(typeof(Playlist));
            TextWriter WriteFileStream = new StreamWriter(RootRepo + Environment.UserName + ProjectRepo + "\\" + rec);
            xsl.Serialize(WriteFileStream, playlist);
            WriteFileStream.Close();
            CancelPlaylist(null);
            ListBoxPlaylistAdd(rec);
        }
        catch (Exception e)
        {
            MessageBoxResult result = MessageBox.Show("Erreur lors de la création de la playlist", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }

    private ICommand setPlaylistName;

    public ICommand SetPlaylistName
    {
        get
        {
            return setPlaylistName ?? (setPlaylistName = new CommandHandler(CheckName, true));
        }
    }

    public void setThemeAction(object parameter)
    {
        String color = (String)parameter;

        model.BackgroundColor = color;
    }

    private ICommand setTheme;

    public ICommand SetTheme
    {
        get
        {
            return setTheme ?? (setTheme = new CommandHandler(setThemeAction, true));
        }
    }

    private ObservableCollection<Playlist> getPlaylist()
    {
        ObservableCollection<Playlist> list = new ObservableCollection<Playlist>();

        string[] dirs = Directory.GetFiles(RootRepo + Environment.UserName + ProjectRepo, "*.xml", SearchOption.AllDirectories);
        int index = 0;
        foreach (string dir in dirs)
        {
            index = dir.LastIndexOf('\\');
            list.Add(new Playlist(dir.Substring(index + 1)));
        }
        return list;
    }

    public void ListBoxPlaylistAdd(String rec)
    {
        ModelList.ListBoxPlaylist.Add(new Playlist(rec));
        foreach (Media tmpMed in modelList.ListBoxImage)
        {
            ObservableCollection<Tuple<Playlist, Media>> tuppletmp = new ObservableCollection<Tuple<Playlist, Media>>();
            foreach (Playlist tmp in ModelList.ListBoxPlaylist)
            {
                tuppletmp.Add(new Tuple<Playlist, Media>(tmp, tmpMed));
            }
            tmpMed.LBP = tuppletmp;
        }
        foreach (Media tmpMed in modelList.ListBoxMusique)
        {
            ObservableCollection<Tuple<Playlist, Media>> tuppletmp = new ObservableCollection<Tuple<Playlist, Media>>();
            foreach (Playlist tmp in ModelList.ListBoxPlaylist)
            {
                tuppletmp.Add(new Tuple<Playlist, Media>(tmp, tmpMed));
            }
            tmpMed.LBP = tuppletmp;
        }
        foreach (Media tmpMed in modelList.ListBoxVideo)
        {
            ObservableCollection<Tuple<Playlist, Media>> tuppletmp = new ObservableCollection<Tuple<Playlist, Media>>();
            foreach (Playlist tmp in ModelList.ListBoxPlaylist)
            {
                tuppletmp.Add(new Tuple<Playlist, Media>(tmp, tmpMed));
            }
            tmpMed.LBP = tuppletmp;
        }
    }

    public void ListBox_MouseDoubleClickAction(object parameter)
    {
        mainMedia.Source = (Uri)parameter;
        model.CurrentTab = 0;
        mainMedia.NextState = true;
        mainMedia.PlayState = false;
        mainMedia.Len = 10;
    }

    private ICommand listBox_MouseDoubleClick;
    public ICommand ListBox_MouseDoubleClick
    {
        get
        {
            return listBox_MouseDoubleClick ?? (listBox_MouseDoubleClick = new CommandHandler(ListBox_MouseDoubleClickAction, true));
        }
    }
}
