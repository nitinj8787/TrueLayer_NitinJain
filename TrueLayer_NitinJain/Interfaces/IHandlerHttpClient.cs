using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TrueLayer_NitinJain.Interfaces
{
    public interface IHandlerHttpClient
    {
        Task<HttpResponseMessage> GenerateTokenFromAuthServerPostAsync(string uri, HttpContent content);

        Task<HttpResponseMessage> GetDataFromTrueLayer(string method, string uri, HttpContent content=null);

    }
}
