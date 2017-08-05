using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyHRMobile.FhirGatewayTool.ApiCall
{
  public interface IApiCall
  {
    string ApiCallDescription { get; }
    string ToString();
    UIElement CallDisplayElement { get; }
  }
}
