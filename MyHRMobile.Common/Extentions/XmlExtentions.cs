using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MyHRMobile.Common.Extentions
{
  public static class XmlExtentions
  {
    static public string Beautify(this XmlDocument doc)
    {
      StringBuilder sb = new StringBuilder();
      XmlWriterSettings settings = new XmlWriterSettings
      {
        Indent = true,
        IndentChars = "  ",
        NewLineChars = "\r\n",
        NewLineHandling = NewLineHandling.Replace
      };
      using (XmlWriter writer = XmlWriter.Create(sb, settings))
      {
        doc.Save(writer);
      }
      return sb.ToString();
    }

  }
}
