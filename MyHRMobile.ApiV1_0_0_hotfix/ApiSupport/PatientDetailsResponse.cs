using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MyHRMobile.Common.Extentions;
using Hl7.FhirPath.Parser;
using Hl7.Fhir.FhirPath;
using Hl7.FhirPath;
using Hl7.Fhir.ElementModel;
using Hl7.FhirPath.Functions;
using Hl7.Fhir.Model;


namespace MyHRMobile.ApiV1_0_0_hotfix.ApiSupport
{
  public class PatientDetailsResponse : ApiResponseBase
  {
    public PatientDetailsResponse(HttpStatusCode StatusCode, string Body, FhirApi.FhirFormat Format)
    {
      this.StatusCode = StatusCode;
      this.Format = Format;
      this.Body = Body;
      ParseResponseBody();
    }

    public FhirApi.FhirFormat Format { get; set; }
    public string Body { get; set; }
    public ApiPatient ApiPatient { get; set; }
    protected void ParseResponseBody()
    {
      var tpXml = this.Body;
      Patient Patient = null;
      if (this.Format == FhirApi.FhirFormat.Xml)
      {
        var parser = new Hl7.Fhir.Serialization.FhirXmlParser();
        Patient = parser.Parse<Patient>(tpXml);
      }
      else
      {
        var parser = new Hl7.Fhir.Serialization.FhirJsonParser();
        Patient = parser.Parse<Patient>(tpXml);
      }

      IElementNavigator PocoPatient = new PocoNavigator(Patient);
      this.ApiPatient = new ApiPatient(PocoPatient);
    }
  }

  public class ApiPatient : ApiRelatedPerson
  {
    public ApiPatient() : base() { }
    public ApiPatient(IElementNavigator RelatedPerson)
      : base(RelatedPerson)
    {
      if (RelatedPerson.Select("extension.where(url='https://apinams.ehealthvendortest.health.gov.au/fhir/v1.0.0/StructureDefinition/indigenous-status').value.as(Coding).display").IsAny())
      {
        this.IndigenousStatusDescription = RelatedPerson.Select("extension.where(url='https://apinams.ehealthvendortest.health.gov.au/fhir/v1.0.0/StructureDefinition/indigenous-status').value.as(Coding).display").First().Value.ToString();
      }
    }
    public string IndigenousStatusDescription { get; set; }
  }
}
