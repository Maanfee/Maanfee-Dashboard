using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoggingPlatform.Controllers
{
	[ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        private readonly ILogger<TestController> _logger;

        [Authorize]
        [HttpGet("GetStatus")]
        public string GetStatus()
        {
            return "Authorized";
        }

        [HttpGet("ConnectionTest")]
        public string ConnectionTest()
        {
			return "The connection is established.";
		}

	}

}