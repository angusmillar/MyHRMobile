using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MyHRMobile.API_V1_0_0_hotfix.ApiSupport
{
  public class ErrorResponse
  {
    public ErrorResponse(string Error, string Description)
    {
      this.Error = Error;
      this.Description = Description;
    }
    public ErrorResponse(string Body)
    {
      try
      {
        var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Body);
        this.Error = dict["error"] ?? string.Empty;
        this.Description = dict["error_description"] ?? string.Empty;
      }
      catch (Exception Exec)
      {
        this.Error = Exec.Message;
        this.Description = Body;
      }
    }

    public string Error { get; protected set; }
    public string Description { get; protected set; }
  }
}
