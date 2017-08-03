using MyHRMobile.FhirGatewayTool.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyHRMobile.FhirGatewayTool.Extensions;

namespace MyHRMobile.FhirGatewayTool.Views
{
  /// <summary>
  /// Interaction logic for DisplayTextEditorView.xaml
  /// </summary>
  public partial class DisplayTextEditorView : UserControl
  {
    Presenter Presenter;
    private ICSharpCode.AvalonEdit.Folding.FoldingManager FoldingManager;
    private ICSharpCode.AvalonEdit.Folding.XmlFoldingStrategy FoldingStrategy;

    public DisplayTextEditorView(Presenter Presenter)
    {
      InitializeComponent();
      this.Presenter = Presenter;
      DataContext = Presenter;


      TextEditorBody.WordWrap = false;
      TextEditorBody.ShowLineNumbers = true;
      TextEditorBody.FontFamily = new FontFamily("Consolas");
      TextEditorBody.FontSize = 12;
      TextEditorBody.LineNumbersForeground = Brushes.DarkGray;
      ExtentionAvalonEdit.AvalonEditContextMenu(TextEditorBody);
      if (!string.IsNullOrWhiteSpace(Presenter.TextEditorViewModel.Text))
      {
        if (Presenter.TextEditorViewModel.FormatType == AvalonEditSyntaxTypes.Xml)
        {
          TextEditorBody.SetSyntaxType(AvalonEditSyntaxTypes.Xml);
          //AvalonEdit Folding
          FoldingManager = ICSharpCode.AvalonEdit.Folding.FoldingManager.Install(TextEditorBody.TextArea);
          FoldingStrategy = new ICSharpCode.AvalonEdit.Folding.XmlFoldingStrategy();
          FoldingStrategy = new ICSharpCode.AvalonEdit.Folding.XmlFoldingStrategy();
          TextEditorBody.Text = MyHRMobile.Common.Utility.XmlTool.BeautifyXML(Presenter.TextEditorViewModel.Text);
          FoldingStrategy.UpdateFoldings(FoldingManager, TextEditorBody.Document);
        }
        else if (Presenter.TextEditorViewModel.FormatType == AvalonEditSyntaxTypes.Json)
        {
          TextEditorBody.SetSyntaxType(AvalonEditSyntaxTypes.Json);
          TextEditorBody.Text = Presenter.TextEditorViewModel.Text;
        }
        else
        {
          TextEditorBody.SetSyntaxType(AvalonEditSyntaxTypes.None);
          TextEditorBody.Text = Presenter.TextEditorViewModel.Text;
        }
      }



    }
  }
}
