using System;
using System.Collections.Generic;
using System.Text;
using MyHRMobile.Rest;
using System.Net.Http;
using MyHRMobile.ApiV1_0_0_hotfix.ApiSupport;

namespace MyHRMobile.ApiV1_0_0_hotfix
{
  public class FhirApi
  {
    public enum FhirFormat { Json, Xml };
    private Client _Client;
    public FhirApi(Uri ServiceEndpoint)
    {
      this.ServiceEndpoint = ServiceEndpoint;
      this.Format = FhirFormat.Xml;
    }

    public ApiRequestHeader ApiRequestHeader { get; set; }
    public Uri ServiceEndpoint { get; set; }
    public FhirFormat Format { get; set; }

    private string GetFormatString(FhirFormat format)
    {
      switch (format)
      {
        case FhirFormat.Json:
          return "application/json+fhir";
        case FhirFormat.Xml:
          return "application/xml+fhir";
        default:
          throw new FormatException($"{format.ToString()}");
      }

    }

    public TokenResponse GetAccessToken(AccessTokenRequest AccessTokenRequest)
    {
      string RequestAccessTokenQuery = "api/oauth/token";
      _Client = new Client();
      _Client.Endpoint = ServiceEndpoint.OriginalString;
      _Client.ContentType = "application/x-www-form-urlencoded";
      HttpResponseMessage response = _Client.PostFormUrlEncodedContent(RequestAccessTokenQuery, AccessTokenRequest.GetRequestParametersList()).Result;
      return new TokenResponse(response.StatusCode, response.Content.ReadAsStringAsync().Result);
    }

    public TokenResponse GetRefreshToken(RefreshTokenRequest RefreshTokenRequest)
    {
      string RequestAccessTokenQuery = "api/oauth/token";
      _Client = new Client();
      _Client.Endpoint = ServiceEndpoint.OriginalString;
      _Client.ContentType = "application/x-www-form-urlencoded";

      HttpResponseMessage response = _Client.PostFormUrlEncodedContent(RequestAccessTokenQuery, RefreshTokenRequest.GetRequestParametersList(), true).Result;
      return new TokenResponse(response.StatusCode, response.Content.ReadAsStringAsync().Result);
    }

    public RecordListResponse GetRecordList(string Ihi = "")
    {
      if (ApiRequestHeader == null)
        throw new NullReferenceException("ApiRequestHeader can not be null");

      string GetRecordListQuery = "fhir/v1.0.0/RelatedPerson";
      if (!string.IsNullOrEmpty(Ihi))
        GetRecordListQuery = $"{GetRecordListQuery}/{Ihi}?_format={GetFormatString(Format)}";
      else
        GetRecordListQuery = $"{GetRecordListQuery}?_format={GetFormatString(Format)}";

      _Client = new Client();
      _Client.Endpoint = ServiceEndpoint.OriginalString;
      HttpResponseMessage response = _Client.Get(GetRecordListQuery, this.ApiRequestHeader.Authorization, this.ApiRequestHeader.AppId, this.ApiRequestHeader.AppVersion).Result;
      return new RecordListResponse(response.StatusCode, response.Content.ReadAsStringAsync().Result, Format);
    }

    public PatientDetailsResponse GetPatientDetails(string Ihi = "")
    {
      if (ApiRequestHeader == null)
        throw new NullReferenceException("ApiRequestHeader can not be null");

      string GetRecordListQuery = "fhir/v1.0.0/Patient";
      if (!string.IsNullOrEmpty(Ihi))
      {
        //Dumb, not ?_id=ihi but rather Patient/ihi?_format=
        GetRecordListQuery = $"{GetRecordListQuery}/{Ihi}?_format={GetFormatString(Format)}";
      }
      else
      {
        GetRecordListQuery = $"{GetRecordListQuery}?_format={GetFormatString(Format)}";
      }

      _Client = new Client();
      _Client.Endpoint = ServiceEndpoint.OriginalString;
      HttpResponseMessage response = _Client.Get(GetRecordListQuery, this.ApiRequestHeader.Authorization, this.ApiRequestHeader.AppId, this.ApiRequestHeader.AppVersion).Result;
      return new PatientDetailsResponse(response.StatusCode, response.Content.ReadAsStringAsync().Result, Format);
    }
  }
}
