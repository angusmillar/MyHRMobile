using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyHRMobile.ApiV1_0_0_hotfix.ApiSupport
{
  public class PbsItemsResponse : ApiResponseBase
  {
    public PbsItemsResponse(HttpStatusCode StatusCode, string Body, FhirApi.FhirFormat Format)
    {
      this.StatusCode = StatusCode;
      this.Format = Format;
      this.Body = Body;
      //ParseResponseBody();
    }

    public FhirApi.FhirFormat Format { get; set; }
    public string Body { get; set; }
  }
}
