using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHRMobile.Common.Extentions
{
  public static class FhirPathExtentions
  {
    public static bool IsAny<T>(this IEnumerable<T> data)
    {
      return data != null && data.Any();
    }

  }
}
