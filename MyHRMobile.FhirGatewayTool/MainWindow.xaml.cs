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
using MyHRMobile.FhirGatewayTool.CustomControl;

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
      //RightPanleStateList = new List<UIElement>();
      Presenter = new ViewModel.Presenter();
      UiService = new UiService();
      Presenter.MainGrid = GridMain;
      Presenter.UiService = UiService;
      DataContext = Presenter;
    }

  }
}
