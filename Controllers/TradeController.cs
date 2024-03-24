using Microsoft.AspNetCore.Mvc;

namespace FakeDataGenerate.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TradeController : ControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
