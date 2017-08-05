using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHRMobile.Common.Extentions;
using System.Collections.ObjectModel;
using MyHRMobile.FhirGatewayTool.Extensions;
using MyHRMobile.FhirGatewayTool.Views;
using MyHRMobile.FhirGatewayTool.CustomControl;
using MyHRMobile.Common.Utility;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Net;

namespace MyHRMobile.FhirGatewayTool.ViewModel
{
  public class Presenter : ViewModelBase, INotifyPropertyChanged
  {
    private ICommand _LoadRecordListCommand;
    public ICommand LoadRecordListCommand
    {
      get
      {
        return _LoadRecordListCommand;
      }
      set
      {
        _LoadRecordListCommand = value;
      }
    }

    private ICommand _SelectedRecordCommand;
    public ICommand SelectedRecordCommand
    {
      get
      {
        return _SelectedRecordCommand;
      }
      set
      {
        _SelectedRecordCommand = value;
      }
    }

    private ICommand _AddAccountCommand;
    public ICommand AddAccountCommand
    {
      get
      {
        return _AddAccountCommand;
      }
      set
      {
        _AddAccountCommand = value;
      }
    }

    private ICommand _DeleteAccountCommand;
    public ICommand DeleteAccountCommand
    {
      get
      {
        return _DeleteAccountCommand;
      }
      set
      {
        _DeleteAccountCommand = value;
      }
    }

    private ICommand _MyGovWebBrowserCommand;
    public ICommand MyGovWebBrowserCommand
    {
      get
      {
        return _MyGovWebBrowserCommand;
      }
      set
      {
        _MyGovWebBrowserCommand = value;
      }
    }

    private ICommand _MyGovWebBrowserLoadCompleteCommand;
    public ICommand MyGovWebBrowserLoadCompleteCommand
    {
      get
      {
        return _MyGovWebBrowserLoadCompleteCommand;
      }
      set
      {
        _MyGovWebBrowserLoadCompleteCommand = value;
      }
    }


    private bool _CanExecute = true;
    public bool CanExecute
    {
      get
      {
        return this._CanExecute;
      }

      set
      {
        if (this._CanExecute == value)
        {
          return;
        }
        this._CanExecute = value;
      }
    }

    private AddUserAccountViewModel _AddUserAccountViewModel;
    public AddUserAccountViewModel AddUserAccountViewModel
    {
      get
      {
        return _AddUserAccountViewModel;
      }
    }

    private MyGovBrowserViewModel _MyGovBrowserViewModel;
    public MyGovBrowserViewModel MyGovBrowserViewModel
    {
      get
      {
        return _MyGovBrowserViewModel;
      }
    }

    private MyGovAuthenticationOutcomeViewModel _MyGovAuthenticationOutcomeViewModel;
    public MyGovAuthenticationOutcomeViewModel MyGovAuthenticationOutcomeViewModel
    {
      get
      {
        return _MyGovAuthenticationOutcomeViewModel;
      }
      set
      {
        _MyGovAuthenticationOutcomeViewModel = value;
      }
    }

    private TextEditorViewModel _TextEditorViewModel;
    public TextEditorViewModel TextEditorViewModel
    {
      get
      {
        return _TextEditorViewModel;
      }
      set
      {
        _TextEditorViewModel = value;
      }
    }

    private PatientBanerViewModel _PatientBanerViewModel;
    public PatientBanerViewModel PatientBanerViewModel
    {
      get
      {
        return _PatientBanerViewModel;
      }
      set
      {
        _PatientBanerViewModel = value;
      }
    }

    private ApiCallViewModel _ApiCallViewModel;
    public ApiCallViewModel ApiCallViewModel
    {
      get
      {
        return _ApiCallViewModel;
      }
      set
      {
        _ApiCallViewModel = value;
      }
    }


    private List<UIElement> RightPanleStateList;
    private UiService _UiService;
    public UiService UiService
    {
      get
      {
        return _UiService;
      }
      set
      {
        _UiService = value;
        UiService.PrimeApplicationStore();
        UiService.LoadApplicationStore();
        UiService.UpdateView(this);
      }
    }
    public GridControl MainGrid;

