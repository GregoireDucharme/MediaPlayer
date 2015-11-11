using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class BaseViewModel
{
    protected string RootRepo = ConfigurationManager.AppSettings.Get("RootRepo");
    protected string PublicRepo = ConfigurationManager.AppSettings.Get("PublicRepo");
    protected string ProjectRepo = ConfigurationManager.AppSettings.Get("ProjectRepo");
    protected FirsTab mainMedia = new FirsTab();
    public FirsTab MainMedia
    {
        get
        {
            return mainMedia;
        }
    }

    protected ModelWMP model = new ModelWMP();
    public ModelWMP Model
    {
        get
        {
            return model;
        }
    }
    protected ModelList modelList = new ModelList();
    public ModelList ModelList
    {
        get
        {
            return modelList;
        }
    }
}
