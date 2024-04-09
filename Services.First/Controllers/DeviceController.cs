using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Data.Core;
using Services.First.Models;
using Services.First.Services.Abstraction;

namespace Services.First.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceServices _deviceServices;

        public DeviceController(IDeviceServices deviceServices)
        {
            _deviceServices = deviceServices;

        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<DeviceDto>> Geṭ̣̣̣̣̣̣̣̣(Guid id)
        {
            var device = await _deviceServices.GetSingleAsync(id);
            var deviceDto = new DeviceDto()
            {
                Name = device.Name,
                Description = device.Description,
            };

            return Ok(deviceDto);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Create(DeviceDto deviceDto)
        {
            var device = new Device()
            {
                Name = deviceDto.Name,
                Description= deviceDto.Description,
            };
            await _deviceServices.AddSingleAsync(device);

            return Created();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Update(Guid id)
        {
            var device = await _deviceServices.GetSingleAsync(id);
            device.Name = $"{device.Name} - Changed";

            await _deviceServices.UpdateSingleAsync(device);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _deviceServices.DeleteSingleAsync(id);

            return Ok();
        }
    }
}
