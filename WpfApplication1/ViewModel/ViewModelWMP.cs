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
                throw e;
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
        }
        catch (PathTooLongException)
        {
        }
        catch (DirectoryNotFoundException)
        {
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
        XmlSerializer xsl = new XmlSerializer(typeof(Playlist));
        Environment.CurrentDirectory = @"C:\";
        TextWriter WriteFileStream = new StreamWriter(RootRepo + Environment.UserName + ProjectRepo + "\\" + rec);
        xsl.Serialize(WriteFileStream, playlist);
        WriteFileStream.Close();
        CancelPlaylist(null);
        ListBoxPlaylistAdd(rec);
    }

    private ICommand setPlaylistName;

    public ICommand SetPlaylistName
    {
        get
        {
            return setPlaylistName ?? (setPlaylistName = new CommandHandler(CheckName, true));
        }
    }

    public void selectPlayList(object parameter)
    {
        String name = (String)parameter;
        XmlSerializer deserializerPlaylist = new XmlSerializer(typeof(Playlist));
        Environment.CurrentDirectory = @"C:\";
        try
        {
            using (Stream reader = new FileStream(RootRepo + Environment.UserName + ProjectRepo + "\\" + name, FileMode.Open))
            {
                _selectedPlaylist = (Playlist)deserializerPlaylist.Deserialize(reader);
                reader.Close();
                //   selectedPlaylist.Add(newMedia);
                // suprimer le fichier xml ouvert
                // Serializer la nouvelle liste
            }
        }
        catch (InvalidOperationException e)
        {

        }
        catch (Exception e)
        {

        }
    }

    private ICommand pickPlayList;

    public ICommand PickPlayList
    {
        get
        {
            return pickPlayList ?? (pickPlayList = new CommandHandler(selectPlayList, true));
        }
    }

    public void updatePlayListAction(object parameter)
    {
        Tuple<Playlist, Media> parameters = (Tuple<Playlist, Media>)parameter;

        String nameList = (String)parameters.Item1.Index;
        // String nameNewMedia = (String)parameter.Item2;
        XmlSerializer deserializerPlaylist = new XmlSerializer(typeof(Playlist));
        Environment.CurrentDirectory = @"C:\";
        try
        {
            using (Stream reader = new FileStream(RootRepo + Environment.UserName + ProjectRepo + "\\" + nameList, FileMode.Open))
            {
                _selectedPlaylist = (Playlist)deserializerPlaylist.Deserialize(reader);
                reader.Close();
                _selectedPlaylist.Add(parameters.Item2);
                if (File.Exists(RootRepo + Environment.UserName + ProjectRepo + "\\" + nameList))
                {
                    File.Delete(RootRepo + Environment.UserName + ProjectRepo + "\\" + nameList);
                    XmlSerializer serializePlaylist = new XmlSerializer(typeof(Playlist));
                    TextWriter WriteFileStream = new StreamWriter(RootRepo + Environment.UserName + ProjectRepo + "\\" + nameList);
                    serializePlaylist.Serialize(WriteFileStream, _selectedPlaylist);
                    WriteFileStream.Close();
                }
            }
        }
        catch (Exception e)
        {

        }
    }

    private ICommand updatePlayList;

    public ICommand UpdatePlayList
    {
        get
        {
            return updatePlayList ?? (updatePlayList = new CommandHandler(updatePlayListAction, true));
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
