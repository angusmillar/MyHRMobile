using System;
using System.Collections.Generic;
using MyHRMobile.Common.Attributes;
using MyHRMobile.Common.Utility;

namespace MyHRMobile.API_V1_0_0_hotfix.ApiSupport
{
  public class RefreshTokenRequest : TokenRequestBase
  {
    public RefreshTokenRequest()
    {
      this.Grant_type = "refresh_token";
    }

    [PropertyLiteral("refresh_token")]
    public string RefreshToken { get; set; }

    public List<KeyValuePair<string, string>> GetRequestParametersList()
    {
      var RequestParametersList = new List<KeyValuePair<string, string>>();
      RequestParametersList.Add(new KeyValuePair<string, string>(PropertyUtility.GetLiteral(this, "Client_id"), Client_id));
      RequestParametersList.Add(new KeyValuePair<string, string>(PropertyUtility.GetLiteral(this, "Client_secret"), Client_secret));
      RequestParametersList.Add(new KeyValuePair<string, string>(PropertyUtility.GetLiteral(this, "Grant_type"), Grant_type));
      RequestParametersList.Add(new KeyValuePair<string, string>(PropertyUtility.GetLiteral(this, "Format"), Format));
      RequestParametersList.Add(new KeyValuePair<string, string>(PropertyUtility.GetLiteral(this, "RefreshToken"), RefreshToken));
      return RequestParametersList;
    }
  }
}
