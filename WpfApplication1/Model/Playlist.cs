﻿using System.Windows.Input;

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