    public Presenter()
    {
      _ApiCallViewModel = new ApiCallViewModel();

      LoadRecordListCommand = new RelayCommand(LoadRecordList, param => this._CanExecute);
      SelectedRecordCommand = new RelayCommand(LoadSelectedRecord, param => this._CanExecute);
      AddAccountCommand = new RelayCommand(PresentAddUserAccount, param => this._CanExecute);
      DeleteAccountCommand = new RelayCommand(DeleteUserAccount, param => this._CanExecute);
      MyGovWebBrowserCommand = new RelayCommand(PresentMyGovWebBrowser, param => this._CanExecute);
      MyGovWebBrowserLoadCompleteCommand = new RelayCommand(MyGovWebBrowserLoadComplete, param => this._CanExecute);
      this._UserAccountViewList = new ObservableCollection<UserAccountView>();
      RightPanleStateList = new List<UIElement>();
    }

    private UserAccountView ConvertToUserAccountView(DataStore.UserAccount UserAccount)
    {
      var UserAccountView = new UserAccountView();
      UserAccountView.AccessExpires = UserAccount.AccessExpires;
      UserAccountView.AccessToken = UserAccount.AccessToken;
      UserAccountView.AuthorisationCode = UserAccount.AuthorisationCode;
      UserAccountView.RefreshExpires = UserAccount.RefreshExpires;
      UserAccountView.RefreshToken = UserAccount.RefreshToken;
      UserAccountView.Scope = UserAccount.Scope;
      UserAccountView.Username = UserAccount.Username;
      return UserAccountView;
    }
    private ObservableCollection<UserAccountView> ConvertToUserAccountViewList(List<DataStore.UserAccount> UseAcountList)
    {
      var List = new ObservableCollection<UserAccountView>();
      UseAcountList.ForEach(x => List.Add(ConvertToUserAccountView(x)));
      return List;
    }

    public void MyGovWebBrowserLoadComplete(object obj)
    {
      Uri UriString = this.MyGovBrowserViewModel.NavigatedUri;

      if (UriString.OriginalString.StartsWith("https://localhost/oauth_callback?"))
      {
        if (UriString.OriginalString.Contains('?'))
        {
          string[] QuerySplit = UriString.Query.Split('?');
          string ParameterCodeValue = MyHRMobile.Common.Utility.HttpTools.GetQueryParameter(QuerySplit[1], "code");
          if (!string.IsNullOrWhiteSpace(ParameterCodeValue))
          {
            UiService.CurrectUserAccount = new DataStore.UserAccount();
            UiService.CurrectUserAccount.AuthorisationCode = ParameterCodeValue.Trim();
            UiService.CurrectUserAccount.Username = this.AddUserAccountViewModel.UserAccountNameText;
            if (UiService.GetAccessToken())
            {
              this.AddUserAccountViewList = ConvertToUserAccountViewList(UiService.ApplicationStore.UserList);
              //this.CurrentUserAccount = ConvertToUserAccountView(UiService.ApplicationStore.UserList.SingleOrDefault(x => x.Username == UiService.CurrectUserAccount.Username));

              DropRightPanel();
              ReportMyGovLoginOutcome(true);
            }
          }
          else
          {
            //Cancel
            //https://localhost/oauth_callback?error=invalid_grant&error_description=Authentication+cancelled+by+user

            string ParameterErrorValue = MyHRMobile.Common.Utility.HttpTools.GetQueryParameter(QuerySplit[1], "error");
            if (!string.IsNullOrWhiteSpace(ParameterErrorValue))
            {
              this.MyGovError = ParameterErrorValue;
              var ErrorDescription = MyHRMobile.Common.Utility.HttpTools.GetQueryParameter(QuerySplit[1], "error_description");
              if (!string.IsNullOrWhiteSpace(ErrorDescription))
              {
                this.MyGovErrorDescription = ErrorDescription;
              }
              ReportMyGovLoginOutcome(false);
            }
          }
        }
      }
      else if (UriString.OriginalString.StartsWith("https://apinams.ehealthvendortest.health.gov.au/api/oauth/ams.ehealthvendortest.health.gov.au/api/oauth/authorize??"))
      {
        //Cancel from Secret Question
        //https://apinams.ehealthvendortest.health.gov.au/api/oauth/ams.ehealthvendortest.health.gov.au/api/oauth/authorize??error=invalid_grant&error_description=Authentication+cancelled+by+user
        if (UriString.OriginalString.Contains('?'))
        {
          string[] QuerySplit = UriString.Query.Split('?');
          if (QuerySplit.Count() == 3)
          {
            string Query = QuerySplit[2];
            string ParameterErrorValue = MyHRMobile.Common.Utility.HttpTools.GetQueryParameter(Query, "error");
            if (!string.IsNullOrWhiteSpace(ParameterErrorValue))
            {
              this.MyGovError = ParameterErrorValue;
              var ErrorDescription = MyHRMobile.Common.Utility.HttpTools.GetQueryParameter(Query, "error_description");
              if (!string.IsNullOrWhiteSpace(ErrorDescription))
              {
                this.MyGovErrorDescription = ErrorDescription;
              }
              ReportMyGovLoginOutcome(false);
            }
            else
            {
              this.MyGovError = $"Unknown Error";
              this.MyGovErrorDescription = $"URL was: {UriString.OriginalString}";
              ReportMyGovLoginOutcome(false);
            }
          }
          else
          {
            this.MyGovError = $"Unknown Error";
            this.MyGovErrorDescription = $"URL was: {UriString.OriginalString}";
            ReportMyGovLoginOutcome(false);
          }
        }
        else
        {
          this.MyGovError = $"Unknown Error";
          this.MyGovErrorDescription = $"URL was: {UriString.OriginalString}";
          ReportMyGovLoginOutcome(false);
        }
      }
    }

