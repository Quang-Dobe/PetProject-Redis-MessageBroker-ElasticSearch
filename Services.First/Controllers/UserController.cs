using Microsoft.AspNetCore.Mvc;
using Services.First.Models;
using Services.First.Services.Abstraction;

namespace Services.First.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;

        }

        [HttpGet]
        [Route("{id}")]
        public async Task<UserDto> Geṭ̣̣̣̣̣̣̣̣(Guid id)
        {
            var user = await _userServices.GetSingleAsync(id);

            return new UserDto();
        }
    }
}
