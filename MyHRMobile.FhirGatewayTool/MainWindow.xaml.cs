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
      UiService.PrimeApplicationStore();
      UiService.LoadApplicationStore();
      UiService.UpdateView(Presenter);
      DataContext = Presenter;

      SetupRightPanel();

    }

    private void SetupRightPanel()
    {
      //ColumnDefinition gridGrip = new ColumnDefinition();
      //gridGrip.Width = GridLength.Auto;

      ColumnDefinition ColumnRight = new ColumnDefinition();
      ColumnRight.Width = new GridLength(1, GridUnitType.Star);
      //ColumnRight.MinWidth = 490;
      //gridColumnRight.MaxWidth = 800;
      //GridMain.ColumnDefinitions.Add(gridGrip);
      GridMain.ColumnDefinitions.Add(ColumnRight);


    }

    private void AddRightPanel(UIElement Element)
    {
      //Clear anything already there
      DropRightPanel();

      //Create the splitter
      //GridSplitter Split = new GridSplitter();
      //Split.Name = "Split";
      //Split.HorizontalAlignment = HorizontalAlignment.Right;
      //Split.VerticalAlignment = VerticalAlignment.Stretch;
      //Split.ResizeBehavior = GridResizeBehavior.PreviousAndNext;
      //Split.Width = 5;
      //Split.Background = Brushes.LightGray;
      //Grid.SetColumn(Split, 1);

      ////Remember State
      //RightPanleStateList.Add(Split);
      RightPanleStateList.Add(Element);

      //Add elements to Grid
      //GridMain.Children.Add(Split);
      GridMain.Children.Add(Element);
    }

    private void DropRightPanel()
    {
      //GridMain.Children.Remove(Split);
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
              ReportMyGovLoginError();
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
              ReportMyGovLoginError();
            }
            else
            {
              Presenter.MyGovError = $"Unknown Error";
              Presenter.MyGovErrorDescription = $"URL was: {e.Uri.OriginalString}";
              ReportMyGovLoginError();
            }
          }
          else
          {
            Presenter.MyGovError = $"Unknown Error";
            Presenter.MyGovErrorDescription = $"URL was: {e.Uri.OriginalString}";
            ReportMyGovLoginError();
          }
        }
        else
        {
          Presenter.MyGovError = $"Unknown Error";
          Presenter.MyGovErrorDescription = $"URL was: {e.Uri.OriginalString}";
          ReportMyGovLoginError();
        }
      }
    }

    private void ReportMyGovLoginError()
    {
      Border Border = new Border();
      Border.BorderThickness = new Thickness(3);
      Border.BorderBrush = Brushes.Salmon;
      Border.Margin = new Thickness(10);

      StackPanel VerticalStack = new StackPanel();
      VerticalStack.Margin = new Thickness(20);
      VerticalStack.Orientation = Orientation.Vertical;
      VerticalStack.HorizontalAlignment = HorizontalAlignment.Left;
      Border.Child = VerticalStack;

      StackPanel HorizontalStackOne = new StackPanel();
      HorizontalStackOne.Orientation = Orientation.Horizontal;
      HorizontalStackOne.HorizontalAlignment = HorizontalAlignment.Left;
      VerticalStack.Children.Add(HorizontalStackOne);

      Label ErrorDesc = new Label();
      ErrorDesc.Content = "MyGov Error:";
      ErrorDesc.FontSize = 14;
      HorizontalStackOne.Children.Add(ErrorDesc);

      Label LabelErrorValue = new Label();
      LabelErrorValue.FontSize = 14;
      LabelErrorValue.Foreground = Brushes.Red;
      Binding binding = new Binding();
      binding.Path = new PropertyPath("MyGovError");
      binding.Source = Presenter;  // view model?
      BindingOperations.SetBinding(LabelErrorValue, Label.ContentProperty, binding);
      HorizontalStackOne.Children.Add(LabelErrorValue);

      StackPanel HorizontalStackTwo = new StackPanel();
      HorizontalStackTwo.Orientation = Orientation.Horizontal;
      HorizontalStackTwo.HorizontalAlignment = HorizontalAlignment.Left;
      VerticalStack.Children.Add(HorizontalStackTwo);

      Label ErrorDescriptionDesc = new Label();
      ErrorDescriptionDesc.Content = "Error Description:";
      ErrorDescriptionDesc.FontSize = 14;
      HorizontalStackTwo.Children.Add(ErrorDescriptionDesc);

      Label LabelErrorDescriptionValue = new Label();
      LabelErrorDescriptionValue.FontSize = 14;
      LabelErrorDescriptionValue.Foreground = Brushes.Red;
      Binding binding2 = new Binding();
      binding2.Path = new PropertyPath("MyGovErrorDescription");
      binding2.Source = Presenter;  // view model?
      BindingOperations.SetBinding(LabelErrorDescriptionValue, Label.ContentProperty, binding2);
      HorizontalStackTwo.Children.Add(LabelErrorDescriptionValue);

      StackPanel HorizontalStackThree = new StackPanel();
      HorizontalStackThree.Orientation = Orientation.Horizontal;
      HorizontalStackThree.HorizontalAlignment = HorizontalAlignment.Left;
      VerticalStack.Children.Add(HorizontalStackThree);

      Button ButCancel = new Button();
      ButCancel.Content = "OK";
      ButCancel.Width = 50;
      ButCancel.Height = 20;
      ButCancel.Click += Button_Click_AddUser;
      HorizontalStackThree.Children.Add(ButCancel);

      Grid.SetColumn(Border, 2);
      Grid.SetRow(Border, 0);
      AddRightPanel(Border);
    }

    private void Button_Click_AddUser(object sender, RoutedEventArgs e)
    {
      //DropRightPanel();
      UiService.CurrectUserAccount = new DataStore.UserAccount();

      string MyGovButtonText = "Save & MyGov Login";
      StackPanel PanelAddUser = new StackPanel();
      PanelAddUser.Orientation = Orientation.Vertical;
      PanelAddUser.Margin = new Thickness(10);
      PanelAddUser.HorizontalAlignment = HorizontalAlignment.Left;

      StackPanel PanelHorizontalsInfo = new StackPanel();
      PanelHorizontalsInfo.Orientation = Orientation.Horizontal;
      PanelAddUser.Children.Add(PanelHorizontalsInfo);
      Border InfoTextBoarder = new Border();
      InfoTextBoarder.BorderBrush = Brushes.DarkGray;
      InfoTextBoarder.BorderThickness = new Thickness(2);
      InfoTextBoarder.Width = 500;
      //InfoTextBoarder.Margin = new Thickness(10, 10, 0, 0);
      PanelHorizontalsInfo.Children.Add(InfoTextBoarder);

      TextBlock InfoText = new TextBlock();
      InfoText.TextWrapping = TextWrapping.Wrap;
      InfoText.Text = $"Create a friendly account name for this user . When you hit '{MyGovButtonText}' you will be sent to the MyGov authentication page. If the authentication is successful your new account will be save for use.";
      InfoText.FontSize = 14;
      InfoText.Margin = new Thickness(5);
      InfoTextBoarder.Child = InfoText;

      StackPanel PanelHorizontalControls = new StackPanel();
      PanelHorizontalControls.Orientation = Orientation.Horizontal;
      PanelAddUser.Children.Add(PanelHorizontalControls);

      Label LabelUsername = new Label();
      LabelUsername.Content = "Account name:";
      TextBox TextBoxUsername = new TextBox();
      TextBoxUsername.Margin = new Thickness(3);
      TextBoxUsername.Width = 150;
      TextBoxUsername.TextChanged += TextBoxUsername_TextChanged;
      PanelHorizontalControls.Children.Add(LabelUsername);
      PanelHorizontalControls.Children.Add(TextBoxUsername);

      Grid.SetRow(PanelAddUser, 1);
      Grid.SetColumn(PanelAddUser, 2);

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


      AddRightPanel(PanelAddUser);
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
        MessageBox.Show("This Username already exists, must enter a new Username.", "Error", MessageBoxButton.OK);
      }
      else
      {
        WebBrowser MyGovWeb = new WebBrowser();
        MyGovWeb.LoadCompleted += MyGovWeb_LoadCompleted;
        Grid.SetRow(MyGovWeb, 1);
        Grid.SetColumn(MyGovWeb, 2);
        MyGovWeb.Navigate(new Uri("https://apinams.ehealthvendortest.health.gov.au/api/oauth/authorize/login?client_id=28198d27-c475-4695-83d3-1f1f8256e01b&response_type=code&redirect_uri=https://localhost/oauth_callback&scope=https://localhost:8090/pcehr+offline_access"));
        AddRightPanel(MyGovWeb);
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
      AddRightPanel(RightGrid);

    }


  }
}