    private void ReportMyGovLoginOutcome(bool IsSuccessFull)
    {
      this._MyGovAuthenticationOutcomeViewModel = new MyGovAuthenticationOutcomeViewModel();
      this._MyGovAuthenticationOutcomeViewModel.OutcomeState = IsSuccessFull;
      MyGovAuthenticationOutcomeView MyGovAuthenticationOutcomeView = new MyGovAuthenticationOutcomeView(this);

      Grid.SetRow(MyGovAuthenticationOutcomeView, 1);
      Grid.SetColumn(MyGovAuthenticationOutcomeView, 2);
      DropRightPanel();
      RightPanelAdd(MyGovAuthenticationOutcomeView);
    }

    public void PresentMyGovWebBrowser(object obj)
    {

      if (UiService.ApplicationStore.UserList.SingleOrDefault(x => x.Username == AddUserAccountViewModel.UserAccountNameText.ToLower()) != null)
      {
        MessageBox.Show("This User Account name already exists, must enter a new name.", "Error", MessageBoxButton.OK);
      }
      else
      {
        this._MyGovBrowserViewModel = new ViewModel.MyGovBrowserViewModel();
        MyGovBrowserView MyGovBrowserView = new MyGovBrowserView(this);
        Grid.SetRow(MyGovBrowserView, 1);
        Grid.SetColumn(MyGovBrowserView, 2);
        DropRightPanel();
        RightPanelAdd(MyGovBrowserView);
      }
    }

    public void PresentAddUserAccount(object obj)
    {
      this._AddUserAccountViewModel = new AddUserAccountViewModel();
      AddUserAccountView AddUserAccountView = new AddUserAccountView(this);
      Grid.SetRow(AddUserAccountView, 1);
      Grid.SetColumn(AddUserAccountView, 2);
      DropRightPanel();
      RightPanelAdd(AddUserAccountView);
    }

