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
        _lbp = ListBoxPlaylist;
    }

    private ObservableCollection<Playlist> _lbp;

    public ObservableCollection<Playlist> LBP
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