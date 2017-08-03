using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHRMobile.Common.Utility
{
  public static class StringTools
  {
    public static string FormatedIHI(string Ihi)
    {
      if (Ihi.Length == 16)
      {
        return $"{Ihi.Substring(0, 4)} {Ihi.Substring(4, 4)} {Ihi.Substring(8, 4)} {Ihi.Substring(12, 4)}";
      }
      else
      {
        return $"{Ihi}";
      }
    }
  }
}
