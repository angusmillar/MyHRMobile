using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace MyHRMobile.Common.Utility
{
  public static class HttpTools
  {
    public static string GetQueryParameter(string QueryString, string ParameterName)
    {
      NameValueCollection List = System.Web.HttpUtility.ParseQueryString(QueryString);
      string Key = List.AllKeys.SingleOrDefault(x => x.ToLower() == ParameterName.ToLower());
      if (Key != null)
        return List[Key];
      else
        return null;
    }
  }
}
