using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyHRMobile.FhirGatewayTool.Extensions
{
  public enum AvalonEditSyntaxTypes { None, Xml, Json };
  public static class ExtentionAvalonEdit
  {
    public static void AvalonEditContextMenu(this TextEditor value)
    {
      var oContextMenu = new ContextMenu();
      oContextMenu.Height = 80;
      oContextMenu.Width = 190;

      MenuItem Copy = new MenuItem();
      Copy.Command = System.Windows.Input.ApplicationCommands.Copy;
      Copy.Header = "_Copy";
      oContextMenu.Items.Add(Copy);

      MenuItem Paste = new MenuItem();
      Paste.Command = System.Windows.Input.ApplicationCommands.Paste;
      Paste.Header = "_Paste";
      oContextMenu.Items.Add(Paste);

      MenuItem SelectAll = new MenuItem();
      SelectAll.Command = System.Windows.Input.ApplicationCommands.SelectAll;
      SelectAll.Header = "_SelectAll";
      oContextMenu.Items.Add(SelectAll);

      value.ContextMenu = oContextMenu;
    }

    public static void SetSyntaxType(this TextEditor value, AvalonEditSyntaxTypes Syntaxtype)
    {

      string StoreDirectoryName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyHRMobile");
      Directory.CreateDirectory(StoreDirectoryName);

      switch (Syntaxtype)
      {
        case AvalonEditSyntaxTypes.Json:
          {
            try
            {
              value.SyntaxHighlighting = ExtentionAvalonEdit.AvalonEditSyntaxHighlighting(Path.Combine(StoreDirectoryName, @"Json.xshd"));
            }
            catch
            {
              value.SyntaxHighlighting = null;
            }

          }
          break;
        case AvalonEditSyntaxTypes.Xml:
          {
            var typeConverter = new ICSharpCode.AvalonEdit.Highlighting.HighlightingDefinitionTypeConverter();
            value.SyntaxHighlighting = (ICSharpCode.AvalonEdit.Highlighting.IHighlightingDefinition)typeConverter.ConvertFrom("XML");
            break;
          }
        case AvalonEditSyntaxTypes.None:
          {
            value.SyntaxHighlighting = null;
            break;
          }
        default:
          throw new NotImplementedException(String.Format("AvalonEditSyntaxHighlightingTypes of '{0}' has not been implemented in the tool as yet.", Syntaxtype.ToString()));
      }
    }

    public static ICSharpCode.AvalonEdit.Highlighting.IHighlightingDefinition AvalonEditSyntaxHighlighting(string value)
    {
      using (System.IO.Stream s = System.IO.File.OpenRead(value))
      {
        using (System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(s))
        {
          return ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load
              (reader, ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
        }
      }
    }

    public static string GetWordUnderMouse(this TextDocument document, TextViewPosition position)
    {
      string wordHovered = string.Empty;

      var line = position.Line;
      var column = position.Column;

      var offset = document.GetOffset(line, column);
      if (offset >= document.TextLength)
        offset--;

      var textAtOffset = document.GetText(offset, 1);

      // Get text backward of the mouse position, until the first space
      while (!string.IsNullOrWhiteSpace(textAtOffset))
      {
        wordHovered = textAtOffset + wordHovered;

        offset--;

        if (offset < 0)
          break;

        textAtOffset = document.GetText(offset, 1);
      }

      // Get text forward the mouse position, until the first space
      offset = document.GetOffset(line, column);
      if (offset < document.TextLength - 1)
      {
        offset++;

        textAtOffset = document.GetText(offset, 1);

        while (!string.IsNullOrWhiteSpace(textAtOffset))
        {
          wordHovered = wordHovered + textAtOffset;

          offset++;

          if (offset >= document.TextLength)
            break;

          textAtOffset = document.GetText(offset, 1);
        }
      }

      return wordHovered;
    }
  }
}
