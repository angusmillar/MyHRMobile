using MyHRMobile.FhirGatewayTool.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHRMobile.FhirGatewayTool.ViewModel
{
  public class TextEditorViewModel : ViewModelBase
  {
    public TextEditorViewModel()
    {
      _Text = "No Content";
    }

    private string _Text { get; set; }
    public string Text
    {
      get
      {
        return _Text;
      }
      set
      {
        _Text = value;
        NotifyPropertyChanged("Text");
      }
    }

    private AvalonEditSyntaxTypes _FormatType { get; set; }
    public AvalonEditSyntaxTypes FormatType
    {
      get
      {
        return _FormatType;
      }
      set
      {
        _FormatType = value;
        NotifyPropertyChanged("FormatType");
      }
    }
  }
}
