using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TrueLayer_NitinJain.Interfaces;

namespace TrueLayer_NitinJain.Services
{
    public class AuthService : IAuthService
    {

        private readonly IOptions<HandlerSettings> _appSettings;
        private readonly ISessionService _sessionService;
        private readonly IHandlerHttpClient _httpClient;

        public AuthService(IOptions<HandlerSettings> appSettings, ISessionService sessionService, IHandlerHttpClient handlerClient)
        {
            _appSettings = appSettings;
            _sessionService = sessionService;
            _httpClient = handlerClient;
        }

        /// <summary>
        /// Builds and returns a string representative of the provider's authorization URI.
        /// </summary>
        public Uri GetAuthorizationUri()
        {
            return new Uri(
                 new Uri(_appSettings.Value.AuthBaseUri) +
                 "?response_type=code&response_mode=form_post" +
                 "&client_id=" + CommonHelper.DecodeBase64ToString(_appSettings.Value.AuthClientId) +
                 "&nonce=12394770744" +
                 "&redirect_uri=" + _appSettings.Value.AuthRedirectUri +
                 "&scope=info accounts balance transactions cards products beneficiaries offline_access" +
                 "&enable_mock=true&enable_oauth_providers=true"
             );
        }

        public HttpResponseMessage ExchangeCodeForAccessToken(string code)
        {
            return FetchTokens(new Dictionary<string, string>(){
                { "code", code },
                { "grant_type", "authorization_code" },
                { "client_id", CommonHelper.DecodeBase64ToString(_appSettings.Value.AuthClientId) },
                { "client_secret", CommonHelper.DecodeBase64ToString(_appSettings.Value.AuthClientSecret) },
                { "redirect_uri", _appSettings.Value.AuthRedirectUri }
            });
        }

        private HttpResponseMessage FetchTokens(Dictionary<string, string> requestBody)
        {
            using (HttpClient client = new HttpClient())
            {
                // Build token endpoint URL.
                string url = new Uri(new Uri(_appSettings.Value.AuthBaseUri), "connect/token").ToString();

                // Set request headers.
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
               
                // Fetch tokens from auth server.
                HttpResponseMessage response = _httpClient.GenerateTokenFromAuthServerPostAsync(url, new FormUrlEncodedContent(requestBody)).Result;

                // Save the access/refresh tokens in the Session.
                _sessionService.SetTokens(response);

                return response;
            }
        }

        private static string Base64Encode(string plainText)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(bytes);
        }
    }
}
