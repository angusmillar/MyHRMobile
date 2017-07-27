using MyHRMobile.Common.Extentions;
using System;
using System.ComponentModel;

namespace MyHRMobile.FhirGatewayTool.ViewModel
{
  public abstract class ViewModelBase : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    protected void NotifyPropertyChanged(String info)
    {
      PropertyChanged.Raise(this, info);
    }
  }
}
