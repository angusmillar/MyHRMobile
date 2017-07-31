using System;
using System.Collections.Generic;
using System.Text;

namespace MyHRMobile.ApiV1_0_0_hotfix.ApiSupport
{
  public class ApiRequestHeader
  {
    public ApiRequestHeader(string Authorization, string AppId, string AppVersion)
    {
      this.Authorization = Authorization;
      this.AppId = AppId;
      this.AppVersion = AppVersion;
    }

    public string Authorization { get; }
    public string AppId { get; }
    public string AppVersion { get; }
  }
}
