using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHRMobile.Common.Extentions;
using System.Collections.ObjectModel;

namespace MyHRMobile.FhirGatewayTool.ViewModel
{
  public class MainViewModel : INotifyPropertyChanged
  {
    public MainViewModel()
    {
      this._UserAccountViewList = new ObservableCollection<UserAccountView>();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public string _Client_id { get; set; }
    public string Client_id
    {
      get
      {
        return _Client_id;
      }
      set
      {
        _Client_id = value;
        NotifyPropertyChanged("Client_id");
      }
    }

    public string _Client_secret { get; set; }
    public string Client_secret
    {
      get
      {
        return _Client_secret;
      }
      set
      {
        _Client_secret = value;
        NotifyPropertyChanged("Client_secret");
      }
    }

    private ObservableCollection<UserAccountView> _UserAccountViewList { get; set; }
    public ObservableCollection<UserAccountView> UserAccountViewList
    {
      get { return _UserAccountViewList; }
      set
      {
        if (value != _UserAccountViewList)
        {
          _UserAccountViewList = value;
          NotifyPropertyChanged("UserAccountViewList");
        }
      }
    }

    private void NotifyPropertyChanged(String info)
    {
      PropertyChanged.Raise(this, info);
    }
  }

  public class UserAccountView
  {
    public string Username { get; set; }
    public string AuthorisationCode { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime AccessExpires { get; set; }
    public DateTime RefreshExpires { get; set; }
    public string Scope { get; set; }

    public override string ToString()
    {
      return Username;
    }
  }
}
