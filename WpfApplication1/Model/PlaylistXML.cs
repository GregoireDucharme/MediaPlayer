using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
public class PlaylistXML
{
    private String _name;

    private List<MediaXML> _listMedia;

    public List<MediaXML> ListMedia
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

    public PlaylistXML()
    {
        _name = null;
    }

    public PlaylistXML(String name)
    {
        _name = name;
    }

    public void Add(MediaXML media)
    {
        if (media!= null)
            _listMedia.Add(media);
    }
}
