using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;

class ViewModelWMP : BaseModel
{
    private ModelWMP model = new ModelWMP();
    private FirsTab mainMedia = new FirsTab();
    private Playlist _selectedPlaylist = new Playlist();
    public ModelWMP Model
    {
        get
        {
            return model;
        }
    }
    public FirsTab MainMedia
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
    }

    private void _fill_list(string dir, IList<Media> tmp, string filePM)
    {
        int index = dir.LastIndexOf('\\');
        string filename = dir.Substring(index + 1);
        switch (filePM)
        {
            case @"\Videos":
                tmp.Add(new Media(filename, new Uri(new Uri(dir).LocalPath), new Uri(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Images\film.jpg"), ListBoxPlaylist));
                break;
            case @"\Music":
                tmp.Add(new Media(filename, new Uri(new Uri(dir).LocalPath), new Uri(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Images\music.jpg"), ListBoxPlaylist));
                break;
            default:
                tmp.Add(new Media(filename, new Uri(new Uri(dir).LocalPath), new Uri(new Uri(dir).LocalPath), ListBoxPlaylist));
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
                if (MainMedia.Index < _selectedPlaylist.ListMedia.Count && MainMedia.Index > 0)
                {
                    MainMedia.Index -= 1;
                    mainMedia.Source = new Uri(new Uri(_selectedPlaylist.ListMedia[MainMedia.Index].Info).AbsolutePath);
                }
                MainMedia.PlayState = false;
                break;
            case "Next":
                MainMedia.StopState = false;
                if (MainMedia.Index +1 < _selectedPlaylist.ListMedia.Count)
                {
                    MainMedia.Index += 1;
                    mainMedia.Source = new Uri(new Uri(_selectedPlaylist.ListMedia[MainMedia.Index].Info).AbsolutePath);
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
       // var media = new MediaXML("Croquette");
        //playlist.Add(new Media("Croquette"));
        //playlist.Add(new Media("Cachou"));
        //playlist.Add(media);
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
                if (_selectedPlaylist.ListMedia.Count > 0)
                    mainMedia.Source = new Uri(new Uri(_selectedPlaylist.ListMedia[0].Info).AbsolutePath);
                model.CurrentTab = 0;
                mainMedia.NextState = true;
                mainMedia.PlayState = false;
                mainMedia.Len = 10;
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
/*        if (parameter.Item1 && parameter.Item2)
        {
            String nameList = (String)parameter.Item1;
            String nameNewMedia = (String)parameter.Item2;
            XmlSerializer deserializerPlaylist = new XmlSerializer(typeof(PlaylistXML));
            Environment.CurrentDirectory = @"C:\";
            try
            {
                using (Stream reader = new FileStream(RootRepo + Environment.UserName + ProjectRepo + "\\" + nameList, FileMode.Open))
                {
                    _selectedPlaylist = (PlaylistXML)deserializerPlaylist.Deserialize(reader);
                    reader.Close();
                    _selectedPlaylist.Add(newMedia);
                    // suprimer le fichier xml ouvert
                    // Serializer la nouvelle liste
                }
            }
            catch (Exception e)
            {

            }
        }*/
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
        listBoxPlaylist.Add(new Playlist(rec));
        OnPropertyChanged("ListBoxPlaylist");
    }

    private ObservableCollection<Playlist> listBoxPlaylist = null;

    public ObservableCollection<Playlist> ListBoxPlaylist
    {
        get
        {
            if (listBoxPlaylist == null)
                listBoxPlaylist = getPlaylist();
            return listBoxPlaylist;
        }
    }
    public void MyAction(object parameter)
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
            return listBox_MouseDoubleClick ?? (listBox_MouseDoubleClick = new CommandHandler(MyAction, true));
        }
    }
    public void InsertMediaAction(object parameter)
    {

    }

    private ICommand insertMedia;
    public ICommand InsertMedia
    {
        get
        {
            return insertMedia ?? (insertMedia = new CommandHandler(InsertMediaAction, true));
        }
    }
}
