using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHRMobile.ApiV1_0_0_hotfix.ApiSupport
{
  public static class ApiEnum
  {
    public enum RelationshipType
    {
      Self,
      Under18ParentalResponsibility,
      Under18LegalAuthority,
      UnderAge18OtherwiseAppropriatePerson,
      Age18andOverLegalAuthority,
      Age18andOverOtherwiseAppropriatePerson,
      FullAccessNominatedRepresentative,
      NominatedRepresentative
    };

  }
}
