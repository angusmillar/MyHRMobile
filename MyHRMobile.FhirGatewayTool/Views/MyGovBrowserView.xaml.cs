using MyHRMobile.FhirGatewayTool.ViewModel;
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

namespace MyHRMobile.FhirGatewayTool.Views
{
  /// <summary>
  /// Interaction logic for MyGovBrowserView.xaml
  /// </summary>
  public partial class MyGovBrowserView : UserControl
  {
    Presenter Presenter;
    public MyGovBrowserView(Presenter Presenter)
    {
      InitializeComponent();
      this.Presenter = Presenter;
      DataContext = Presenter;
      WebMyGov.Navigate(new Uri("https://apinams.ehealthvendortest.health.gov.au/api/oauth/authorize/login?client_id=28198d27-c475-4695-83d3-1f1f8256e01b&response_type=code&redirect_uri=https://localhost/oauth_callback&scope=https://localhost:8090/pcehr+offline_access"));
    }

    private void WebMyGov_Navigating(object sender, NavigatingCancelEventArgs e)
    {
      this.Presenter.MyGovBrowserViewModel.NavigatedUri = e.Uri;
    }
  }
}
