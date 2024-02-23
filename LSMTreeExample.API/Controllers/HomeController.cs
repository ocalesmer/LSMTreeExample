using LSMTreeExample.API.Business.Interfaces;
using LSMTreeExample.API.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace LSMTreeExample.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HomeController : ControllerBase
    {
        private readonly ILSMTreeService _lsmTreeService;

        public HomeController(ILSMTreeService lsmTreeService)
        {
            _lsmTreeService = lsmTreeService;
        }

        [HttpGet]
        public IActionResult Get(int key)
        {
            var val = _lsmTreeService.Get(key);
            return Ok(new KeyValue { Key = key, Value = val });
        }
        [HttpGet]
        public IActionResult Delete(int key)
        {
            _lsmTreeService.Delete(key);
            return Ok();
        }
        [HttpPost]
        public IActionResult Put(KeyValue request)
        {
            _lsmTreeService.Put(request.Key,request.Value);
            return Ok();
        }
    }
}
