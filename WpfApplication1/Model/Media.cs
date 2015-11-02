using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
class Media
{
    public Media(string info, Uri uri, Uri source)
    {
        Info = info;
        ListSource = source;
        Uri = uri;
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

