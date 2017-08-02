using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MyHRMobile.FhirGatewayTool.ViewModel
{
  public class MyGovAuthenticationOutcomeViewModel : ViewModelBase
  {
    public MyGovAuthenticationOutcomeViewModel()
    {
      this.SuccessInfoMessage = $"You are now able to select you Account name from the drop down on the left.\nYour account will be remembered and authentication managed. ";
    }
    private bool _OutcomeState { get; set; }
    public bool OutcomeState
    {
      get
      {
        return _OutcomeState;
      }
      set
      {
        _OutcomeState = value;
        if (_OutcomeState)
        {
          this._OutcomeColor = Brushes.Green;
          this.OutcomeMessage = $"You MyGov authentication was successful.";
          this.SuccessVisibility = Visibility.Visible;
          this.UnSuccessfulVisibility = Visibility.Hidden;
        }
        else
        {
          this._OutcomeColor = Brushes.Maroon;
          this.OutcomeMessage = $"You MyGov authentication was unsuccessful.";
          this.SuccessVisibility = Visibility.Hidden;
          this.UnSuccessfulVisibility = Visibility.Visible;
        }
        NotifyPropertyChanged("OutcomeState");
      }
    }

    private Visibility _SuccessVisibility { get; set; }
    public Visibility SuccessVisibility
    {
      get
      {
        return _SuccessVisibility;
      }
      set
      {
        _SuccessVisibility = value;
        NotifyPropertyChanged("SuccessVisibility");
      }
    }

    private Visibility _UnSuccessfulVisibility { get; set; }
    public Visibility UnSuccessfulVisibility
    {
      get
      {
        return _UnSuccessfulVisibility;
      }
      set
      {
        _UnSuccessfulVisibility = value;
        NotifyPropertyChanged("UnSuccessfulVisibility");
      }
    }

    private Brush _OutcomeColor { get; set; }
    public Brush OutcomeColor
    {
      get
      {
        return _OutcomeColor;
      }
      set
      {
        _OutcomeColor = value;
        NotifyPropertyChanged("OutcomeColor");
      }
    }

    private string _OutcomeMessage { get; set; }
    public string OutcomeMessage
    {
      get
      {
        return _OutcomeMessage;
      }
      set
      {
        _OutcomeMessage = value;
        NotifyPropertyChanged("OutcomeMessage");
      }
    }

    private string _SuccessInfoMessage { get; set; }
    public string SuccessInfoMessage
    {
      get
      {
        return _SuccessInfoMessage;
      }
      set
      {
        _SuccessInfoMessage = value;
        NotifyPropertyChanged("SuccessInfoMessage");
      }
    }

  }
}
