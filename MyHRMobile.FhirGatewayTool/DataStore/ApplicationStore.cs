using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHRMobile.Common.Extentions;

namespace MyHRMobile.FhirGatewayTool.DataStore
{
  public class ApplicationStore
  {
    public string App_id { get; set; }
    public string App_secret { get; set; }
    public string App_Version { get; set; }
    public List<UserAccount> UserList { get; set; }
  }

  public class UserAccount
  {
    public string Username { get; set; }
    public string AuthorisationCode { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime AccessExpires { get; set; }
    public DateTime RefreshExpires { get; set; }
    public string Scope { get; set; }
  }
}
