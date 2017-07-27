using System;
using System.Collections.Generic;
using MyHRMobile.Common.Attributes;
using MyHRMobile.Common.Utility;

namespace MyHRMobile.API_V1_0_0_hotfix.ApiSupport
{
  public class AccessTokenRequest : TokenRequestBase
  {
    public AccessTokenRequest()
    {
      this.Grant_type = "authorization_code";
      this.Redirect_uri = "https://localhost/oauth_callback";
    }

    [PropertyLiteral("redirect_uri")]
    public string Redirect_uri { get; }

    [PropertyLiteral("code")]
    public string Code { get; set; }

    public List<KeyValuePair<string, string>> GetRequestParametersList()
    {
      var RequestParametersList = new List<KeyValuePair<string, string>>();
      RequestParametersList.Add(new KeyValuePair<string, string>(PropertyUtility.GetLiteral(this, "Client_id"), Client_id));
      RequestParametersList.Add(new KeyValuePair<string, string>(PropertyUtility.GetLiteral(this, "Client_secret"), Client_secret));
      RequestParametersList.Add(new KeyValuePair<string, string>(PropertyUtility.GetLiteral(this, "Grant_type"), Grant_type));
      RequestParametersList.Add(new KeyValuePair<string, string>(PropertyUtility.GetLiteral(this, "Redirect_uri"), Redirect_uri));
      RequestParametersList.Add(new KeyValuePair<string, string>(PropertyUtility.GetLiteral(this, "Format"), Format));
      RequestParametersList.Add(new KeyValuePair<string, string>(PropertyUtility.GetLiteral(this, "Code"), Code));
      return RequestParametersList;
    }
  }
}
