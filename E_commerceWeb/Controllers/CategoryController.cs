using Microsoft.AspNetCore.Mvc;

namespace E_commerceWeb.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
