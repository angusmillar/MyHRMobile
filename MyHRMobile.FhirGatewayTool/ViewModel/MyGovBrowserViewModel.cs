using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHRMobile.FhirGatewayTool.ViewModel
{
  public class MyGovBrowserViewModel : ViewModelBase
  {
    private Uri _NavigatedUri { get; set; }
    public Uri NavigatedUri
    {
      get
      {
        return _NavigatedUri;
      }
      set
      {
        _NavigatedUri = value;
        NotifyPropertyChanged("NavigatedUri");
      }
    }

  }
}
