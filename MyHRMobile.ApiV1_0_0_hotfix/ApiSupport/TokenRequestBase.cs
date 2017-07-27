using System;
using System.Collections.Generic;
using MyHRMobile.Common.Attributes;
using MyHRMobile.Common.Utility;

namespace MyHRMobile.API_V1_0_0_hotfix.ApiSupport
{
  public class TokenRequestBase
  {
    public TokenRequestBase()
    {
      this.Format = "JSON";
    }

    [PropertyLiteral("client_id")]
    public string Client_id { get; set; }

    [PropertyLiteral("client_secret")]
    public string Client_secret { get; set; }

    [PropertyLiteral("grant_type")]
    public string Grant_type { get; protected set; }

    [PropertyLiteral("format")]
    public string Format { get; private set; }

  }
}
