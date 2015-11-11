using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

class ViewModelXML : BaseViewModel
{
    private Playlist _selectedPlaylist = new Playlist();
    public Playlist SelectedPlaylist
    {
        get
        {
            return _selectedPlaylist;
        }
    }

    public ViewModelXML(FirsTab MM, ModelWMP M)
    {
        mainMedia = MM;
        model = M;
    }
    public void selectPlayList(object parameter)
    {
        String name = (String)parameter;
        XmlSerializer deserializerPlaylist = new XmlSerializer(typeof(Playlist));
        Environment.CurrentDirectory = @"C:\";
        try
        {
            using (Stream reader = new FileStream(RootRepo + Environment.UserName + ProjectRepo + "\\" + name, FileMode.Open))
            {
                _selectedPlaylist = (Playlist)deserializerPlaylist.Deserialize(reader);
                if (_selectedPlaylist.ListMedia.Count > 0)
                    mainMedia.Source = new Uri(_selectedPlaylist.ListMedia[0].UriXML);
                model.CurrentTab = 0;
                mainMedia.NextState = true;
                mainMedia.PlayState = false;
                mainMedia.Len = 10;
                reader.Close();
            }
        }
        catch (InvalidOperationException e)
        {

        }
        catch (Exception e)
        {

        }
    }

    private ICommand pickPlayList;

    public ICommand PickPlayList
    {
        get
        {
            return pickPlayList ?? (pickPlayList = new CommandHandler(selectPlayList, true));
        }
    }

    public void updatePlayListAction(object parameter)
    {
        Tuple<Playlist, Media> parameters = (Tuple<Playlist, Media>)parameter;

        String nameList = (String)parameters.Item1.Index;
        XmlSerializer deserializerPlaylist = new XmlSerializer(typeof(Playlist));
        Environment.CurrentDirectory = @"C:\";
        try
        {
            using (Stream reader = new FileStream(RootRepo + Environment.UserName + ProjectRepo + "\\" + nameList, FileMode.Open))
            {
                _selectedPlaylist = (Playlist)deserializerPlaylist.Deserialize(reader);
                reader.Close();
                _selectedPlaylist.Add(parameters.Item2);
                if (File.Exists(RootRepo + Environment.UserName + ProjectRepo + "\\" + nameList))
                {
                    File.Delete(RootRepo + Environment.UserName + ProjectRepo + "\\" + nameList);
                    XmlSerializer serializePlaylist = new XmlSerializer(typeof(Playlist));
                    TextWriter WriteFileStream = new StreamWriter(RootRepo + Environment.UserName + ProjectRepo + "\\" + nameList);
                    serializePlaylist.Serialize(WriteFileStream, _selectedPlaylist);
                    WriteFileStream.Close();
                }
            }
        }
        catch (Exception e)
        {

        }
    }

    private ICommand updatePlayList;

    public ICommand UpdatePlayList
    {
        get
        {
            return updatePlayList ?? (updatePlayList = new CommandHandler(updatePlayListAction, true));
        }
    }
}