    public void DeleteUserAccount(object obj)
    {
      if (this.CurrentUserAccount != null)
      {
        if (MessageBox.Show($"Confirm deletion of Account: {this.CurrentUserAccount.Username}", "Account deletion confirmation", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
        {
          string TempAccountDeletedName = this.CurrentUserAccount.Username;
          var AccountToDelete = UiService.ApplicationStore.UserList.SingleOrDefault(x => x.Username.ToLower() == this.CurrentUserAccount.Username.ToLower());
          if (AccountToDelete != null)
          {
            UiService.ApplicationStore.UserList.Remove(AccountToDelete);
            UiService.CurrectUserAccount = null;
            UiService.SaveApplicationStore();
            DropRightPanel();
            this.AddUserAccountViewList = ConvertToUserAccountViewList(UiService.ApplicationStore.UserList);
            MessageBox.Show($"The Account named: {TempAccountDeletedName} was deleted.", "Account deletion success", MessageBoxButton.OK);
          }
          else
          {
            throw new NullReferenceException($"Could not locate account with name {this.CurrentUserAccount.Username.ToLower()} within the UiService.ApplicationStore.UserList");
          }
        }
      }
      else
      {
        MessageBox.Show("There is not Account selected to delete.", "No Account Selected", MessageBoxButton.OK);
      }
    }

    public void LoadSelectedRecord(object obj)
    {
      if (obj != null && obj is UserAccountRecord UserAccountRecord)
      {
        this._CurrentUserAccount.SelectedUserAccountRecord = UserAccountRecord;
        this.TextEditorViewModel = new TextEditorViewModel();
        if (UiService.GetPatientDetails(this))
        {
          PatientBannerView PatientBannerView = new PatientBannerView(this);

          ApiCallView ApiCallView = new ApiCallView(this);

          DisplayTextEditorView TextEditorView = new DisplayTextEditorView(this);


          Grid OuterGrid = new Grid();
          RowDefinition GridRow1 = new RowDefinition();
          GridRow1.Height = new GridLength(0, GridUnitType.Auto);

          RowDefinition GridRow2 = new RowDefinition();
          GridRow2.Height = new GridLength(45);

          RowDefinition GridRow3 = new RowDefinition();
          GridRow3.Height = new GridLength(1, GridUnitType.Star);


          OuterGrid.RowDefinitions.Add(GridRow1);
          OuterGrid.RowDefinitions.Add(GridRow2);
          OuterGrid.RowDefinitions.Add(GridRow3);

          Grid.SetColumn(PatientBannerView, 0);
          Grid.SetRow(PatientBannerView, 0);

          Grid.SetColumn(ApiCallView, 0);
          Grid.SetRow(ApiCallView, 1);

          Grid.SetColumn(TextEditorView, 0);
          Grid.SetRow(TextEditorView, 2);

          OuterGrid.Children.Add(PatientBannerView);
          OuterGrid.Children.Add(ApiCallView);
          OuterGrid.Children.Add(TextEditorView);

          Grid.SetColumn(OuterGrid, 1);
          Grid.SetRow(OuterGrid, 0);
          RightPanelAdd(OuterGrid);

        }
      }
    }

    public void LoadRecordList(object obj)
    {
      this.TextEditorViewModel = new TextEditorViewModel();
      UiService.GetRecordList(this.CurrentUserAccount, this);
      DisplayTextEditorView TextEditorView = new DisplayTextEditorView(this);

      Grid.SetColumn(TextEditorView, 1);
      Grid.SetRow(TextEditorView, 0);

      RightPanelAdd(TextEditorView);

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
        if (_CurrentUserAccount != null)
        {
          try
          {
            UiService.GetRefeashToken(_CurrentUserAccount);
            LoadRecordList(null);
          }
          catch (Exception Exec)
          {
            MessageBox.Show($"No internet connectivity. Error: {Exec.Message}", "No internet", MessageBoxButton.OK);
          }
        }
        NotifyPropertyChanged("CurrentUserAccount");
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

    private void RightPanelAdd(UIElement Element)
    {
      //Clear anything already there
      DropRightPanel();

      //Remember State      
      RightPanleStateList.Add(Element);

      //Add the elements to Grid      
      MainGrid.Children.Add(Element);
    }

    private void DropRightPanel()
    {
      //Remove all elements in the state list
      RightPanleStateList.ForEach(x => MainGrid.Children.Remove(x));
    }

  }

  public class UserAccountView : ViewModelBase, INotifyPropertyChanged
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

    private UserAccountRecord _SelectedUserAccountRecord { get; set; }
    public UserAccountRecord SelectedUserAccountRecord
    {
      get
      {
        return _SelectedUserAccountRecord;
      }
      set
      {
        _SelectedUserAccountRecord = value;
        NotifyPropertyChanged("SelectedUserAccountRecord");

      }
    }

    private ObservableCollection<UserAccountRecord> _UserAccountRecordList { get; set; }
    public ObservableCollection<UserAccountRecord> UserAccountRecordList
    {
      get { return _UserAccountRecordList; }
      set
      {
        if (value != _UserAccountRecordList)
        {
          _UserAccountRecordList = value;
          NotifyPropertyChanged("UserAccountRecordList");
        }
      }
    }

    public override string ToString()
    {
      return Username;
    }
  }

  public class UserAccountRecord
  {
    public string FormatedName
    {
      get
      {
        return $"{this.Family.ToUpper()}, {this.Given}";
      }
    }
    public string FormatedIHI
    {
      get
      {
        return StringTools.FormatedIHI(this.Ihi);
      }
    }

    public string Family { get; set; }
    public string Given { get; set; }
    public string Ihi { get; set; }
    public string RelationshipTypeDescription { get; set; }
  }
}
