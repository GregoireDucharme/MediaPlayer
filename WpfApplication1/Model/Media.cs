using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

[Serializable]
public class Media
{

    public Media()
    {

    }
    public Media(string info, Uri uri, Uri source, ObservableCollection<Playlist> ListBoxPlaylist)
    {
        Info = info;
        ListSource = source;
        Uri = uri;
        foreach (Playlist playlist in ListBoxPlaylist)
        {
            _lbp.Add(new Tuple<Playlist, Media>(playlist, this));
        }
    }

    private ObservableCollection<Tuple<Playlist, Media>> _lbp = new ObservableCollection<Tuple<Playlist, Media>>();
    [XmlIgnore]
    public ObservableCollection<Tuple<Playlist, Media>> LBP
    {
        get
        {   
            return (_lbp);
        }
    }

    private string info;
    private Uri _source;
    [XmlIgnore]

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
    [XmlIgnore]
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
    [XmlIgnore]
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
    public String UriXML
    {
        get
        {
            return Uri.ToString();
        }
    }
}