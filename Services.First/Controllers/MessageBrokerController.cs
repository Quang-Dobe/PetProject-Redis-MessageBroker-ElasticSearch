using Microsoft.AspNetCore.Mvc;
using Services.First.Services.Abstraction;

namespace Services.First.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageBrokerController : ControllerBase
    {
        private readonly IMessageBrokerServices _messageBrokerServices;

        public MessageBrokerController(IMessageBrokerServices messageBrokerServices)
        {
            _messageBrokerServices = messageBrokerServices;
        }

        [HttpPost]
        public async Task<bool> Send([FromQuery] string message)
        {
            _messageBrokerServices.SendMessage(message);

            return true;
        }
    }
}
