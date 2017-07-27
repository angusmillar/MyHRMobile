using System;
using Xunit;
using MyHRMobile.API_V1_0_0_hotfix;
using MyHRMobile.API_V1_0_0_hotfix.ApiSupport;


namespace MyHRMobile.Test
{
  public class Test_Api_V1_0_0_hotfix
  {
    [Theory(DisplayName = "GetAccessToken")]
    [InlineData("p6AsqAXxnUMgZiUCfw1pAHWMEnSwlU")]
    public void Test_GetAccessToken(string Code)
    {
      //Arrange
      Uri FhirGatewayEndpoint = new Uri("https://apinams.ehealthvendortest.health.gov.au");

      var TokenRequest = new AccessTokenRequest()
      {
        Client_id = "28198d27-c475-4695-83d3-1f1f8256e01b",
        Client_secret = "1c73a74b-45fc-4ee4-a400-15a398e46143",
        Code = Code
      };

      FhirApi Api = new FhirApi(FhirGatewayEndpoint);

      //Act
      TokenResponse TokenResponse = Api.GetAccessToken(TokenRequest);

      //Assert
      Assert.Equal(TokenResponse.StatusCode, System.Net.HttpStatusCode.OK);
      Assert.NotNull(TokenResponse.RefreshToken);
      Assert.NotNull(TokenResponse.AccessToken);
      Assert.NotNull(TokenResponse.AccessExpires);
      Assert.NotNull(TokenResponse.RefreshExpires);
      Assert.NotNull(TokenResponse.Scope);
      Assert.NotNull(TokenResponse.TokenType);
      Assert.Null(TokenResponse.ErrorResponse);
    }

    [Theory(DisplayName = "GetRefreshToken")]
    [InlineData("qBPfwVXfkFOrNly9qxLuenCgpmsN6chRNnX0WPF0DQMD8i")]
    public void Test_GetRefreshToken(string RefreshToken)
    {
      //Arrange
      Uri FhirGatewayEndpoint = new Uri("https://apinams.ehealthvendortest.health.gov.au");

      var RefreshTokenRequest = new RefreshTokenRequest()
      {
        Client_id = "28198d27-c475-4695-83d3-1f1f8256e01b",
        Client_secret = "1c73a74b-45fc-4ee4-a400-15a398e46143",
        RefreshToken = RefreshToken,
      };

      FhirApi Api = new FhirApi(FhirGatewayEndpoint);

      //Act
      TokenResponse TokenResponse = Api.GetRefreashToken(RefreshTokenRequest);

      //Assert
      Assert.Equal(TokenResponse.StatusCode, System.Net.HttpStatusCode.OK);
      Assert.NotNull(TokenResponse.RefreshToken);
      Assert.NotNull(TokenResponse.AccessToken);
      Assert.NotNull(TokenResponse.AccessExpires);
      Assert.NotNull(TokenResponse.RefreshExpires);
      Assert.NotNull(TokenResponse.Scope);
      Assert.NotNull(TokenResponse.TokenType);
      Assert.Null(TokenResponse.ErrorResponse);
    }


    [Theory(DisplayName = "GetRecordList")]
    [InlineData("qBPfwVXfkFOrNly9qxLuenCgpmsN6chRNnX0WPF0DQMD8i")]
    public void Test_GetRecordList(string RefreshToken)
    {
      //Arrange
      Uri FhirGatewayEndpoint = new Uri("https://apinams.ehealthvendortest.health.gov.au");
      FhirApi Api = new FhirApi(FhirGatewayEndpoint);
      Api.Format = FhirApi.FhirFormat.Xml;
      //Act
      RecordListResponse RecordListResponse = Api.GetRecordList();

      //Assert
      Assert.Equal(RecordListResponse.StatusCode, System.Net.HttpStatusCode.OK);
      Assert.Equal(RecordListResponse.Format, FhirApi.FhirFormat.Xml);
      Assert.NotNull(RecordListResponse.Body);
      Assert.Null(RecordListResponse.ErrorResponse);
    }
  }
}
