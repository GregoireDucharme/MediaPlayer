﻿using System.Windows.Input;

public class ModelWMP : BaseModel
{
    private string backgroundColor;
    public string BackgroundColor
    {
        get
        {
            return backgroundColor;
        }
        set
        {
            backgroundColor = "#" + value;
            if (value == "FF151515")
            {
                ForegroundColor = "White";
            }
            else
            {
                ForegroundColor = "Black";
            }
            OnPropertyChanged("BackgroundColor");
        }
    }
    private string foregroundColor = "Black";
    public string ForegroundColor
    {
        get
        {
            return foregroundColor;
        }
        set
        {
            foregroundColor = value;
            OnPropertyChanged("ForegroundColor");
        }
    }
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

    private bool cancel_buttonVisibility = false;
    public bool Cancel_buttonVisibility
    {
        get
        {
            return cancel_buttonVisibility;
        }
        set
        {
            cancel_buttonVisibility = value;
            OnPropertyChanged("Cancel_buttonVisibility");
        }
    }


    private bool confirm_buttonVisibility = false;
    public bool Confirm_buttonVisibility
    {
        get
        {
            return confirm_buttonVisibility;
        }
        set
        {
            confirm_buttonVisibility = value;
            OnPropertyChanged("Confirm_buttonVisibility");
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
    private string wStyle = "SingleBorderWindow";

    public string WStyle
    {
        get
        {
            return wStyle;
        }
        set
        {
            wStyle = value;
            OnPropertyChanged("WStyle");
        }
    }
    private string wState = "Normal";

    public string WState
    {
        get
        {
            return wState;
        }
        set
        {
            wState = value;
            OnPropertyChanged("WState");
        }
    }

    public void ToggleScreenSizeAction(object parameter)
    {
        WStyle = (wStyle == "SingleBorderWindow" ? "None" : "SingleBorderWindow");
        WState = (wState == "Normal" ? "Maximized" : "Normal");
    }

    private ICommand toggleScreenSize;
    public ICommand ToggleScreenSize
    {
        get
        {
            return toggleScreenSize ?? (toggleScreenSize = new CommandHandler(ToggleScreenSizeAction, true));
        }
    }
}



