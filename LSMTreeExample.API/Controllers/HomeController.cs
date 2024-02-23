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
        public KeyValue Get(int key)
        {
            var val = _lsmTreeService.Get(key);
            return new KeyValue { Key = key, Value = val };
        }
    }
}
