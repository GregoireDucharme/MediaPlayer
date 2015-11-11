using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

class Media
{

    public Media(string info, Uri uri, Uri source, ObservableCollection<Playlist> ListBoxPlaylist)
    {
        Info = info;
        ListSource = source;
        Uri = uri;
        foreach (Playlist playlist in ListBoxPlaylist)
        {
            _lbp.Add(new Tuple<Playlist, Uri>(playlist, Uri));
        }
    }

    private ObservableCollection<Tuple<Playlist, Uri>> _lbp = new ObservableCollection<Tuple<Playlist, Uri>>();

    public ObservableCollection<Tuple<Playlist, Uri>> LBP
    {
        get
        {   
            return (_lbp);
        }
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