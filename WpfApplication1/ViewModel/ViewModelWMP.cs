using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;

class ViewModelWMP
{
    private ModelWMP model = new ModelWMP();
    private MainMedia mainMedia = new MainMedia();
    public ModelWMP Model
    {
        get
        {
            return model;
        }
    }
    public MainMedia MainMedia
    {
        get
        {
            return mainMedia;
        }
    }

    private string RootRepo = ConfigurationManager.AppSettings.Get("RootRepo");
    private string PublicRepo = ConfigurationManager.AppSettings.Get("PublicRepo");
    private string ProjectRepo = ConfigurationManager.AppSettings.Get("ProjectRepo");
    public ViewModelWMP()
    {
        if (!Directory.Exists(RootRepo + Environment.UserName + ProjectRepo));
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
    }

    private void _fill_list(string dir, IList<Media> tmp, string filePM)
    {
        int index = dir.LastIndexOf('\\');
        string filename = dir.Substring(index + 1);
        switch (filePM)
        {
            case @"\Videos":
                tmp.Add(new Media(filename, new Uri(new Uri(dir).LocalPath), new Uri(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Images\film.jpg"), MainMedia, Model));
                break;
            case @"\Music":
                tmp.Add(new Media(filename, new Uri(new Uri(dir).LocalPath), new Uri(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Images\music.jpg"), MainMedia, Model));
                break;
            default:
                tmp.Add(new Media(filename, new Uri(new Uri(dir).LocalPath), new Uri(new Uri(dir).LocalPath), MainMedia, Model));
                break;
        }
    }

    private IList<Media> _get_files(string filePM, string type0, string type1, string type2)
    {
        IList<Media> tmp = new List<Media>();
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
    public IList<Media> ListBoxImage
    {
        get
        {
            try
            {
                return _get_files(@"\Pictures", "jpg", "png", "gif");
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
            catch (PathTooLongException)
            {
                return null;
            }
            catch (DirectoryNotFoundException)
            {
                return null;
            }
        }
    }

    public IList<Media> ListBoxMusique
    {
        get
        {
            try
            {
                return _get_files(@"\Music", "mp3", "wav", "wma");
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
            catch (PathTooLongException)
            {
                return null;
            }
            catch (DirectoryNotFoundException)
            {
                return null;
            }
        }
    }

    public IList<Media> ListBoxVideo
    {
        get
        {
            try
            {
                return _get_files(@"\Videos", "mp4", "avi", "wmv");
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
            catch (PathTooLongException)
            {
                return null;
            }
            catch (DirectoryNotFoundException)
            {
                return null;
            }
        }
    }
  
    //private void ListBox_MouseDoubleClick(object sender, RoutedEventArgs e)
    //{
        // VIEWMODEL
        /*ListBoxItem item = (ListBoxItem)sender;
        Media current = (Media)item.Content;
        VM.Source = current.Uri;

        mediaElement.Play();
        VM.NextState = true;
        VM.PlayState = false;*/
    //}

    public void MyAction()
    {
        model.SetPlaylistNameVisibility = true;
        model.Create_buttonVisibility = false;
        model.Import_buttonVisibility = false;
        model.Cancel_buttonVisibility = true;
        model.Confirm_buttonVisibility = true;
      /*  XmlSerializer serializer = new XmlSerializer(typeof(Media));
        XmlDocument playlistDoc = new XmlDocument();*/
    }
    /*public void handle_volume(object pm)
    {
        //VIEWMODEL
        //MainMedia.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        MainMedia.Volume = (MainMedia.Volume > 1) ? 1 : MainMedia.Volume;
        MainMedia.Volume = (MainMedia.Volume < 0) ? 0 : MainMedia.Volume;
    }

    private ICommand mouse_Volume;
    public ICommand Mouse_Volume
    {
        get
        {
            return mouse_Volume ?? (mouse_Volume = new CommandHandler(handle_volume, true));
        }
    }*/
    private ICommand create_Playlist;
    public ICommand Create_Playlist
    {
        get
        {
            return create_Playlist ?? (create_Playlist = new CommandHandler(MyAction, true));
        }
    }

    private IList<Playlist> getPlaylist()
    {
        IList<Playlist> list = new List<Playlist>();

        string[] dirs = Directory.GetFiles(RootRepo + Environment.UserName + ProjectRepo, "*.xml", SearchOption.AllDirectories);
        int index = 0;
        foreach (string dir in dirs)
        {
            index = dir.LastIndexOf('\\');
            list.Add(new Playlist(dir.Substring(index + 1)));
        }
        return list;
    }
    public IList<Playlist> ListBoxPlaylist
    {
        get
        {
            return getPlaylist();
        }
    }
}
    