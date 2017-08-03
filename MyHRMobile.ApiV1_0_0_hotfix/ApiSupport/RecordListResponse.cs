using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using MyHRMobile.ApiV1_0_0_hotfix;
using MyHRMobile.ApiV1_0_0_hotfix.ApiSupport;
using MyHRMobile.Common.Extentions;
using Hl7.FhirPath.Parser;
using Hl7.Fhir.FhirPath;
using Hl7.FhirPath;
using Hl7.Fhir.ElementModel;
using Hl7.FhirPath.Functions;
using Hl7.Fhir.Model;
using Hl7.Fhir.Support;

namespace MyHRMobile.ApiV1_0_0_hotfix.ApiSupport
{
  public class RecordListResponse : ApiResponseBase
  {
    public RecordListResponse(HttpStatusCode StatusCode, string Body, FhirApi.FhirFormat Format)
    {
      this.StatusCode = StatusCode;
      this.Format = Format;
      this.Body = Body;
      ApiRelatedPersonList = new List<ApiRelatedPerson>();
      ParseResponseBody();
    }

    public FhirApi.FhirFormat Format { get; set; }
    public string Body { get; set; }
    public List<ApiRelatedPerson> ApiRelatedPersonList { get; set; }

    protected void ParseResponseBody()
    {
      var tpXml = this.Body;
      Bundle Bundle = null;
      if (this.Format == FhirApi.FhirFormat.Xml)
      {
        var parser = new Hl7.Fhir.Serialization.FhirXmlParser();
        Bundle = parser.Parse<Bundle>(tpXml);
      }
      else
      {
        var parser = new Hl7.Fhir.Serialization.FhirJsonParser();
        Bundle = parser.Parse<Bundle>(tpXml);
      }

      IElementNavigator PocoBundel = new PocoNavigator(Bundle);

      foreach (var RelatedPerson in PocoBundel.Select(@"Bundle.entry.select(resource as RelatedPerson)"))
      {
        ApiRelatedPerson ApiRelatedPerson = new ApiRelatedPerson(RelatedPerson);
        ApiRelatedPersonList.Add(ApiRelatedPerson);
      }
    }

  }

  public class ApiRelatedPerson
  {
    public string FhirId { get; set; }
    public string Ihi { get; set; }
    public ApiEnum.RelationshipType RelationshipType { get; set; }
    public string RelationshipDescription { get; set; }
    public string Family { get; set; }
    public string Given { get; set; }
    public AdministrativeGender Gender { get; set; }
    public string Sex { get; set; }
    public DateTime? Dob { get; set; }
    public ApiRelatedPerson() { }
    public ApiRelatedPerson(IElementNavigator RelatedPerson)
    {

      if (RelatedPerson.Select("id").IsAny())
      {
        this.FhirId = RelatedPerson.Select("id").First().Value.ToString();
      }
      if (RelatedPerson.Select("identifier.where(system = 'http://ns.electronichealth.net.au/id/hi/ihi/1.0').value").IsAny())
      {
        this.Ihi = RelatedPerson.Select("identifier.where(system = 'http://ns.electronichealth.net.au/id/hi/ihi/1.0').value").First().Value.ToString();
      }

      if (RelatedPerson.Select("name").IsAny())
      {
        if (RelatedPerson.Select("name.family").IsAny())
        {
          this.Family = RelatedPerson.Select("name.family").First().Value.ToString();
        }

        if (RelatedPerson.Select("name.given").IsAny())
        {
          this.Given = RelatedPerson.Select("name.given").First().Value.ToString();
        }

      }
      if (RelatedPerson.Select("birthDate").IsAny())
      {
        FhirDateTime FhirDate = new FhirDateTime(RelatedPerson.Select("birthDate").First().Value.ToString());
        this.Dob = FhirDate.ToDateTime();
      }

      if (RelatedPerson.Select("birthDate").IsAny())
      {
        this.Sex = RelatedPerson.Select("gender").First().Value.ToString().ToLower();
        switch (this.Sex)
        {
          case "female":
            this.Gender = AdministrativeGender.Female;
            break;
          case "male":
            this.Gender = AdministrativeGender.Male;
            break;
          case "other":
            this.Gender = AdministrativeGender.Other;
            break;
          case "unknown":
            this.Gender = AdministrativeGender.Unknown;
            break;
          default:
            break;
        }
      }

      if (RelatedPerson.Select("relationship.extension.where(url='https://apinams.ehealthvendortest.health.gov.au/fhir/v1.0.0/StructureDefinition/relationship-type').value.as(CodeableConcept).coding.code").IsAny())
      {
        string RelationshipCode = RelatedPerson.Select("relationship.extension.where(url='https://apinams.ehealthvendortest.health.gov.au/fhir/v1.0.0/StructureDefinition/relationship-type').value.as(CodeableConcept).coding.code").First().Value.ToString();
        switch (RelationshipCode)
        {
          case "RT001":
            this.RelationshipType = ApiEnum.RelationshipType.Self;
            this.RelationshipDescription = "Self";
            break;
          case "RT002":
            this.RelationshipType = ApiEnum.RelationshipType.Under18ParentalResponsibility;
            this.RelationshipDescription = "Under 18 - Parental Responsibility";
            break;
          case "RT003":
            this.RelationshipType = ApiEnum.RelationshipType.Under18LegalAuthority;
            this.RelationshipDescription = "Under 18 - Legal Authority";
            break;
          case "RT004":
            this.RelationshipType = ApiEnum.RelationshipType.UnderAge18OtherwiseAppropriatePerson;
            this.RelationshipDescription = "Under 18 - Otherwise Appropriate Person";
            break;
          case "RT005":
            this.RelationshipType = ApiEnum.RelationshipType.Under18LegalAuthority;
            this.RelationshipDescription = "18 and Over - Legal Authority";
            break;
          case "RT006":
            this.RelationshipType = ApiEnum.RelationshipType.Age18andOverOtherwiseAppropriatePerson;
            this.RelationshipDescription = "18 and Over - Otherwise Appropriate Person";
            break;
          case "RT007":
            this.RelationshipType = ApiEnum.RelationshipType.FullAccessNominatedRepresentative;
            this.RelationshipDescription = "Full Access Nominated Representative";
            break;
          case "RT008":
            this.RelationshipType = ApiEnum.RelationshipType.NominatedRepresentative;
            this.RelationshipDescription = "Nominated Representative";
            break;
          default:
            throw new FormatException($"Unknown ApiRelatedPerson.RelationshipType of {this.RelationshipType.ToString()}");
        }
      }
    }
  }
}
