using Microsoft.AspNetCore.Mvc;

// A simple webview for anyone trying to go to the main API server 
namespace EyesOnTheNet.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
