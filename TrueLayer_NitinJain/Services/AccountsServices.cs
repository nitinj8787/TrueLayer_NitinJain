using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using TrueLayer_NitinJain.Interfaces;
using TrueLayer_NitinJain.Model;

namespace TrueLayer_NitinJain.Services
{
    public class AccountsServices : IAccountsService
    {
        private readonly IOptions<HandlerSettings> _appSettings;

        private readonly IAuthService _authService;
        private readonly IHandlerHttpClient _httpClient;

        public AccountsServices(IOptions<HandlerSettings> appSettings, ISessionService sessionService, IAuthService authService, IHandlerHttpClient handlerClient)
        {
            _appSettings = appSettings;
            _authService = authService;
            _httpClient = handlerClient;
        }

        public HttpResponseMessage GetAccount(string id)
        {
            // Make the request.
            HttpResponseMessage response = _httpClient.GetDataFromTrueLayer("get", "accounts/" + id).Result;

            // Handle bad response.
            if (!response.IsSuccessStatusCode)
            {
                int statusCode = (int)response.StatusCode;
                switch (statusCode)
                {
                    // to do.. check all status code and return response content that make sense to api consumer
                    case 400:
                        response.Content = new StringContent("{ error: \"please contact True layer.\" }");
                        break;
                }
            }

            return response;
        }

        public HttpResponseMessage GetAccounts()
        {
            return _httpClient.GetDataFromTrueLayer("get", "accounts/").Result;
        }

        public HttpResponseMessage GetTransactionsOnAccount(string id)
        {
            // Make the request.
            HttpResponseMessage response = _httpClient.GetDataFromTrueLayer("get", "accounts/" + id + "/transactions/").Result;

            return response;
        }

        public async Task<TransactionDetailResponse> GetTransactionsMinMaxDetails(string id)
        {
            // Make the request.
            HttpResponseMessage response = _httpClient.GetDataFromTrueLayer("get", "accounts/" + id + "/transactions/").Result;

            if (response.StatusCode == HttpStatusCode.BadRequest)
                return null;

            // todo.. extend the TransactionDetailResponse class to cover more details and get min max amount using linq lamba

            return await response.Content.ReadAsAsync<TransactionDetailResponse>(new[] { new JsonMediaTypeFormatter() });
        }
    }
}
