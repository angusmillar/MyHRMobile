using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHRMobile.FhirGatewayTool.ApiCall;

namespace MyHRMobile.FhirGatewayTool.ViewModel
{
  public class ApiCallViewModel : ViewModelBase
  {
    public ApiCallViewModel()
    {
      _ApiCallList = new ObservableCollection<IApiCall>();
      _ApiCallList.Add(new PbsApiCall());
      _SelectedApiCall = _ApiCallList[0];
    }

    private IApiCall _SelectedApiCall { get; set; }
    public IApiCall SelectedApiCall
    {
      get
      {
        return _SelectedApiCall;
      }
      set
      {
        _SelectedApiCall = value;
        NotifyPropertyChanged("SelectedApiCall");
      }
    }
    private ObservableCollection<IApiCall> _ApiCallList { get; set; }
    public ReadOnlyObservableCollection<IApiCall> ApiCallList
    {
      get
      {
        return new ReadOnlyObservableCollection<IApiCall>(_ApiCallList);
      }
    }

  }
}
