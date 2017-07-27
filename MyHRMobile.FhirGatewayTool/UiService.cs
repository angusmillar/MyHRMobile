using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHRMobile.FhirGatewayTool.DataStore;
using System.IO;
using MyHRMobile.API_V1_0_0_hotfix;
using MyHRMobile.API_V1_0_0_hotfix.ApiSupport;

namespace MyHRMobile.FhirGatewayTool
{
  public class UiService
  {
    private readonly string ApplicationStoreFile = "AccountData.xml";
    private string ApplicationStoreFilePath = "";
    private Uri FhirGatewayEndpoint = new Uri("https://apinams.ehealthvendortest.health.gov.au");

    public UiService()
    {
      var StoreDirectoryName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyHRMobile");
      Directory.CreateDirectory(StoreDirectoryName);
      ApplicationStoreFilePath = Path.Combine(StoreDirectoryName, ApplicationStoreFile);
    }

    public ApplicationStore ApplicationStore { get; protected set; }
    public UserAccount CurrectUserAccount { get; set; }
    public string ErrorMessage { get; protected set; }

    public bool GetAccessToken()
    {
      if (!string.IsNullOrWhiteSpace(CurrectUserAccount.AuthorisationCode))
      {
        FhirApi FhirApi = new FhirApi(FhirGatewayEndpoint);
        var AccessTokenRequest = new AccessTokenRequest();
        AccessTokenRequest.Client_id = ApplicationStore.Client_id;
        AccessTokenRequest.Client_secret = ApplicationStore.Client_secret;
        AccessTokenRequest.Code = CurrectUserAccount.AuthorisationCode;

        TokenResponse TokenResponse = FhirApi.GetAccessToken(AccessTokenRequest);
        if (TokenResponse.StatusCode == System.Net.HttpStatusCode.OK)
        {
          this.CurrectUserAccount.AccessExpires = TokenResponse.AccessExpires;
          this.CurrectUserAccount.AccessToken = TokenResponse.AccessToken;
          this.CurrectUserAccount.RefreshExpires = TokenResponse.RefreshExpires;
          this.CurrectUserAccount.RefreshToken = TokenResponse.RefreshToken;
          this.CurrectUserAccount.Scope = TokenResponse.Scope;
          if (ApplicationStore.UserList.SingleOrDefault(x => x.Username == CurrectUserAccount.Username) == null)
          {
            ApplicationStore.UserList.Add(this.CurrectUserAccount);
            SaveApplicationStore();
          }
          return true;
        }
        else
        {
          if (TokenResponse.ErrorResponse != null)
            this.ErrorMessage = $"Access Token request failed with message: Http Status: {TokenResponse.StatusCode.ToString()}, Error: {TokenResponse.ErrorResponse.Error}, ErrorMessage: {TokenResponse.ErrorResponse.Description}";
          else
            this.ErrorMessage = $"Access Token request failed with no error message returned message, http status was: {TokenResponse.StatusCode.ToString()}";
          return false;
        }
      }
      else
      {
        throw new FormatException($"No CurrectUserAccount.AuthorisationCode found for access token request.");
      }
    }

    public void UpdateView(ViewModel.Presenter Presenter)
    {
      Presenter.Client_id = ApplicationStore.Client_id;
      Presenter.Client_secret = ApplicationStore.Client_secret;
      //Presenter.UserAccountViewList.Clear();
      foreach (var x in this.ApplicationStore.UserList)
      {
        var User = new ViewModel.UserAccountView()
        {
          AccessExpires = x.AccessExpires,
          AccessToken = x.AccessToken,
          AuthorisationCode = x.AuthorisationCode,
          RefreshExpires = x.RefreshExpires,
          RefreshToken = x.RefreshToken,
          Scope = x.Scope,
          Username = x.Username
        };
        Presenter.AddUserAccountViewList.Add(User);
      }
      if (Presenter.UserAccountViewList.Count > 0)
        Presenter.CurrentUserAccount = Presenter.UserAccountViewList[0];
    }
    public void SetCurrentUserAcccount(ViewModel.UserAccountView UserAccountView)
    {
      this.CurrectUserAccount = new UserAccount();
      this.CurrectUserAccount.AccessExpires = UserAccountView.AccessExpires;
      this.CurrectUserAccount.AccessToken = UserAccountView.AccessToken;
      this.CurrectUserAccount.AuthorisationCode = UserAccountView.AuthorisationCode;
      this.CurrectUserAccount.RefreshExpires = UserAccountView.RefreshExpires;
      this.CurrectUserAccount.RefreshToken = UserAccountView.RefreshToken;
      this.CurrectUserAccount.Scope = UserAccountView.Scope;
      this.CurrectUserAccount.Username = UserAccountView.Username;
    }

    public void ResetApplicationStore()
    {
      File.Delete(ApplicationStoreFilePath);
      PrimeApplicationStore();
    }
    public void PrimeApplicationStore()
    {
      if (!File.Exists(ApplicationStoreFilePath))
      {
        ApplicationStore AppStore = new ApplicationStore();
        AppStore.Client_id = "28198d27-c475-4695-83d3-1f1f8256e01b";
        AppStore.Client_secret = "1c73a74b-45fc-4ee4-a400-15a398e46143";
        AppStore.UserList = new List<UserAccount>();
        try
        {
          XmlStoreHelper.ToXmlFile(AppStore, ApplicationStoreFilePath);
        }
        catch (Exception exec)
        {
          throw new FieldAccessException($"Unable to create file on prime at {ApplicationStoreFilePath} ", exec);
        }
      }
    }
    public void LoadApplicationStore()
    {
      ApplicationStore = XmlStoreHelper.FromXmlFile<ApplicationStore>(ApplicationStoreFilePath);
    }
    public void SaveApplicationStore()
    {
      try
      {
        XmlStoreHelper.ToXmlFile(ApplicationStore, ApplicationStoreFilePath);
      }
      catch (Exception Exec)
      {
        throw new FieldAccessException($"Unable to update file on save at {ApplicationStoreFilePath} ", Exec);
      }
    }

  }
}
