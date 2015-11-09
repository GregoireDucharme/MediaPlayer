using System;
using System.Windows.Input;

class Media
{
    FirsTab mainMedia;
    ModelWMP model;
    public Media(string info, Uri uri, Uri source, FirsTab MM, ModelWMP M)
    {
        Info = info;
        ListSource = source;
        Uri = uri;
        mainMedia = MM;
        model = M;
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
    public void MyAction()
    {
        mainMedia.Source = Uri;
        model.CurrentTab = 0;
        mainMedia.NextState = true;
        mainMedia.PlayState = false;
    }
    private ICommand listBox_MouseDoubleClick;
    public ICommand ListBox_MouseDoubleClick
    {
        get
        {
            return listBox_MouseDoubleClick ?? (listBox_MouseDoubleClick = new CommandHandler(MyAction, true));
        }
    }
}