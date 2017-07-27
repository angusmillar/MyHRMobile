using System;

namespace MyHRMobile.Common.Attributes
{
  [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
  public sealed class PropertyLiteralAttribute : Attribute
  {
    readonly string literal;

    // This is a positional argument
    public PropertyLiteralAttribute(string literal)
    {
      this.literal = literal;
    }

    public string Literal
    {
      get { return literal; }
    }
  }

}
