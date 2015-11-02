using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Threading;

public class ModelWMP : INotifyPropertyChanged
{
    private String timeTxt = "00:00:00";
    public String TimeTxt
    {
        get
        {
            return timeTxt;
        }
        set
        {
            timeTxt = TimeSpan.FromMilliseconds(timer).ToString(@"hh\:mm\:ss");
            OnPropertyChanged("TimeTxt");
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
    DispatcherTimer _timer = new DispatcherTimer();
    private int timer = 0;
    private double _len = 0;
    public double _Len
    {
        get
        {
            return _len;
        }
        set
        {
            if (chck == true)
            {
                _len = value;
                Max = value;
                Timer = 0;
                chck = false;
            }
        }
    }
    private Uri source;
    private bool optionVisi = false;
    public bool OptionVisi
    {
        get
        {
            return optionVisi;
        }
        set
        {
            optionVisi = value;
            OnPropertyChanged("OptionVisi");
        }
    }

    private bool chck;
    public Uri Source
    {
        get
        {
            return source;
        }
        set
        {
            if (value != source)
            {
                chck = true;
                source = value;
                OnPropertyChanged("Source");
                CurrentTab = 0;
                OptionVisi = true;
            }
            else
                chck = false;
        }
    }
    public int Timer
    {
        get
        {
            return timer;
        }
        set
        {
            if (value == 0)
                timer = 0;
            if (timer == 0)
                _timer.Start();
            timer = value;
            if (timer >= _len)
            {
                timer = 0;
                _timer.Stop();
            }
            OnPropertyChanged("Timer");
        }
    }
    private void _actualisation(object sender, EventArgs e)
    {
        Timer += 1000;
    }

    private bool playState = false;
    private bool pauseState = false;
    private bool stopState = false;
    private bool nextState = false;
    public ModelWMP()
    {
        _timer.Interval = TimeSpan.FromMilliseconds(1000);
        _timer.Tick += new EventHandler(_actualisation);
    }
    private void _trigger()
    {
        OnPropertyChanged("PlayState");
        OnPropertyChanged("PauseState");
        OnPropertyChanged("StopState");
        OnPropertyChanged("NextState");
    }
    public bool PlayState
    {
        get
        {
            return playState;
        }
        set
        {
            playState = value;
            pauseState = true;
            stopState = true;
            nextState = true;
            /*Uri tmp = Source;
            Source = null;
            Source = tmp;*/
            _timer.Start();
            _trigger();
        }
    }
    public bool PauseState
    {
        get
        {
            return pauseState;
        }
        set
        {
            playState = true;
            pauseState = value;
            stopState = true;
            nextState = true;
            _timer.Stop();
            _trigger();
        }
    }
    public bool StopState
    {
        get
        {
            return stopState;
        }
        set
        {
            playState = true;
            pauseState = value;
            stopState = value;
            nextState = false;
            _timer.Stop();
            timer = 0;
            OnPropertyChanged("Timer");
            _trigger();
        }
    }
    public bool NextState
    {
        get
        {
            return nextState;
        }
        set
        {
            playState = value;
            pauseState = value;
            stopState = value;
            nextState = value;
            _timer.Stop();
            timer = 0;
            OnPropertyChanged("Timer");
            _trigger();
        }
    }
    private Double volume = 0.5;
    public Double Volume
    {
        get
        {
            return volume;
        }
        set
        {
            volume = value;
            OnPropertyChanged("Volume");
        }
    }
    private Double max = 0.5;
    public Double Max
    {
        get
        {
            return max;
        }
        set
        {
            max = value;
            OnPropertyChanged("Max");
        }
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

    // BINDING DE POSITION ?
    /*private TimeSpan position;
    public TimeSpan Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value;
            OnPropertyChanged("Position");
        }
    }*/

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string v)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(v));
        }
    }
}



