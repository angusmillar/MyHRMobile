using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace MyHRMobile.ApiV1_0_0_hotfix.ApiSupport
{
  public class TokenResponse : ApiResponseBase
  {
    private readonly int RefreshTokenExpiryMonthsPeriod = 24;
    public TokenResponse(HttpStatusCode StatusCode, string Body)
    {
      this.StatusCode = StatusCode;
      if (StatusCode == HttpStatusCode.OK)
      {
        try
        {
          var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Body);
          this.AccessToken = dict["access_token"] ?? string.Empty;
          this.TokenType = dict["token_type"] ?? string.Empty;
          this.RefreshToken = dict["refresh_token"] ?? string.Empty;
          this.Scope = dict["scope"] ?? string.Empty;
          this.RefreshExpires = DateTime.Now.AddMonths(RefreshTokenExpiryMonthsPeriod);

          string ExpiresIn = dict["expires_in"] ?? string.Empty;
          if (!string.IsNullOrWhiteSpace(ExpiresIn))
          {
            int TempMinInteger = 0;
            if (int.TryParse(ExpiresIn, out TempMinInteger))
            {
              DateTime Now = DateTime.Now;
              this.AccessExpires = Now.AddSeconds(TempMinInteger);
            }
            else
            {
              throw new FormatException($"expires_in returned a non integer value of {ExpiresIn}");
            }
          }
        }
        catch (Exception Exec)
        {
          this.ErrorResponse = new ErrorResponse(Exec.Message, Body);
        }
      }
      else
      {
        this.ErrorResponse = new ErrorResponse(Body);
      }
    }

    public string AccessToken { get; private set; }
    public string TokenType { get; private set; }
    public DateTime AccessExpires { get; private set; }
    public DateTime RefreshExpires { get; private set; }
    public string RefreshToken { get; private set; }
    public string Scope { get; private set; }
  }
}
