using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHRMobile.Common.Extentions;
using System.Collections.ObjectModel;
using MyHRMobile.FhirGatewayTool.Extensions;
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
      this._UserAccountViewList = new ObservableCollection<UserAccountView>();
      RightPanleStateList = new List<UIElement>();
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
      ICSharpCode.AvalonEdit.TextEditor TextEditor = new ICSharpCode.AvalonEdit.TextEditor();
      TextEditor.SetSyntaxType(AvalonEditSyntaxTypes.Xml);

      //AvalonEdit Folding
      FoldingManager = ICSharpCode.AvalonEdit.Folding.FoldingManager.Install(TextEditor.TextArea);
      FoldingStrategy = new ICSharpCode.AvalonEdit.Folding.XmlFoldingStrategy();
      FoldingStrategy = new ICSharpCode.AvalonEdit.Folding.XmlFoldingStrategy();

      TextEditor.WordWrap = false;
      TextEditor.ShowLineNumbers = true;
      TextEditor.FontFamily = new FontFamily("Consolas");
      TextEditor.FontSize = 12;
      TextEditor.LineNumbersForeground = Brushes.DarkGray;
      ExtentionAvalonEdit.AvalonEditContextMenu(TextEditor);

      if (UiService.GetRecordList(this.CurrentUserAccount, this))
      {

        TextEditor.Text = MyHRMobile.Common.Utility.XmlTool.BeautifyXML(this.TextEditorRight);
        FoldingStrategy.UpdateFoldings(FoldingManager, TextEditor.Document);
      }
      else
      {
        TextEditor.Text = this.TextEditorRight;
      }

      Grid RightGrid = new Grid();
      ColumnDefinition ColumnOne = new ColumnDefinition();
      RightGrid.ColumnDefinitions.Add(ColumnOne);

      RowDefinition RowOne = new RowDefinition();
      //RowOne.Height = new GridLength(0, GridUnitType.Auto);
      RightGrid.RowDefinitions.Add(RowOne);

      RowDefinition RowTwo = new RowDefinition();
      //RowTwo.Height = new GridLength(10);
      RightGrid.RowDefinitions.Add(RowTwo);

      Grid.SetColumn(TextEditor, 0);
      Grid.SetRowSpan(TextEditor, 2);

      Grid.SetColumn(RightGrid, 1);
      Grid.SetRow(RightGrid, 0);

      RightGrid.Children.Add(TextEditor);
      //DropRightPanel();
      // SetupRightPanel();
      RightPanelAdd(RightGrid);

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
