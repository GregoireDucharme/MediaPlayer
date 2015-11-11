using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Xml.Serialization;

[Serializable]
public class Playlist
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
    private List<Media> _listMedia = new List<Media>();
    
    public List<Media> ListMedia
    {
        get
        {
            return (_listMedia);
        }
        set
        {
            _listMedia = value;
        }
    }

    public void Add(Media media)
    {
        if (media != null)
            _listMedia.Add(media);
    }
    public void InsertMediaAction(object parameter)
    {
        Add((Media)parameter);
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



