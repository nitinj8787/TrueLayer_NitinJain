using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TrueLayer_NitinJain.Interfaces;

namespace TrueLayer_NitinJain.Services
{
    public class SessionService : ISessionService
    {
        private const string ACCESS_TOKEN_NAME = "token";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetAccessToken()
        {
            return TryGetString(ACCESS_TOKEN_NAME);
        }

        public void SetTokens(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                string jsonString = response.Content.ReadAsStringAsync().Result;
                Dictionary<string, string> attrs = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
                _httpContextAccessor.HttpContext.Session.SetString(ACCESS_TOKEN_NAME, attrs["access_token"]);              
            }
        }

        /// <summary>
        /// Return session value as a string (if it exists), or an empty string.
        /// </summary>
        private string TryGetString(string name)
        {
            byte[] valueBytes = new Byte[700];
            bool valueOkay = _httpContextAccessor.HttpContext.Session.TryGetValue(name, out valueBytes);
            if (valueOkay)
            {
                return System.Text.Encoding.UTF8.GetString(valueBytes);
            }
            return null;
        }
    }
}
