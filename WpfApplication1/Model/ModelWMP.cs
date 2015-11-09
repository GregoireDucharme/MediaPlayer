public class ModelWMP : BaseModel
{
    private int currentTab = 1;
    public int CurrentTab
    {
        get
        {
            return currentTab;
        }
        set
        {
            currentTab = value;
            OnPropertyChanged("CurrentTab");
        }
    }
 
    public ModelWMP()
    {

    }
   
    private bool setPlaylistNameVisibility = false;
    public bool SetPlaylistNameVisibility
    {
        get
        {
            return setPlaylistNameVisibility;
        }
        set
        {
            setPlaylistNameVisibility = value;
            OnPropertyChanged("SetPlaylistNameVisibility");
        }
    }
    private bool create_buttonVisibility = true;
    public bool Create_buttonVisibility
    {
        get
        {
            return create_buttonVisibility;
        }
        set
        {
            create_buttonVisibility = value;
            OnPropertyChanged("Create_buttonVisibility");
        }
    }
    private bool import_buttonVisibility = true;
    public bool Import_buttonVisibility
    {
        get
        {
            return import_buttonVisibility;
        }
        set
        {
            import_buttonVisibility = value;
            OnPropertyChanged("Import_buttonVisibility");
        }
    }
}



