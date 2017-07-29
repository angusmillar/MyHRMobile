using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyHRMobile.FhirGatewayTool.Extensions;

namespace MyHRMobile.FhirGatewayTool
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private List<UIElement> RightPanleStateList;
    //private WebBrowser MyGovWeb = null;
    //private GridSplitter Split = null;
    //private StackPanel PanelAddUser = null;
    private UiService UiService;
    private ViewModel.Presenter Presenter;

    public MainWindow()
    {

      InitializeComponent();
      RightPanleStateList = new List<UIElement>();

      Presenter = new ViewModel.Presenter();
      UiService = new UiService();
      Presenter.UiService = UiService;
      DataContext = Presenter;
    }


    private void RightPanelAdd(UIElement Element)
    {
      //Clear anything already there
      DropRightPanel();

      //Remember State      
      RightPanleStateList.Add(Element);

      //Add the elements to Grid      
      GridMain.Children.Add(Element);
    }

    private void DropRightPanel()
    {
      //Remove all elements in the state list
      RightPanleStateList.ForEach(x => GridMain.Children.Remove(x));
    }

    private void MyGovWeb_LoadCompleted(object sender, NavigationEventArgs e)
    {

      if (e.Uri.OriginalString.StartsWith("https://localhost/oauth_callback?"))
      {
        if (e.Uri.OriginalString.Contains('?'))
        {
          string[] QuerySplit = e.Uri.Query.Split('?');
          string ParameterCodeValue = MyHRMobile.Common.Utility.HttpTools.GetQueryParameter(QuerySplit[1], "code");
          if (!string.IsNullOrWhiteSpace(ParameterCodeValue))
          {
            UiService.CurrectUserAccount.AuthorisationCode = ParameterCodeValue.Trim();
            if (UiService.GetAccessToken())
            {
              ComboBoxAccount.ItemsSource = UiService.ApplicationStore.UserList;
              ComboBoxAccount.SelectedItem = UiService.ApplicationStore.UserList.SingleOrDefault(x => x.Username == UiService.CurrectUserAccount.Username);
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
              Presenter.MyGovError = ParameterErrorValue;
              var ErrorDescription = MyHRMobile.Common.Utility.HttpTools.GetQueryParameter(QuerySplit[1], "error_description");
              if (!string.IsNullOrWhiteSpace(ErrorDescription))
              {
                Presenter.MyGovErrorDescription = ErrorDescription;
              }
              ReportMyGovLoginOutcome(false);
            }
          }
        }
      }
      else if (e.Uri.OriginalString.StartsWith("https://apinams.ehealthvendortest.health.gov.au/api/oauth/ams.ehealthvendortest.health.gov.au/api/oauth/authorize??"))
      {
        //Cancel from Secret Question
        //https://apinams.ehealthvendortest.health.gov.au/api/oauth/ams.ehealthvendortest.health.gov.au/api/oauth/authorize??error=invalid_grant&error_description=Authentication+cancelled+by+user
        if (e.Uri.OriginalString.Contains('?'))
        {
          string[] QuerySplit = e.Uri.Query.Split('?');
          if (QuerySplit.Count() == 3)
          {
            string Query = QuerySplit[2];
            string ParameterErrorValue = MyHRMobile.Common.Utility.HttpTools.GetQueryParameter(Query, "error");
            if (!string.IsNullOrWhiteSpace(ParameterErrorValue))
            {
              Presenter.MyGovError = ParameterErrorValue;
              var ErrorDescription = MyHRMobile.Common.Utility.HttpTools.GetQueryParameter(Query, "error_description");
              if (!string.IsNullOrWhiteSpace(ErrorDescription))
              {
                Presenter.MyGovErrorDescription = ErrorDescription;
              }
              ReportMyGovLoginOutcome(false);
            }
            else
            {
              Presenter.MyGovError = $"Unknown Error";
              Presenter.MyGovErrorDescription = $"URL was: {e.Uri.OriginalString}";
              ReportMyGovLoginOutcome(false);
            }
          }
          else
          {
            Presenter.MyGovError = $"Unknown Error";
            Presenter.MyGovErrorDescription = $"URL was: {e.Uri.OriginalString}";
            ReportMyGovLoginOutcome(false);
          }
        }
        else
        {
          Presenter.MyGovError = $"Unknown Error";
          Presenter.MyGovErrorDescription = $"URL was: {e.Uri.OriginalString}";
          ReportMyGovLoginOutcome(false);
        }
      }
    }

    private void ReportMyGovLoginOutcome(bool IsSuccessFull)
    {
      DockPanel OuterDockPanel = new DockPanel();
      Grid.SetRow(OuterDockPanel, 1);
      Grid.SetColumn(OuterDockPanel, 2);

      StackPanel VerticalPanel = new StackPanel();
      VerticalPanel.Orientation = Orientation.Vertical;
      VerticalPanel.Margin = new Thickness(5);
      VerticalPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
      OuterDockPanel.Children.Add(VerticalPanel);

      GroupBox MainGroupBox = new GroupBox();
      MainGroupBox.Header = "Create a User Account";
      MainGroupBox.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      VerticalPanel.Children.Add(MainGroupBox);

      GridControl InnerGrid = new GridControl();
      ColumnDefinition ColOne = new ColumnDefinition();
      ColOne.Width = new GridLength(0, GridUnitType.Auto);
      InnerGrid.ColumnDefinitions.Add(ColOne);
      RowDefinition RowOne = new RowDefinition();
      RowOne.Height = new GridLength(0, GridUnitType.Auto);
      InnerGrid.RowDefinitions.Add(RowOne);
      RowDefinition RowTwo = new RowDefinition();
      RowTwo.Height = new GridLength(0, GridUnitType.Auto);
      InnerGrid.RowDefinitions.Add(RowTwo);
      MainGroupBox.Content = InnerGrid;

      if (IsSuccessFull)
      {
        TextBlock InfoTextSuccess = new TextBlock();
        InfoTextSuccess.TextWrapping = TextWrapping.Wrap;
        InfoTextSuccess.Text = $"You MyGov authentication was successfully.";
        InfoTextSuccess.FontSize = 24;
        InfoTextSuccess.Foreground = Brushes.Green;
        InfoTextSuccess.Margin = new Thickness(5);
        Grid.SetColumn(InfoTextSuccess, 0);
        Grid.SetRow(InfoTextSuccess, 0);
        InnerGrid.Children.Add(InfoTextSuccess);

        TextBlock InfoTextMessage = new TextBlock();
        InfoTextMessage.TextWrapping = TextWrapping.Wrap;
        InfoTextMessage.Text = $"You are now able to select you Account name from the drop down on the left.\nYour account will be remembered and authentication managed. ";
        InfoTextMessage.Margin = new Thickness(5);
        Grid.SetColumn(InfoTextMessage, 0);
        Grid.SetRow(InfoTextMessage, 1);
        InnerGrid.Children.Add(InfoTextMessage);
      }
      else
      {
        TextBlock InfoTextSuccess = new TextBlock();
        InfoTextSuccess.TextWrapping = TextWrapping.Wrap;
        InfoTextSuccess.Text = $"You MyGov authentication was unsuccessfully.";
        InfoTextSuccess.FontSize = 24;
        InfoTextSuccess.Foreground = Brushes.Maroon;
        InfoTextSuccess.Margin = new Thickness(5);
        Grid.SetColumn(InfoTextSuccess, 0);
        Grid.SetRow(InfoTextSuccess, 0);
        InnerGrid.Children.Add(InfoTextSuccess);

        StackPanel HozStackOne = new StackPanel();
        HozStackOne.Orientation = Orientation.Horizontal;
        Grid.SetColumn(HozStackOne, 0);
        Grid.SetRow(HozStackOne, 1);
        InnerGrid.Children.Add(HozStackOne);

        Label LabelErrorCode = new Label();
        LabelErrorCode.Content = "MyGov Error Code:";
        LabelErrorCode.FontWeight = FontWeights.DemiBold;

        HozStackOne.Children.Add(LabelErrorCode);

        TextBlock InfoTextMessage = new TextBlock();
        InfoTextMessage.TextWrapping = TextWrapping.Wrap;
        Binding binding = new Binding();
        binding.Path = new PropertyPath("MyGovError");
        binding.Source = Presenter;  // view model?
        BindingOperations.SetBinding(InfoTextMessage, TextBlock.TextProperty, binding);
        InfoTextMessage.Margin = new Thickness(5);
        HozStackOne.Children.Add(InfoTextMessage);

        RowDefinition RowThree = new RowDefinition();
        RowThree.Height = new GridLength(0, GridUnitType.Auto);
        InnerGrid.RowDefinitions.Add(RowThree);

        StackPanel HozStackTwo = new StackPanel();
        HozStackTwo.Orientation = Orientation.Horizontal;
        Grid.SetColumn(HozStackTwo, 0);
        Grid.SetRow(HozStackTwo, 2);
        InnerGrid.Children.Add(HozStackTwo);

        Label LabelErrorDesc = new Label();
        LabelErrorDesc.Content = "MyGov Error Description:";
        LabelErrorDesc.FontWeight = FontWeights.DemiBold;
        HozStackTwo.Children.Add(LabelErrorDesc);

        TextBlock TextBlockErrDesc = new TextBlock();
        TextBlockErrDesc.TextWrapping = TextWrapping.Wrap;
        TextBlockErrDesc.Margin = new Thickness(5);
        Binding binding2 = new Binding();
        binding2.Path = new PropertyPath("MyGovErrorDescription");
        binding2.Source = Presenter;  // view model?
        BindingOperations.SetBinding(TextBlockErrDesc, TextBlock.TextProperty, binding2);
        HozStackTwo.Children.Add(TextBlockErrDesc);
      }

      RightPanelAdd(OuterDockPanel);
    }

    private void Button_Click_AddUser(object sender, RoutedEventArgs e)
    {
      UiService.CurrectUserAccount = new DataStore.UserAccount();

      string MyGovButtonText = "Save & MyGov Login";

      DockPanel OuterDockPanel = new DockPanel();
      Grid.SetRow(OuterDockPanel, 1);
      Grid.SetColumn(OuterDockPanel, 2);

      StackPanel VerticalPanel = new StackPanel();
      VerticalPanel.Orientation = Orientation.Vertical;
      VerticalPanel.Margin = new Thickness(5);
      //VerticalPanel.HorizontalAlignment = HorizontalAlignment.Left;
      VerticalPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
      OuterDockPanel.Children.Add(VerticalPanel);

      GroupBox MainGroupBox = new GroupBox();
      MainGroupBox.Header = "Create a User Account";
      MainGroupBox.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      VerticalPanel.Children.Add(MainGroupBox);

      GridControl InnerGrid = new GridControl();
      ColumnDefinition ColOne = new ColumnDefinition();
      ColOne.Width = new GridLength(0, GridUnitType.Auto);
      InnerGrid.ColumnDefinitions.Add(ColOne);
      RowDefinition RowOne = new RowDefinition();
      RowOne.Height = new GridLength(0, GridUnitType.Auto);
      InnerGrid.RowDefinitions.Add(RowOne);
      RowDefinition RowTwo = new RowDefinition();
      RowTwo.Height = new GridLength(0, GridUnitType.Auto);
      InnerGrid.RowDefinitions.Add(RowTwo);
      MainGroupBox.Content = InnerGrid;

      TextBlock InfoText = new TextBlock();
      InfoText.TextWrapping = TextWrapping.Wrap;
      InfoText.Text = $"Create a friendly account name for this user. \nHit the '{MyGovButtonText}' button and you will be sent to the MyGov authentication page. \nIf you authentication is successful your new account will be save for use.";
      InfoText.FontSize = 14;
      InfoText.Margin = new Thickness(5);
      Grid.SetColumn(InfoText, 0);
      Grid.SetRow(InfoText, 0);
      InnerGrid.Children.Add(InfoText);

      StackPanel PanelHorizontalControls = new StackPanel();
      PanelHorizontalControls.Orientation = Orientation.Horizontal;
      PanelHorizontalControls.Margin = new Thickness(0, 60, 0, 60);
      Grid.SetColumn(PanelHorizontalControls, 0);
      Grid.SetRow(PanelHorizontalControls, 1);
      InnerGrid.Children.Add(PanelHorizontalControls);

      Label LabelUsername = new Label();
      LabelUsername.Content = "User Account name:";
      TextBox TextBoxUsername = new TextBox();
      TextBoxUsername.Margin = new Thickness(3);
      TextBoxUsername.Width = 250;
      TextBoxUsername.MaxLength = 40;
      TextBoxUsername.TextChanged += TextBoxUsername_TextChanged;
      PanelHorizontalControls.Children.Add(LabelUsername);
      PanelHorizontalControls.Children.Add(TextBoxUsername);

      Button MyGovBut = new Button();
      MyGovBut.Content = MyGovButtonText;
      MyGovBut.Margin = new Thickness(3);
      MyGovBut.Click += MyGovBut_Click;
      PanelHorizontalControls.Children.Add(MyGovBut);


      Button CancelAddUserBut = new Button();
      CancelAddUserBut.Content = "Cancel";
      CancelAddUserBut.Margin = new Thickness(3);
      CancelAddUserBut.Click += CancelAddUserBut_Click;
      PanelHorizontalControls.Children.Add(CancelAddUserBut);


      RightPanelAdd(OuterDockPanel);
    }

    private void TextBoxUsername_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (UiService.CurrectUserAccount != null)
      {
        if (e.Source is TextBox TextBox)
        {
          UiService.CurrectUserAccount.Username = TextBox.Text;
        }
        else
        {
          throw new Exception("Can not cast to TextBlock on event for username text change");
        }
      }
      else
      {
        throw new Exception("Username Text box event, UiService.CurrectUserAccount is null");
      }
    }

    private void CancelAddUserBut_Click(object sender, RoutedEventArgs e)
    {
      DropRightPanel();
    }

    private void MyGovBut_Click(object sender, RoutedEventArgs e)
    {
      if (UiService.ApplicationStore.UserList.SingleOrDefault(x => x.Username == UiService.CurrectUserAccount.Username) != null)
      {
        MessageBox.Show("This User Account name already exists, must enter a new name.", "Error", MessageBoxButton.OK);
      }
      else
      {
        WebBrowser MyGovWeb = new WebBrowser();
        MyGovWeb.LoadCompleted += MyGovWeb_LoadCompleted;
        Grid.SetRow(MyGovWeb, 1);
        Grid.SetColumn(MyGovWeb, 2);
        MyGovWeb.Navigate(new Uri("https://apinams.ehealthvendortest.health.gov.au/api/oauth/authorize/login?client_id=28198d27-c475-4695-83d3-1f1f8256e01b&response_type=code&redirect_uri=https://localhost/oauth_callback&scope=https://localhost:8090/pcehr+offline_access"));
        RightPanelAdd(MyGovWeb);
      }
    }

    private void Button_Click_GetRecordListTest(object sender, RoutedEventArgs e)
    {
      ICSharpCode.AvalonEdit.TextEditor TextEditor = new ICSharpCode.AvalonEdit.TextEditor();
      TextEditor.SetSyntaxType(AvalonEditSyntaxTypes.Xml);
      TextEditor.WordWrap = false;
      TextEditor.ShowLineNumbers = true;
      TextEditor.FontFamily = new FontFamily("Consolas");
      TextEditor.FontSize = 12;
      ExtentionAvalonEdit.AvalonEditContextMenu(TextEditor);

      if (UiService.GetRecordList(Presenter.CurrentUserAccount, Presenter))
      {

        TextEditor.Text = MyHRMobile.Common.Utility.XmlTool.BeautifyXML(Presenter.TextEditorRight);
      }
      else
      {
        TextEditor.Text = Presenter.TextEditorRight;
      }

      Grid RightGrid = new Grid();
      ColumnDefinition ColumnOne = new ColumnDefinition();
      //ColumnOne.Width = new GridLength(0, GridUnitType.Auto);
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


  }
}
