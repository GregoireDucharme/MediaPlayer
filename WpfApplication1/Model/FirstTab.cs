using System;
using System.Windows.Threading;

public class FirsTab : BaseModel
{
    DispatcherTimer _timer = new DispatcherTimer();
    private int index = 0;
    public int Index
    {
        get
        {
            return index;
        }
        set
        {
            index = value;
            index = (index < 0 ?  0 : index);
            NextState = true;
            PlayState = false;
            Len = 10;
        }
    }
    public FirsTab()
    {
        _timer.Interval = TimeSpan.FromMilliseconds(1000);
        _timer.Tick += new EventHandler(_actualisation);
    }
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
    private int timer = 0;
    private double len = 0;
    public double Len
    {
        get
        {
            return len;
        }
        set
        {
            if (chck == true)
            {
                len = value;
                //Max = value;
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
            if (timer >= Max)
            {
                timer = 0;
                _timer.Stop();
            }
            OnPropertyChanged("Timer");
        }
    }

    private double speedratio = 1;
    public double SpeedRatio
    {
        get
        {
            return speedratio;
        }
        set
        {
            speedratio = value;
            OnPropertyChanged("SpeedRatio");
        }
    }
    private void _actualisation(object sender, EventArgs e)
    {
        Timer += (int)(1000 * speedratio);
        TimeTxt = "actualisation";
    }

    private bool playState = false;
    private bool pauseState = false;
    private bool stopState = false;
    private bool nextState = false;
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
  /*  private Double volume = 0.5;
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
    }*/
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
}
