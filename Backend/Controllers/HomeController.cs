using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class HomeController : Controller
    {
        [HttpGet("index")]
        public bool Index()
        {
            var test = true;
            return test;
        }


    }
}
