using System;
using MyHRMobile.Rest;
using System.Net.Http;
using System.Collections.Generic;
using MyHRMobile.ApiV1_0_0_hotfix;
using MyHRMobile.ApiV1_0_0_hotfix.ApiSupport;

namespace MyHR.Runner
{
  class Run
  {
    static void Main(string[] args)
    {
      Console.WriteLine("HTTP Client");

      //MyGov login widget page URL
      //https://apinams.ehealthvendortest.health.gov.au/api/oauth/authorize/login?client_id=28198d27-c475-4695-83d3-1f1f8256e01b&response_type=code&redirect_uri=https://localhost/oauth_callback&scope=https://localhost:8090/pcehr+offline_access

      //MyGov Details
      //User Name: VH429777
      //Password:  Welcome123
      //Secret: 
      //1) What is the name of the first street I lived in? 
      //Answer: street   
      //2) What was the model of the car I learnt to drive in?
      //Answer: car 
      //3) What was my favourite subject at school?
      //Answer: subject 

      Uri FhirGatewayEndpoint = new Uri("https://apinams.ehealthvendortest.health.gov.au");

      var TokenRequest = new AccessTokenRequest()
      {
        Client_id = "28198d27-c475-4695-83d3-1f1f8256e01b",
        Client_secret = "1c73a74b-45fc-4ee4-a400-15a398e46143",
        Code = "Q9O6sy0XTiEI5U8xc0xmVNzKY9j4aZ"

      };

      FhirApi Api = new FhirApi(FhirGatewayEndpoint);
      TokenResponse TokenResponse = Api.GetAccessToken(TokenRequest);
      if (TokenResponse.StatusCode == System.Net.HttpStatusCode.OK)
      {
        Console.WriteLine($"HttpStatus:   {TokenResponse.StatusCode.ToString()}");
        Console.WriteLine($"AccessToken:  {TokenResponse.AccessToken}");
        Console.WriteLine($"RefreshToken: {TokenResponse.RefreshToken}");
        Console.ReadKey();
      }
      else
      {
        Console.WriteLine($"HttpStatus:  {TokenResponse.StatusCode.ToString()}");
        Console.WriteLine($"Error:       {TokenResponse.ErrorResponse.Error}");
        Console.WriteLine($"Description: {TokenResponse.ErrorResponse.Description}");
        Console.ReadKey();
      }
      //Client.ContentType = "application/x-www-form-urlencoded";
      //HttpResponseMessage response;
      //response = Client.PostFormUrlEncodedContent(RequestAccessTokenQuery, RequestParametersList).Result;

      ////Client.Accept = "application/fhir+xml";
      //string test2 = response.Content.ReadAsStringAsync().Result;
      //string StatusCode = response.StatusCode.ToString();
      //Console.WriteLine("Http Status: " + StatusCode);
      //Console.Write(test);
      //Console.ReadKey();


      ////"access_token": "EEMhwH8qjmD23G4bz3itub0np2mXXMtnHtHiDhLGDdmSXQQ20HE58v",
      //"token_type": "Bearer",
      //"expires_in": "15552000",
      //"refresh_token": "cUOlXUQuG7dkBPJBxDubnCah61xT3apsXYXGhHG7ahRwbe",
      //"scope": "https://localhost:8090/pcehr"


      //GET Example
      //HttpResponseMessage response;
      //response = Client.Get("]/api/oauth/token").Result;
      //Client.ContentType = "application/x-www-form-urlencoded";
      //Client.Accept = "application/fhir+xml";
      //string test = response.Content.ReadAsStringAsync().Result;
      //string StatusCode = response.StatusCode.ToString();
      //Console.WriteLine("Http Status: " + StatusCode);
      //Console.Write(test);
      //Console.ReadKey();
    }
  }
}