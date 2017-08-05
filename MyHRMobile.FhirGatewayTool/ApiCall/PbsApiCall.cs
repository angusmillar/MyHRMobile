using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyHRMobile.FhirGatewayTool.ApiCall
{
  public class PbsApiCall : IApiCall
  {
    public PbsApiCall()
    {
      CallDisplayElement = new Views.PbsCallView();
    }

    public string ApiCallDescription { get => "Get PBS Items"; }
    public override string ToString()
    {
      return ApiCallDescription;
    }
    public UIElement CallDisplayElement { get; set; }


  }
}
