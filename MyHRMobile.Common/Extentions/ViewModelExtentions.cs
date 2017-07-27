using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHRMobile.Common.Extentions
{
  public static class ViewModelExtentions
  {
    public static void Raise(this PropertyChangedEventHandler handler, object sender, string propertyName)
    {
      if (null != handler)
      {
        handler(sender, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}
