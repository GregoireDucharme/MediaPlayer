using System.Collections.ObjectModel;

class ModelList : BaseModel
{
    private ObservableCollection<Playlist> listBoxPlaylist = null;

    public ObservableCollection<Playlist> ListBoxPlaylist
    {
        get
        {
            return listBoxPlaylist;
        }
        set
        {
            listBoxPlaylist = value;
            listBoxPlaylistFull = (value.Count > listBoxPlaylistFull.Count ? value : listBoxPlaylistFull);
            OnPropertyChanged("ListBoxPlaylist");
        }
    }

    private ObservableCollection<Playlist> listBoxPlaylistFull = new ObservableCollection<Playlist>();

    public ObservableCollection<Playlist> ListBoxPlaylistFull
    {
        get
        {
            return listBoxPlaylistFull;
        }
    }
    private ObservableCollection<Media> listBoxMusique;
    public ObservableCollection<Media> ListBoxMusique
    {
        get
        {
            return listBoxMusique;
        }
        set
        {
            listBoxMusique = value;
            listBoxMusiqueFull = (value.Count > listBoxMusiqueFull.Count ? value : listBoxMusiqueFull);
            OnPropertyChanged("ListBoxMusique");
        }
    }
    private ObservableCollection<Media> listBoxMusiqueFull = new ObservableCollection<Media>();

    public ObservableCollection<Media> ListBoxMusiqueFull
    {
        get
        {
            return listBoxMusiqueFull;
        }
    }

    private ObservableCollection<Media> listBoxImage;
    public ObservableCollection<Media> ListBoxImage
    {
        get
        {
            return listBoxImage;
        }
        set
        {
            listBoxImage = value;
            listBoxImageFull = (value.Count > listBoxImageFull.Count ? value : listBoxImageFull);
            OnPropertyChanged("ListBoxImage");
        }
    }
    private ObservableCollection<Media> listBoxImageFull = new ObservableCollection<Media>();

    public ObservableCollection<Media> ListBoxImageFull
    {
        get
        {
            return listBoxImageFull;
        }
    }
    private ObservableCollection<Media> listBoxVideo;
    public ObservableCollection<Media> ListBoxVideo
    {
        get
        {
            return listBoxVideo;
        }
        set
        {
            listBoxVideo = value;
            listBoxVideoFull = (value.Count > listBoxVideoFull.Count ? value : listBoxVideoFull);
            OnPropertyChanged("ListBoxVideo");
        }
    }
    private ObservableCollection<Media> listBoxVideoFull = new ObservableCollection<Media>();

    public ObservableCollection<Media> ListBoxVideoFull
    {
        get
        {
            return listBoxVideoFull;
        }
    }
}