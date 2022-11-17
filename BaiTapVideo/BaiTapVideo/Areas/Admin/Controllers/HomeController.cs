using BaiTapVideo.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaiTapVideo.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly db_newsContext _context;

        public HomeController(db_newsContext context)
        {
            _context = context;
        }

        [Area("Admin")]
        public IActionResult Index()
        {
            ViewBag.NumCat = _context.Categories.Where(tt => tt.Status == 1).ToList().Count();
            ViewBag.NumPost = _context.Posts.Where(tt => tt.Status == 1).ToList().Count();
            return View();
        }
    }
}
