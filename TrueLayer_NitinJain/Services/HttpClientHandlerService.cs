using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TrueLayer_NitinJain.Interfaces;

namespace TrueLayer_NitinJain.Services
{
    public class HttpClientHandlerService : IHandlerHttpClient
    {
        private Uri _apiBaseUri;
        private readonly ISessionService _sessionService;
        private readonly IOptions<HandlerSettings> _appSettings;

        public HttpClientHandlerService(IOptions<HandlerSettings> appSettings, ISessionService sessionService)
        {
            _sessionService = sessionService;
            _appSettings = appSettings;

            _apiBaseUri = new Uri(new Uri(_appSettings.Value.ApiBaseUri), "data/v1/");
        }

        public async Task<HttpResponseMessage> GenerateTokenFromAuthServerPostAsync(string uri, HttpContent content)
        {
            var client = new HttpClient();

            using (client)
            {
                return await client.PostAsync(uri, content);
            }
        }


        public async Task<HttpResponseMessage> GetDataFromTrueLayer(string method, string uri, HttpContent content = null)
        {
            HttpResponseMessage response;

            using (HttpClient client = new HttpClient())
            {
                string token = _sessionService.GetAccessToken();


                if (token != null) // token not exist
                {
                    // Set TrueLayer endpoint.
                    client.BaseAddress = _apiBaseUri;

                    // Set request headers.
                    //client.DefaultRequestHeaders.Add("bb-api-subscription-key", _appSettings.Value.AuthSubscriptionKey);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);

                    // Make the request to TrueLayer API.
                    switch (method.ToLower())
                    {
                        default:
                        case "get":
                            response = await client.GetAsync(uri);
                            break;

                        case "post":
                            response = await client.PostAsync(uri, content);
                            break;
                    }

                    return response;
                }

                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            }

        }
    }
}
