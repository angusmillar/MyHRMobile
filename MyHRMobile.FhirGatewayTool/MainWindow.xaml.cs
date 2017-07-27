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

namespace MyHRMobile.FhirGatewayTool
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private List<UIElement> RightPanleStateList;
    private WebBrowser MyGovWeb = null;
    private GridSplitter Split = null;
    private StackPanel PanelAddUser = null;
    private UiService UiService;
    private ViewModel.MainViewModel MainViewModel;

    public MainWindow()
    {
      InitializeComponent();
      SetupRightPanel();
      RightPanleStateList = new List<UIElement>();

      MainViewModel = new ViewModel.MainViewModel();
      UiService = new UiService();
      UiService.PrimeApplicationStore();
      UiService.LoadApplicationStore();
      UiService.UpdateView(MainViewModel);
      ComboBoxAccount.ItemsSource = MainViewModel.UserAccountViewList;
      ComboBoxAccount.SelectedItem = MainViewModel.UserAccountViewList[0];
    }

    private void SetupRightPanel()
    {
      ColumnDefinition gridGrip = new ColumnDefinition();
      gridGrip.Width = GridLength.Auto;

      ColumnDefinition gridColumnRight = new ColumnDefinition();
      gridColumnRight.Width = new GridLength(2, GridUnitType.Star);
      gridColumnRight.MinWidth = 490;

      GridMain.ColumnDefinitions.Add(gridGrip);
      GridMain.ColumnDefinitions.Add(gridColumnRight);
    }

    private void AddRightPanel(UIElement Element)
    {
      //Clear anything already there
      DropRightPanel();

      //Create the splitter
      Split = new GridSplitter();
      Split.Name = "Split";
      Split.HorizontalAlignment = HorizontalAlignment.Right;
      Split.VerticalAlignment = VerticalAlignment.Stretch;
      Split.ResizeBehavior = GridResizeBehavior.PreviousAndNext;
      Split.Width = 5;
      Split.Background = Brushes.LightGray;
      Grid.SetColumn(Split, 1);

      //Remember State
      RightPanleStateList.Add(Split);
      RightPanleStateList.Add(Element);

      //Add elements to Grid
      GridMain.Children.Add(Split);
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
        string AuthorisationCode = string.Empty;
        string[] QuerySplit = e.Uri.Query.Split('=');
        if (QuerySplit.Count() == 2 && QuerySplit[0] == "?code")
        {
          UiService.CurrectUserAccount.AuthorisationCode = QuerySplit[1].Trim();
          if (UiService.GetAccessToken())
          {

            ComboBoxAccount.ItemsSource = UiService.ApplicationStore.UserList;
            ComboBoxAccount.SelectedItem = UiService.ApplicationStore.UserList.SingleOrDefault(x => x.Username == UiService.CurrectUserAccount.Username);
            DropRightPanel();
          }

        }
      }
    }

    private void Button_Click_AddUser(object sender, RoutedEventArgs e)
    {
      //DropRightPanel();
      UiService.CurrectUserAccount = new DataStore.UserAccount();

      string MyGovButtonText = "Save & MyGov Login";
      PanelAddUser = new StackPanel();
      PanelAddUser.Orientation = Orientation.Vertical;
      PanelAddUser.Margin = new Thickness(10);

      StackPanel PanelHorizontalsInfo = new StackPanel();
      PanelHorizontalsInfo.Orientation = Orientation.Horizontal;
      PanelAddUser.Children.Add(PanelHorizontalsInfo);
      Border InfoTextBoarder = new Border();
      InfoTextBoarder.BorderBrush = Brushes.DarkGray;
      InfoTextBoarder.BorderThickness = new Thickness(2);
      InfoTextBoarder.Width = 400;
      InfoTextBoarder.Margin = new Thickness(10, 10, 0, 0);
      PanelHorizontalsInfo.Children.Add(InfoTextBoarder);

      TextBlock InfoText = new TextBlock();
      InfoText.TextWrapping = TextWrapping.Wrap;
      InfoText.Text = $"Create a friendly Account Username for this user . When you hit '{MyGovButtonText}' you will be sent to the MyGov authentication page. If the authentication is successful your new account will be save for use.";
      InfoTextBoarder.Child = InfoText;



      StackPanel PanelHorizontalControls = new StackPanel();
      PanelHorizontalControls.Orientation = Orientation.Horizontal;
      PanelAddUser.Children.Add(PanelHorizontalControls);

      Label LabelUsername = new Label();
      LabelUsername.Content = "Username:";
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
        MyGovWeb = new WebBrowser();
        MyGovWeb.LoadCompleted += MyGovWeb_LoadCompleted;
        Grid.SetRow(MyGovWeb, 1);
        Grid.SetColumn(MyGovWeb, 2);
        MyGovWeb.Navigate(new Uri("https://apinams.ehealthvendortest.health.gov.au/api/oauth/authorize/login?client_id=28198d27-c475-4695-83d3-1f1f8256e01b&response_type=code&redirect_uri=https://localhost/oauth_callback&scope=https://localhost:8090/pcehr+offline_access"));
        AddRightPanel(MyGovWeb);
      }
    }

    private void ComboBoxAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.Source != null && e.Source is ComboBox AccountCombo)
      {
        if (AccountCombo.SelectedItem is ViewModel.UserAccountView UserAccountView)
        {
          UiService.SetCurrentUserAcccount(UserAccountView);
        }
      }
    }
  }
}
