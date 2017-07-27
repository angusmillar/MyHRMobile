using System;
using System.Collections.Generic;
using System.Text;
using MyHRMobile.Common.Attributes;
using System.Reflection;
using System.Collections;
using System.Linq;

namespace MyHRMobile.Common.Utility
{
  public static class PropertyUtility
  {
    public static string GetLiteral<T>(T item, string PropertyName) where T : new()
    {
      // Get the PropertyInfo object:
      var properties = item.GetType().GetProperties();
      var getit = properties.FirstOrDefault(x => x.Name == PropertyName);
      var attributes = getit.GetCustomAttributes(false);
      var columnMapping = attributes.FirstOrDefault(a => a.GetType() == typeof(PropertyLiteralAttribute));
      if (columnMapping != null)
      {
        var mapsto = columnMapping as PropertyLiteralAttribute;
        return mapsto.Literal;
      }
      return null;
    }
  }
}
