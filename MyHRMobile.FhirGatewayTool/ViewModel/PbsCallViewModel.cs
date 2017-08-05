using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHRMobile.FhirGatewayTool.ViewModel
{
  public class PbsCallViewModel : ViewModelBase
  {
    public PbsCallViewModel()
    {
      _ToDate = DateTime.Now;
      _FromDate = DateTime.Now.AddMonths(-2);
    }

    private DateTime _FromDate;
    public DateTime FromDate
    {
      get { return _FromDate; }
      set
      {
        _FromDate = value;
        NotifyPropertyChanged("FromDate");
      }
    }

    private DateTime _ToDate;
    public DateTime ToDate
    {
      get { return _ToDate; }
      set
      {
        _ToDate = value;
        NotifyPropertyChanged("ToDate");
      }
    }

  }
}
