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
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

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

    private ICommand _AddUserAccountCommand;
    public ICommand AddUserAccountCommand
    {
      get
      {
        return _AddUserAccountCommand;
      }
      set
      {
        _AddUserAccountCommand = value;
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
    private ICSharpCode.AvalonEdit.Folding.FoldingManager FoldingManager;
    private ICSharpCode.AvalonEdit.Folding.XmlFoldingStrategy FoldingStrategy;

    public Presenter()
    {
      LoadRecordListCommand = new RelayCommand(LoadRecordList, param => this._CanExecute);
      SelectedRecordCommand = new RelayCommand(LoadSelectedRecord, param => this._CanExecute);
      AddUserAccountCommand = new RelayCommand(PresentAddUserAccount, param => this._CanExecute);
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
              this.CurrentUserAccount = ConvertToUserAccountView(UiService.ApplicationStore.UserList.SingleOrDefault(x => x.Username == UiService.CurrectUserAccount.Username));
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

      //DockPanel OuterDockPanel = new DockPanel();
      //Grid.SetRow(OuterDockPanel, 1);
      //Grid.SetColumn(OuterDockPanel, 2);

      //StackPanel VerticalPanel = new StackPanel();
      //VerticalPanel.Orientation = Orientation.Vertical;
      //VerticalPanel.Margin = new Thickness(5);
      //VerticalPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
      //OuterDockPanel.Children.Add(VerticalPanel);

      //GroupBox MainGroupBox = new GroupBox();
      //MainGroupBox.Header = "Create a User Account";
      //MainGroupBox.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      //VerticalPanel.Children.Add(MainGroupBox);

      //GridControl InnerGrid = new GridControl();
      //ColumnDefinition ColOne = new ColumnDefinition();
      //ColOne.Width = new GridLength(0, GridUnitType.Auto);
      //InnerGrid.ColumnDefinitions.Add(ColOne);
      //RowDefinition RowOne = new RowDefinition();
      //RowOne.Height = new GridLength(0, GridUnitType.Auto);
      //InnerGrid.RowDefinitions.Add(RowOne);
      //RowDefinition RowTwo = new RowDefinition();
      //RowTwo.Height = new GridLength(0, GridUnitType.Auto);
      //InnerGrid.RowDefinitions.Add(RowTwo);
      //MainGroupBox.Content = InnerGrid;

      //if (IsSuccessFull)
      //{
      //  TextBlock InfoTextSuccess = new TextBlock();
      //  InfoTextSuccess.TextWrapping = TextWrapping.Wrap;
      //  InfoTextSuccess.Text = $"You MyGov authentication was successfully.";
      //  InfoTextSuccess.FontSize = 24;
      //  InfoTextSuccess.Foreground = Brushes.Green;
      //  InfoTextSuccess.Margin = new Thickness(5);
      //  Grid.SetColumn(InfoTextSuccess, 0);
      //  Grid.SetRow(InfoTextSuccess, 0);
      //  InnerGrid.Children.Add(InfoTextSuccess);

      //  TextBlock InfoTextMessage = new TextBlock();
      //  InfoTextMessage.TextWrapping = TextWrapping.Wrap;
      //  InfoTextMessage.Text = $"You are now able to select you Account name from the drop down on the left.\nYour account will be remembered and authentication managed. ";
      //  InfoTextMessage.Margin = new Thickness(5);
      //  Grid.SetColumn(InfoTextMessage, 0);
      //  Grid.SetRow(InfoTextMessage, 1);
      //  InnerGrid.Children.Add(InfoTextMessage);
      //}
      //else
      //{
      //  TextBlock InfoTextSuccess = new TextBlock();
      //  InfoTextSuccess.TextWrapping = TextWrapping.Wrap;
      //  InfoTextSuccess.Text = $"You MyGov authentication was unsuccessfully.";
      //  InfoTextSuccess.FontSize = 24;
      //  InfoTextSuccess.Foreground = Brushes.Maroon;
      //  InfoTextSuccess.Margin = new Thickness(5);
      //  Grid.SetColumn(InfoTextSuccess, 0);
      //  Grid.SetRow(InfoTextSuccess, 0);
      //  InnerGrid.Children.Add(InfoTextSuccess);

      //  StackPanel HozStackOne = new StackPanel();
      //  HozStackOne.Orientation = Orientation.Horizontal;
      //  Grid.SetColumn(HozStackOne, 0);
      //  Grid.SetRow(HozStackOne, 1);
      //  InnerGrid.Children.Add(HozStackOne);

      //  Label LabelErrorCode = new Label();
      //  LabelErrorCode.Content = "MyGov Error Code:";
      //  LabelErrorCode.FontWeight = FontWeights.DemiBold;

      //  HozStackOne.Children.Add(LabelErrorCode);

      //  TextBlock InfoTextMessage = new TextBlock();
      //  InfoTextMessage.TextWrapping = TextWrapping.Wrap;
      //  Binding binding = new Binding();
      //  binding.Path = new PropertyPath("MyGovError");
      //  binding.Source = Presenter;  // view model?
      //  BindingOperations.SetBinding(InfoTextMessage, TextBlock.TextProperty, binding);
      //  InfoTextMessage.Margin = new Thickness(5);
      //  HozStackOne.Children.Add(InfoTextMessage);

      //  RowDefinition RowThree = new RowDefinition();
      //  RowThree.Height = new GridLength(0, GridUnitType.Auto);
      //  InnerGrid.RowDefinitions.Add(RowThree);

      //  StackPanel HozStackTwo = new StackPanel();
      //  HozStackTwo.Orientation = Orientation.Horizontal;
      //  Grid.SetColumn(HozStackTwo, 0);
      //  Grid.SetRow(HozStackTwo, 2);
      //  InnerGrid.Children.Add(HozStackTwo);

      //  Label LabelErrorDesc = new Label();
      //  LabelErrorDesc.Content = "MyGov Error Description:";
      //  LabelErrorDesc.FontWeight = FontWeights.DemiBold;
      //  HozStackTwo.Children.Add(LabelErrorDesc);

      //  TextBlock TextBlockErrDesc = new TextBlock();
      //  TextBlockErrDesc.TextWrapping = TextWrapping.Wrap;
      //  TextBlockErrDesc.Margin = new Thickness(5);
      //  Binding binding2 = new Binding();
      //  binding2.Path = new PropertyPath("MyGovErrorDescription");
      //  binding2.Source = Presenter;  // view model?
      //  BindingOperations.SetBinding(TextBlockErrDesc, TextBlock.TextProperty, binding2);
      //  HozStackTwo.Children.Add(TextBlockErrDesc);
      //}

      //RightPanelAdd(OuterDockPanel);
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

    public void LoadSelectedRecord(object obj)
    {
      if (obj != null && obj is UserAccountRecord UserAccountRecord)
      {
        this._CurrentUserAccount.SelectedUserAccountRecord = UserAccountRecord;
        string test = UserAccountRecord.FormatedName;
        UiService.GetPatientDetails(this);

      }
    }

    public void LoadRecordList(object obj)
    {
      DisplayTextEditorView TextEditorView = new DisplayTextEditorView();
      TextEditorView.TextEditorBody.SetSyntaxType(AvalonEditSyntaxTypes.Xml);

      //AvalonEdit Folding
      FoldingManager = ICSharpCode.AvalonEdit.Folding.FoldingManager.Install(TextEditorView.TextEditorBody.TextArea);
      FoldingStrategy = new ICSharpCode.AvalonEdit.Folding.XmlFoldingStrategy();
      FoldingStrategy = new ICSharpCode.AvalonEdit.Folding.XmlFoldingStrategy();

      TextEditorView.TextEditorBody.WordWrap = false;
      TextEditorView.TextEditorBody.ShowLineNumbers = true;
      TextEditorView.TextEditorBody.FontFamily = new FontFamily("Consolas");
      TextEditorView.TextEditorBody.FontSize = 12;
      TextEditorView.TextEditorBody.LineNumbersForeground = Brushes.DarkGray;
      ExtentionAvalonEdit.AvalonEditContextMenu(TextEditorView.TextEditorBody);

      if (UiService.GetRecordList(this.CurrentUserAccount, this))
      {

        TextEditorView.TextEditorBody.Text = MyHRMobile.Common.Utility.XmlTool.BeautifyXML(this.TextEditorRight);
        FoldingStrategy.UpdateFoldings(FoldingManager, TextEditorView.TextEditorBody.Document);
      }
      else
      {
        TextEditorView.TextEditorBody.Text = this.TextEditorRight;
      }

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
          UiService.GetRefeashToken(_CurrentUserAccount);
          LoadRecordList(null);
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
        if (this.Ihi.Length == 16)
        {
          return $"{this.Ihi.Substring(0, 4)} {this.Ihi.Substring(4, 4)} {this.Ihi.Substring(8, 4)} {this.Ihi.Substring(12, 4)} ";
        }
        else
        {
          return $"{this.Family.ToUpper()}, {this.Given}";
        }
      }
    }

    public string Family { get; set; }
    public string Given { get; set; }
    public string Ihi { get; set; }
  }
}
