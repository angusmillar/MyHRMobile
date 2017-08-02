using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHRMobile.FhirGatewayTool.ViewModel
{
  public class AddUserAccountViewModel : ViewModelBase
  {
    public AddUserAccountViewModel()
    {
      _MyGovLoginButtonText = "Save & MyGov Login";
      _MyGovLoginMessage = $"Create a friendly account name for this user. \nHit the '{_MyGovLoginButtonText}' button and you will be sent to the MyGov authentication page. \nIf you authentication is successful your new account will be save for use."; ;
    }

    private string _MyGovLoginMessage { get; set; }
    public string MyGovLoginMessage
    {
      get
      {
        return _MyGovLoginMessage;
      }
      set
      {
        _MyGovLoginMessage = value;
        NotifyPropertyChanged("MyGovLoginMessage");
      }
    }

    private string _MyGovLoginButtonText { get; set; }
    public string MyGovLoginButtonText
    {
      get
      {
        return _MyGovLoginButtonText;
      }
      set
      {
        _MyGovLoginButtonText = value;
        NotifyPropertyChanged("MyGovLoginButtonText");
      }
    }

    private string _UserAccountNameText { get; set; }
    public string UserAccountNameText
    {
      get
      {
        return _UserAccountNameText;
      }
      set
      {
        _UserAccountNameText = value;
        NotifyPropertyChanged("UserAccountNameText");
      }
    }

  }
}
