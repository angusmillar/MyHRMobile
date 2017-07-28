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
  public class Presenter : ViewModelBase, INotifyPropertyChanged
  {
    public Presenter()
    {
      this._UserAccountViewList = new ObservableCollection<UserAccountView>();
    }

    private UserAccountView _CurrentUserAccount { get; set; }
    public UserAccountView CurrentUserAccount
    {
      get
      {
        return _CurrentUserAccount;
      }
      set
      {
        _CurrentUserAccount = value;
        NotifyPropertyChanged("CurrentUserAccount");
      }
    }

    private string _Client_id { get; set; }
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

    private string _Client_secret { get; set; }
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

    private string _MyGovError { get; set; }
    public string MyGovError
    {
      get
      {
        return _MyGovError;
      }
      set
      {
        _MyGovError = value;
        NotifyPropertyChanged("MyGovError");
      }
    }

    private string _MyGovErrorDescription { get; set; }
    public string MyGovErrorDescription
    {
      get
      {
        return _MyGovErrorDescription;
      }
      set
      {
        _MyGovErrorDescription = value;
        NotifyPropertyChanged("MyGovErrorDescription");
      }
    }

    private string _TextEditorRight { get; set; }
    public string TextEditorRight
    {
      get
      {
        return _TextEditorRight;
      }
      set
      {
        _TextEditorRight = value;
        NotifyPropertyChanged("TextEditorRight");
      }
    }


    private ObservableCollection<UserAccountView> _UserAccountViewList { get; set; }
    public ObservableCollection<UserAccountView> AddUserAccountViewList
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
    public ReadOnlyObservableCollection<UserAccountView> UserAccountViewList
    {
      get
      {
        return new ReadOnlyObservableCollection<UserAccountView>(_UserAccountViewList);
      }

    }

  }

  public class UserAccountView
  {
    public string Username { get; set; }
    public string AuthorisationCode { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime AccessExpires { get; set; }
    public string AccessExpiresString
    {
      get
      {
        return $"{AccessExpires.ToShortDateString()} {AccessExpires.ToShortTimeString()}";
      }
    }
    public DateTime RefreshExpires { get; set; }
    public string RefreshExpiresString
    {
      get
      {
        return $"{RefreshExpires.ToShortDateString()} {RefreshExpires.ToShortTimeString()}";
      }
    }

    public string Scope { get; set; }

    public override string ToString()
    {
      return Username;
    }
  }
}
