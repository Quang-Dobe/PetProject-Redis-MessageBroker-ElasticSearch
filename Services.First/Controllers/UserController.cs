using Microsoft.AspNetCore.Mvc;
using Services.Data.Core;
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
        public async Task<ActionResult<UserDto>> Geṭ̣̣̣̣̣̣̣̣(Guid id)
        {
            var user = await _userServices.GetSingleAsync(id);
            var userDto = new UserDto()
            {
                Name = user.Name,
                Email = user.Email,
                UserName = user.Name,
            };

            return Ok(userDto);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Create(UserDto userDto)
        {
            var user = new User()
            {
                Name = userDto.Name,
                Email = userDto.Email,
                UserName = userDto.Name,
                Password = "Auto Generated"

            };
            await _userServices.AddSingleAsync(user);

            return Created();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Update(Guid id)
        {
            var user = await _userServices.GetSingleAsync(id);
            user.Name = $"{user.Name} - Changed";

            await _userServices.UpdateSingleAsync(user);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _userServices.DeleteSingleAsync(id);

            return Ok();
        }
    }
}
