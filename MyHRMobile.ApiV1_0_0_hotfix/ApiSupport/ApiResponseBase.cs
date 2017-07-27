using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MyHRMobile.API_V1_0_0_hotfix.ApiSupport
{
  public class ApiResponseBase
  {
    public HttpStatusCode StatusCode { get; protected set; }
    public ErrorResponse ErrorResponse { get; protected set; }
  }
}
