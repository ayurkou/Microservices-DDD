using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Accounting.Sdk;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Accounting.Api.Controllers
{
    [ApiVersion("0.1", Deprecated = true)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly IBus _bus;

        public ValuesController(ILogger<ValuesController> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var temp = new
            {
                A = "a",
                B = "b"
            };
            _logger.LogWarning("Test warninig {@temp}", temp);
            
            await _bus.Publish(new TestEvent
            {
                Title = "test",
                CorrelationId = Guid.NewGuid()
            });
            
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
