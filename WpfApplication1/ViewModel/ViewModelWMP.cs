using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

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
    public ViewModelWMP()
    {

    }

    private string RootRepo = ConfigurationManager.AppSettings.Get("RootRepo");
    private string PublicRepo = ConfigurationManager.AppSettings.Get("PublicRepo");

    private void _fill_list(string dir, IList<Media> tmp, string filePM)
    {
        int index = dir.LastIndexOf('\\');
        string filename = dir.Substring(index + 1);
        switch (filePM)
        {
            case @"\Videos":
                tmp.Add(new Media(filename, new Uri(new Uri(dir).LocalPath), new Uri(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Images\film.jpg")));
                break;
            case @"\Music":
                tmp.Add(new Media(filename, new Uri(new Uri(dir).LocalPath), new Uri(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Images\music.jpg")));
                break;
            default:
                tmp.Add(new Media(filename, new Uri(new Uri(dir).LocalPath), new Uri(new Uri(dir).LocalPath)));
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
    public void MyDouble()
    {

    }

    private ICommand listBox_MouseDoubleClick;
    public ICommand ListBox_MouseDoubleClick
    {
        get
        {
            return listBox_MouseDoubleClick ?? (listBox_MouseDoubleClick = new CommandHandler(() => MyDouble(), true));
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
        /*SetPlaylistNameVisibility = true;
        Create_buttonVisibility = false;
        Import_buttonVisibility = false;*/
    }

    private ICommand create_Playlist;
    public ICommand Create_Playlist
    {
        get
        {
            return create_Playlist ?? (create_Playlist = new CommandHandler(() => MyAction(), true));
        }
    }
    public class CommandHandler : ICommand
    {
        private Action _action;
        private bool _canExecute;
        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action();
        }
    }
    private IList<Playlist> getPlaylist()
    {
        IList<Playlist> list = new List<Playlist>();

        string folder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        string[] dirs = Directory.GetFiles(folder, "*.xml", SearchOption.AllDirectories);
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
    