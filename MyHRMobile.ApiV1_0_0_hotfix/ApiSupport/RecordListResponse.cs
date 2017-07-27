using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using MyHRMobile.API_V1_0_0_hotfix;

namespace MyHRMobile.API_V1_0_0_hotfix.ApiSupport
{
  public class RecordListResponse : ApiResponseBase
  {
    public RecordListResponse(HttpStatusCode StatusCode, string Body, FhirApi.FhirFormat Format)
    {
      this.StatusCode = StatusCode;
      this.Format = Format;
      this.Body = Body;
    }

    public FhirApi.FhirFormat Format { get; set; }
    public string Body { get; set; }

  }
}
