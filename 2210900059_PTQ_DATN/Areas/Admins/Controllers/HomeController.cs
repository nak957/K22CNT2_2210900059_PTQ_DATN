using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using _2210900059_PTQ_DATN.Models;

namespace _2210900059_PTQ_DATN.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class HomeController : Controller
    {
   
        public IActionResult Index()
        {
            return View();
        }

      
 
    }
}
