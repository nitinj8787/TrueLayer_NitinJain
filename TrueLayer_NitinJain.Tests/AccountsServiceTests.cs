using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using NUnit;
using NUnit.Framework;
using TrueLayer_NitinJain.Interfaces;
using TrueLayer_NitinJain.Services;

namespace TrueLayer_NitinJain.Tests
{
    public class AccountsServiceTests
    {
        IHandlerHttpClient _handlerClient;
        Mock<ISessionService> _sessionService;
        Mock<IAuthService> _authService;

        Mock<IOptions<HandlerSettings>> _appSettings;

        Mock<IHandlerHttpClient> _handlerClientMock;

        readonly string response_content_account = "{\"results\":[{\"update_timestamp\":\"2017-02-07T17:29:24.740802Z\",\"account_id\":\"f1234560abf9f57287637624def390871\",\"account_type\":\"TRANSACTION\",\"display_name\":\"Club Lloyds\",\"currency\":\"GBP\",\"account_number\":{\"iban\":\"GB35LOYD12345678901234\",\"number\":\"12345678\",\"sort_code\":\"12-34-56\"},\"provider\":{\"display_name\":\"Lloyds Bank\",\"provider_id\":\"lloyds\"}}]}";

        readonly String test_token = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjE0NTk4OUIwNTdDOUMzMzg0MDc4MDBBOEJBNkNCOUZFQjMzRTk1MTAiLCJ0eXAiOiJhdCtqd3QiLCJ4NXQiOiJGRm1Kc0ZmSnd6aEFlQUNvdW15NV9yTS1sUkEifQ.eyJuYmYiOjE1ODAyNDUzMjgsImV4cCI6MTU4MDI0ODkyOCwiaXNzIjoiaHR0cHM6Ly9hdXRoLnRydWVsYXllci1zYW5kYm94LmNvbSIsImF1ZCI6WyJodHRwczovL2F1dGgudHJ1ZWxheWVyLXNhbmRib3guY29tL3Jlc291cmNlcyIsImluZm9fYXBpIiwiYWNjb3VudHNfYXBpIiwidHJhbnNhY3Rpb25zX2FwaSIsImJhbGFuY2VfYXBpIiwiY2FyZHNfYXBpIiwiYmVuZWZpY2lhcmllc19hcGkiLCJwcm9kdWN0c19hcGkiXSwiY2xpZW50X2lkIjoic2FuZGJveC1uaXRpbmphaW4tZGNhMmYwIiwic3ViIjoiT1kvemtHTkVyV0VSb1lUL2oza3FQL1pScUZoV1NZUks0djNuOFdjMFIxWT0iLCJhdXRoX3RpbWUiOjE1ODAyNDUzMDUsImlkcCI6ImxvY2FsIiwiY29ubmVjdG9yX2lkIjoibW9jayIsImNyZWRlbnRpYWxzX2tleSI6Ijc4ZTExN2VlMmViMTczMmRhNzhmZDA3Y2M0MWVlZGNlMWI2YTVmNDExMWI1MWE3YWEwZWU1ODNmNjkyYWI2YWYiLCJwcml2YWN5X3BvbGljeSI6IkZlYjIwMTkiLCJjb25zZW50X2lkIjoiNjU2YTY3MjAtZjEwYS00ZGYwLWJhNjUtYjk3NjkyMmVkMDE0Iiwic2NvcGUiOlsiaW5mbyIsImFjY291bnRzIiwidHJhbnNhY3Rpb25zIiwiYmFsYW5jZSIsImNhcmRzIiwiYmVuZWZpY2lhcmllcyIsInByb2R1Y3RzIiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbInB3ZCJdfQ.nHrC91abbw6Drb2fRPLDGSePMIwS9UO1dK2hCbvRcFYT9iVnhibQP_oOqsOTt4kzKYYIom-zkg-i0MqOR-QUife1CLy0X22l5_le_qhR8ygmhgWZhbPyGoyHoqbzOJ8QnHZlea9vT4xlIFzbZBC9YIz0iiL7NxHPCTArtaNsjj7qSquAuxCPD7y5JGm_j_x3aIcnRqOd-FZJYXXOXHkrKaomaZ_G1ecbNQH_5pitTeiC6WF5nwKgccvF_98K_cnmaUe6VTsc3BIrpzym9sc-y6aGDMy78izgRHHo2UCtcWL2ULGzc780FESEVhU8MhgIT3gg5aEXyFi31nSjCtGT-w";

        readonly string callback_with_auth_code = @"http://localhost:3000/callback?code=xhOg6McZ_pgU2EG8YCc6gj1C3MxnVAx8PKzWJD2S29A&scope=info%20accounts%20balance%20transactions%20cards%20products%20beneficiaries%20offline_access";
            

        readonly string test_accountId = "56c7b029e0f8ec5a2334fb0ffc2fface";

        [SetUp]
        public void Setup()
        {
            _appSettings = new Mock<IOptions<HandlerSettings>>();
            _authService = new Mock<IAuthService>();
            _sessionService = new Mock<ISessionService>();

            _handlerClientMock = new Mock<IHandlerHttpClient>();

            // mock the session and provide token 
            _sessionService.Setup(s => s.GetAccessToken()).Returns(test_token);

            _appSettings.Setup(a => a.Value).Returns(new HandlerSettings
            {
                AuthBaseUri = "https://auth.truelayer-sandbox.com/",
                ApiBaseUri = "https://api.truelayer-sandbox.com/",
                AuthClientId = "c2FuZGJveC1uaXRpbmphaW4tZGNhMmYw",
                AuthRedirectUri = "http://localhost:3000/callback"
            });
        }

        [Test]
        public void Get_Account_By_Id_Should_Return_All_Transactions()
        {
            _handlerClient = new HttpClientHandlerService(_appSettings.Object, _sessionService.Object);

            _authService.Setup(a => a.GetAuthorizationUri()).Returns(new System.Uri(callback_with_auth_code));

            //act 
            AccountsServices ac = new AccountsServices(_appSettings.Object, _sessionService.Object, _authService.Object, _handlerClient);

            var response = ac.GetAccount(test_accountId);

            //assert
            Assert.NotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void Get_Account_By_Id_Should_Transactions()
        {
            // arrange
            HttpContent resContent = new StringContent(response_content_account);

            _handlerClientMock.Setup(a => a.GetDataFromTrueLayer(It.IsAny<string>(), It.IsAny<string>(), null)).ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK, Content = resContent });

            _handlerClient = new HttpClientHandlerService(_appSettings.Object, _sessionService.Object);

            //act 
            AccountsServices ac = new AccountsServices(_appSettings.Object, _sessionService.Object, _authService.Object, _handlerClientMock.Object);

            var response = ac.GetAccount(test_accountId);

            //assert
            Assert.NotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);


        }
    }
}
