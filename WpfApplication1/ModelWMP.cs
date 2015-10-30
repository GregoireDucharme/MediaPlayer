using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Threading;

public class ModelWMP : INotifyPropertyChanged
{
    private String timeTxt = "00:00:00";
    public String TimeTxt
    {
        get
        {
            return timeTxt;
        }
        set
        {
            timeTxt = TimeSpan.FromMilliseconds(timer).ToString(@"hh\:mm\:ss");
            OnPropertyChanged("TimeTxt");
        }
    }
    private int currentTab = 1;
    public int CurrentTab
    {
        get
        {
            return currentTab;
        }
        set
        {
            currentTab = value;
            OnPropertyChanged("CurrentTab");
        }
    }
    DispatcherTimer _timer = new DispatcherTimer();
    private int timer = 0;
    private double _len = 0;
    public double _Len
    {
        get
        {
            return _len;
        }
        set
        {
            if (chck == true)
            {
                _len = value;
                Max = value;
                Timer = 0;
                chck = false;
            }
        }
    }
    private Uri source;
    private bool optionVisi = false;
    public bool OptionVisi
    {
        get
        {
            return optionVisi;
        }
        set
        {
            optionVisi = value;
            OnPropertyChanged("OptionVisi");
        }
    }

    private bool chck;
    public Uri Source
    {
        get
        {
            return source;
        }
        set
        {
            if (value != source)
            {
                chck = true;
                source = value;
                OnPropertyChanged("Source");
                CurrentTab = 0;
                OptionVisi = true;
            }
            else
                chck = false;
        }
    }
    public int Timer
    {
        get
        {
            return timer;
        }
        set
        {
            if (value == 0)
                timer = 0;
            if (timer == 0)
                _timer.Start();
            timer = value;
            if (timer >= _len)
            {
                timer = 0;
                _timer.Stop();
            }
            OnPropertyChanged("Timer");
        }
    }
    private void _actualisation(object sender, EventArgs e)
    {
        Timer += 1000;
    }

    private bool playState = false;
    private bool pauseState = false;
    private bool stopState = false;
    private bool nextState = false;
    public ModelWMP()
    {
        _timer.Interval = TimeSpan.FromMilliseconds(1000);
        _timer.Tick += new EventHandler(_actualisation);
    }
    private void _trigger()
    {
        OnPropertyChanged("PlayState");
        OnPropertyChanged("PauseState");
        OnPropertyChanged("StopState");
        OnPropertyChanged("NextState");
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
            nextState = true;
            /*Uri tmp = Source;
            Source = null;
            Source = tmp;*/
            _timer.Start();
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
            nextState = true;
            _timer.Stop();
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
            nextState = false;
            _timer.Stop();
            timer = 0;
            OnPropertyChanged("Timer");
            _trigger();
        }
    }
    public bool NextState
    {
        get
        {
            return nextState;
        }
        set
        {
            playState = value;
            pauseState = value;
            stopState = value;
            nextState = value;
            _timer.Stop();
            timer = 0;
            OnPropertyChanged("Timer");
            _trigger();
        }
    }
    private Double volume = 0.5;
    public Double Volume
    {
        get
        {
            return volume;
        }
        set
        {
            volume = value;
            OnPropertyChanged("Volume");
        }
    }
    private Double max = 0.5;
    public Double Max
    {
        get
        {
            return max;
        }
        set
        {
            max = value;
            OnPropertyChanged("Max");
        }
    }
    private ICommand create_Playlist;
    public ICommand Create_Playlist
    {
        get
        {
            return create_Playlist ?? (create_Playlist = new CommandHandler(() => MyAction(), true));
        }
    }
    private bool setPlaylistNameVisibility = false;
    public bool SetPlaylistNameVisibility
    {
        get
        {
            return setPlaylistNameVisibility;
        }
        set
        {
            setPlaylistNameVisibility = value;
            OnPropertyChanged("SetPlaylistNameVisibility");
        }
    }
    private bool create_buttonVisibility = true;
    public bool Create_buttonVisibility
    {
        get
        {
            return create_buttonVisibility;
        }
        set
        {
            create_buttonVisibility = value;
            OnPropertyChanged("Create_buttonVisibility");
        }
    }
    private bool import_buttonVisibility = true;
    public bool Import_buttonVisibility
    {
        get
        {
            return import_buttonVisibility;
        }
        set
        {
            import_buttonVisibility = value;
            OnPropertyChanged("Import_buttonVisibility");
        }
    }
    public void MyAction()
    {
        SetPlaylistNameVisibility = true;
        Create_buttonVisibility = false;
        Import_buttonVisibility = false;
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

    // BINDING DE POSITION ?
    /*private TimeSpan position;
    public TimeSpan Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value;
            OnPropertyChanged("Position");
        }
    }*/
    public class playlist
    {
        public playlist(string index)
        {
            Index = index;
        }
        private string _index;
        public string Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
            }
        }
    }
    private IList<playlist> getPlaylist()
    {
        IList<playlist> list = new List<playlist>();

        string folder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        string[] dirs = Directory.GetFiles(folder, "*.xml", SearchOption.AllDirectories);
        int index = 0;
        foreach (string dir in dirs)
        {
            index = dir.LastIndexOf('\\');
            list.Add(new playlist(dir.Substring(index + 1)));
        }
        return list;
    }
    public IList<playlist> ListBoxPlaylist
    {
        get
        {
            return getPlaylist();
        }
    }

    private string RootRepo = ConfigurationManager.AppSettings.Get("RootRepo");
    private string PublicRepo = ConfigurationManager.AppSettings.Get("PublicRepo");

    private void _fill_list(string dir, IList<media> tmp, string filePM)
    {
        int index = dir.LastIndexOf('\\');
        string filename = dir.Substring(index + 1);
        switch (filePM)
        {
            case @"\Videos":
                tmp.Add(new media(filename, new Uri(new Uri(dir).LocalPath), new Uri(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Images\film.jpg")));
                break;
            case @"\Music":
                tmp.Add(new media(filename, new Uri(new Uri(dir).LocalPath), new Uri(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Images\music.jpg")));
                break;
            default:
                tmp.Add(new media(filename, new Uri(new Uri(dir).LocalPath), new Uri(new Uri(dir).LocalPath)));
                break;
        }
    }

    private IList<media> _get_files(string filePM, string type0, string type1, string type2)
    {
        IList<media> tmp = new List<media>();
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

    public class media
    {
        public media(string info, Uri uri, Uri source)
        {
            Info = info;
            ListSource = source;
            Uri = uri;
        }
        private string info;
        private Uri _source;
        public string Info
        {
            get
            {
                return info;
            }
            set
            {
                info = value;
            }
        }
        public Uri ListSource
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
            }
        }
        private Uri uri;
        public Uri Uri
        {
            get
            {
                return uri;
            }
            set
            {
                uri = value;
            }
        }

    }

    public IList<media> ListBoxImage
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
    public IList<media> ListBoxVideo
    {
        get
        {
            try {
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
    public IList<media> ListBoxMusique
    {
        get
        {
            try {
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

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string v)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(v));
        }
    }
}



