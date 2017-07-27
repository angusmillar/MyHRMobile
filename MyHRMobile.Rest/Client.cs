using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace MyHRMobile.Rest
{
  public class Client
  {
    private string _Endpoint;
    public string Endpoint
    {
      get
      {
        return _Endpoint;
      }

      set
      {
        if (value.EndsWith("/"))
          _Endpoint = value.Substring(0, value.Length - 1);
        else
          _Endpoint = value;
      }
    }
    public string Accept { get; set; }
    public string ContentType { get; set; }


    public async Task<HttpResponseMessage> Get(string Query, string Authorization, string AppId, string AppVersion)
    {
      Query = PrepairQuery(Query);
      var client = new HttpClient();
      client.DefaultRequestHeaders.Accept.Clear();
      if (!string.IsNullOrWhiteSpace(Accept))
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Accept));
      if (!string.IsNullOrWhiteSpace(Authorization))
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Authorization);
      if (!string.IsNullOrWhiteSpace(AppId))
        client.DefaultRequestHeaders.Add("App-Id", AppId);
      if (!string.IsNullOrWhiteSpace(AppVersion))
        client.DefaultRequestHeaders.Add("App-Version", AppVersion);

      var stringTask = client.GetAsync($"{Endpoint}{Query}").ConfigureAwait(false);
      return await stringTask;
    }

    public async Task<HttpResponseMessage> PostFormUrlEncodedContent(string Query, List<KeyValuePair<string, string>> ParameterList, bool SendAuthorization = false)
    {
      Query = PrepairQuery(Query);
      var client = new HttpClient();
      client.DefaultRequestHeaders.Accept.Clear();
      if (!string.IsNullOrWhiteSpace(Accept))
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Accept));
      if (SendAuthorization)
      {
        string Auth = $"{ParameterList.Single(x => x.Key == "client_id").Value}:{ParameterList.Single(x => x.Key == "client_secret").Value}";
        byte[] EncodedByte = System.Text.ASCIIEncoding.ASCII.GetBytes(Auth);
        string Base64EncodedAuthorization = Convert.ToBase64String(EncodedByte);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Base64EncodedAuthorization);
      }
      var DataContent = new FormUrlEncodedContent(ParameterList);
      var Response = client.PostAsync($"{Endpoint}{Query}", DataContent).ConfigureAwait(false);
      return await Response;
    }

    public async Task<HttpResponseMessage> Post(string Query, string Data)
    {
      Query = PrepairQuery(Query);
      var client = new HttpClient();
      client.DefaultRequestHeaders.Accept.Clear();
      if (!string.IsNullOrWhiteSpace(Accept))
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Accept));
      if (!string.IsNullOrWhiteSpace(ContentType))
        client.DefaultRequestHeaders.Add("Content-Type", ContentType);
      var DataContent = new StringContent(Data);
      var Response = client.PostAsync($"{Endpoint}{Query}", DataContent).ConfigureAwait(false);
      return await Response;
    }

    public async Task<HttpResponseMessage> Put(string Query, string Data)
    {
      Query = PrepairQuery(Query);
      var client = new HttpClient();
      client.DefaultRequestHeaders.Accept.Clear();
      if (!string.IsNullOrWhiteSpace(Accept))
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Accept));
      if (!string.IsNullOrWhiteSpace(ContentType))
        client.DefaultRequestHeaders.Add("Content-Type", ContentType);
      var DataContent = new StringContent(Data);
      var Response = client.PutAsync($"{Endpoint}{Query}", DataContent).ConfigureAwait(false);
      return await Response;
    }

    public async Task<HttpResponseMessage> Delete(string Query)
    {
      Query = PrepairQuery(Query);
      var client = new HttpClient();
      client.DefaultRequestHeaders.Accept.Clear();
      if (!string.IsNullOrWhiteSpace(Accept))
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Accept));
      if (!string.IsNullOrWhiteSpace(ContentType))
        client.DefaultRequestHeaders.Add("Content-Type", ContentType);
      var Response = client.DeleteAsync($"{Endpoint}/{Query}").ConfigureAwait(false);
      return await Response;
    }

    private string PrepairQuery(string Query)
    {
      if (string.IsNullOrWhiteSpace(Query))
        throw new NullReferenceException("Query can not be null");
      if (!Query.StartsWith("/"))
        Query = $"/{Query}";
      return Query;
    }

  }
}
