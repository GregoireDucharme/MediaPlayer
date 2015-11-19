using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Serialization;

[Serializable]
public class Playlist : BaseModel
{
    public Playlist()
    {
        Index = null;
    }
    public Playlist(string index)
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
    private ObservableCollection<Media> _listMedia = new ObservableCollection<Media>();
    public ObservableCollection<Media> ListMedia
    {
        get
        {
            return (_listMedia);
        }
        set
        {
            _listMedia = value;
            listMediaFull = (value.Count > listMediaFull.Count ? value : listMediaFull);
            OnPropertyChanged("ListMedia");
        }
    }
    private ObservableCollection<Media> listMediaFull;
    public ObservableCollection<Media> ListMediaFull
    {
        get
        {
            return (listMediaFull);
        }
        set
        {
            listMediaFull = value;
        }
    }

    public void Add(Media media)
    {
        if (media != null)
            _listMedia.Add(media);
    }
}



