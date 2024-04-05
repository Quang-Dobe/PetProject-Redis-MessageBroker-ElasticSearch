using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.First.Services.Abstraction;

namespace Services.First.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly ICacheServices _cacheServices;

        public RedisController(ICacheServices cacheServices)
        {
            _cacheServices = cacheServices;
        }

        [HttpGet]
        public async Task<List<string>> GetAll()
        {
            var result = new List<string>();

            return result;
        }

        [HttpGet]
        [Route("{key}")]
        public async Task<string> GetByKey(string key)
        {
            var result = _cacheServices.GetData<string>(key);

            return result;
        }

        [HttpPost]
        [Route("{key}")]
        public async Task<bool> AddByKey(string key, [FromQuery] string value)
        {
            var result = _cacheServices.SetData(key, value, DateTimeOffset.Now.AddDays(1));

            return result;
        }

        [HttpDelete]
        [Route("{key}")]
        public async Task<bool> DeleteByKey(string key)
        {
            var result = _cacheServices.RemoveData(key);

            return result;
        }
    }
}
