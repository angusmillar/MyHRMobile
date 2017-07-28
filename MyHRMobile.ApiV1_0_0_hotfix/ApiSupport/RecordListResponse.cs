using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using MyHRMobile.API_V1_0_0_hotfix;
using MyHRMobile.ApiV1_0_0_hotfix.ApiSupport;
using Hl7.FhirPath.Parser;
using Hl7.Fhir.FhirPath;
using Hl7.FhirPath;
using Hl7.Fhir.ElementModel;
using Hl7.FhirPath.Functions;
using Hl7.Fhir.Model;


namespace MyHRMobile.API_V1_0_0_hotfix.ApiSupport
{


  public class RecordListResponse : ApiResponseBase
  {
    public RecordListResponse(HttpStatusCode StatusCode, string Body, FhirApi.FhirFormat Format)
    {
      this.StatusCode = StatusCode;
      this.Format = Format;
      this.Body = Body;
      Parse();
    }

    public FhirApi.FhirFormat Format { get; set; }
    public string Body { get; set; }
    public List<ApiRelatedPerson> ApiRelatedPersonList { get; set; }

    private void Parse()
    {
      ApiRelatedPersonList = new List<ApiRelatedPerson>();



      var parser = new Hl7.Fhir.Serialization.FhirXmlParser();
      var tpXml = this.Body;

      var MyBundle = parser.Parse<Bundle>(tpXml);
      IElementNavigator PocoBundel = new PocoNavigator(MyBundle);

      var test = PocoBundel.Name;

      //var test2 = PocoBundel.
      var test2 = PocoBundel.Select(@"Bundle.type");
      foreach (var RelatedPerson in PocoBundel.Select(@"Bundle.entry.select(resource as RelatedPerson)"))
      {

        var ApiRelatedPerson = new ApiRelatedPerson();
        ApiRelatedPerson.FhirId = RelatedPerson.Select("id.single().toString()").First().Value.ToString();
        ApiRelatedPerson.Ihi = RelatedPerson.Select("identifier.where(system = 'http://ns.electronichealth.net.au/id/hi/ihi/1.0').value").First().Value.ToString();
        ApiRelatedPerson.Family = RelatedPerson.Select("name.family").First().Value.ToString();
        ApiRelatedPerson.Given = RelatedPerson.Select("name.given").First().Value.ToString();
        ApiRelatedPerson.Dob = new FhirDateTime(RelatedPerson.Select("birthDate").First().Value.ToString());
        string gen = RelatedPerson.Select("gender").First().Value.ToString().ToLower();
        switch (gen)
        {
          case "female":
            ApiRelatedPerson.Gender = AdministrativeGender.Female;
            break;
          case "male":
            ApiRelatedPerson.Gender = AdministrativeGender.Male;
            break;
          case "other":
            ApiRelatedPerson.Gender = AdministrativeGender.Other;
            break;
          case "unknown":
            ApiRelatedPerson.Gender = AdministrativeGender.Unknown;
            break;
          default:
            break;
        }
        string RelationshipCode = RelatedPerson.Select("relationship.extension.where(url='https://apinams.ehealthvendortest.health.gov.au/fhir/v1.0.0/StructureDefinition/relationship-type').value.as(CodeableConcept).coding.code").First().Value.ToString();
        switch (RelationshipCode)
        {
          case "RT001":
            ApiRelatedPerson.RelationshipType = ApiEnum.RelationshipType.Self;
            break;
          case "RT002":
            ApiRelatedPerson.RelationshipType = ApiEnum.RelationshipType.Under18ParentalResponsibility;
            break;
          case "RT003":
            ApiRelatedPerson.RelationshipType = ApiEnum.RelationshipType.Under18LegalAuthority;
            break;
          case "RT004":
            ApiRelatedPerson.RelationshipType = ApiEnum.RelationshipType.UnderAge18OtherwiseAppropriatePerson;
            break;
          case "RT005":
            ApiRelatedPerson.RelationshipType = ApiEnum.RelationshipType.Under18LegalAuthority;
            break;
          case "RT006":
            ApiRelatedPerson.RelationshipType = ApiEnum.RelationshipType.Age18andOverOtherwiseAppropriatePerson;
            break;
          case "RT007":
            ApiRelatedPerson.RelationshipType = ApiEnum.RelationshipType.FullAccessNominatedRepresentative;
            break;
          case "RT008":
            ApiRelatedPerson.RelationshipType = ApiEnum.RelationshipType.NominatedRepresentative;
            break;
          default:
            throw new FormatException($"Unknown ApiRelatedPerson.RelationshipType of {ApiRelatedPerson.RelationshipType.ToString()}");
        }
        ApiRelatedPersonList.Add(ApiRelatedPerson);
      }
    }
  }

  public class ApiRelatedPerson
  {
    public string FhirId { get; set; }
    public string Ihi { get; set; }
    public ApiEnum.RelationshipType RelationshipType { get; set; }
    public string Family { get; set; }
    public string Given { get; set; }
    public AdministrativeGender Gender { get; set; }
    public FhirDateTime Dob { get; set; }
  }
}
