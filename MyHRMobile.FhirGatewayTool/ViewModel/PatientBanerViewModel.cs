using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyHRMobile.FhirGatewayTool.ViewModel
{
  public class PatientBanerViewModel : ViewModelBase
  {
    public PatientBanerViewModel()
    {
    }

    private string _Name { get; set; }
    public string Name
    {
      get
      {
        return $"{this.Family.ToUpper()}, {this.Given}";
      }
    }

    private string _Family { get; set; }
    public string Family
    {
      get
      {
        return _Family;
      }
      set
      {
        _Family = value;
        NotifyPropertyChanged("Family");
        NotifyPropertyChanged("Name");
      }
    }

    private string _Given { get; set; }
    public string Given
    {
      get
      {
        return _Given;
      }
      set
      {
        _Given = value;
        NotifyPropertyChanged("Given");
        NotifyPropertyChanged("Name");
      }
    }

    private string _Dob { get; set; }
    public string Dob
    {
      get
      {
        return _Dob;
      }
      set
      {
        _Dob = value;
        NotifyPropertyChanged("Dob");
      }
    }

    private string _Sex { get; set; }
    public string Sex
    {
      get
      {
        return _Sex;
      }
      set
      {
        _Sex = value;
        if (_Sex.ToLower() == "male")
          this.GenderBrush = Brushes.LightBlue;
        else if (_Sex.ToLower() == "female")
          this.GenderBrush = Brushes.LightPink;
        else
          this.GenderBrush = Brushes.LightYellow;
        NotifyPropertyChanged("Sex");
      }
    }

    private string _Ihi { get; set; }
    public string Ihi
    {
      get
      {
        return MyHRMobile.Common.Utility.StringTools.FormatedIHI(_Ihi);
      }
      set
      {
        _Ihi = value;
        NotifyPropertyChanged("Ihi");
      }
    }

    private Brush _GenderBrush { get; set; }
    public Brush GenderBrush
    {
      get
      {
        return _GenderBrush;
      }
      set
      {
        _GenderBrush = value;
        NotifyPropertyChanged("GenderBrush");
      }
    }

    private string _IndigenousStatus { get; set; }
    public string IndigenousStatus
    {
      get
      {
        return _IndigenousStatus;
      }
      set
      {
        _IndigenousStatus = value;
        NotifyPropertyChanged("IndigenousStatus");
      }
    }

  }
}
