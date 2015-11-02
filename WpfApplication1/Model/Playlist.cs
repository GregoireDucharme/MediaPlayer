using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Playlist
{
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
}



