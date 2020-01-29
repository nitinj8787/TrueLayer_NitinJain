using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TrueLayer_NitinJain.Interfaces;

namespace TrueLayer_NitinJain.Tests
{
    public class AuthServiceTests
    {
        IHandlerHttpClient _handlerClient;
        Mock<ISessionService> _sessionService;
        Mock<IAuthService> _authService;

        Mock<IOptions<HandlerSettings>> _appSettings;


        [SetUp]
        public void Setup()
        {
            _appSettings = new Mock<IOptions<HandlerSettings>>();
            _authService = new Mock<IAuthService>();
            _sessionService = new Mock<ISessionService>();
            
            
        }

    }
}
