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
            OnPropertyChanged("ListBoxPlaylist");
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
        }
    }
}