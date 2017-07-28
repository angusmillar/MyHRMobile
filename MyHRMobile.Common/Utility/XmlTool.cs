using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHRMobile.Common.Extentions;
using System.Xml;

namespace MyHRMobile.Common.Utility
{
  public static class XmlTool
  {
    public static string BeautifyXML(string RawXML)
    {
      XmlDocument XmlDocument = new XmlDocument();
      XmlDocument.LoadXml(RawXML);
      return XmlDocument.Beautify();
    }
  }
}
