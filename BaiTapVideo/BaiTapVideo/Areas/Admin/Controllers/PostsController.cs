using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaiTapVideo.Areas.Admin.Models;
using NuGet.Protocol;

namespace BaiTapVideo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostsController : Controller
    {
        private readonly db_newsContext _context;

        public PostsController(db_newsContext context)
        {
            _context = context;
        }

        // GET: Admin/Posts
        public async Task<IActionResult> Index()
        {
            var db_demoContext = _context.Posts.Where(s => s.Status == 1).Include(p => p.Cat);
            List<Status> ListStatus = new List<Status>();
            ListStatus.Add(new Status(0, "Không hoạt động"));
            ListStatus.Add(new Status(1, "Hoạt động"));
            ListStatus.Add(new Status(2, "Tất cả"));
            ViewData["listStatus"] = new SelectList(ListStatus, "id", "name");
            ViewData["ListCat"] = new SelectList(_context.Categories, "Id", "Name");

            ViewData["NumRs"] = db_demoContext.ToList().Count();

            
            
            return View(await db_demoContext.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Index(string txt_key,int ddl_cat,int ddl_stt)
        {
            List<Status> ListStatus = new List<Status>();
            if (ddl_stt == 2)
            {
                var db = _context.Posts.Where(st => st.Title.ToLower().Contains(txt_key.ToLower()) && st.CatId == ddl_cat);
                ListStatus.Add(new Status(0, "Không hoạt động"));
                ListStatus.Add(new Status(1, "Hoạt động"));
                ListStatus.Add(new Status(2, "Tất cả"));
                
                ViewData["ListCat"] = new SelectList(_context.Categories, "Id", "Name");
                ViewData["listStatus"] = new SelectList(ListStatus, "id", "name");
                ViewData["NumRs"] = db.ToList().Count();
                return View(await db.ToListAsync());
            }


            var db_newsContext = _context.Posts.Where(st => st.Title.ToLower().Contains(txt_key.ToLower()) && st.CatId == ddl_cat&&st.Status == ddl_stt);
            ViewData["ListCat"] = new SelectList(_context.Categories, "Id", "Name");
            
            ListStatus.Add(new Status(0, "Không hoạt động"));
            ListStatus.Add(new Status(1, "Hoạt động"));
            ListStatus.Add(new Status(2, "Tất cả"));
            ViewData["listStatus"] = new SelectList(ListStatus, "id", "name");
            ViewData["NumRs"] = db_newsContext.ToList().Count();
            return View(await db_newsContext.ToListAsync());
        }

        // GET: Admin/Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Cat)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Admin/Posts/Create
        public IActionResult Create()
        {
            List<Status> ListStatus = new List<Status>();
            ListStatus.Add(new Status(0, "Không hoạt động"));
            ListStatus.Add(new Status(1, "Hoạt động"));
            ViewData["listStatus"] = new SelectList(ListStatus, "id", "name");
            ViewData["CatId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Admin/Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,FullContent,Img,Status,CatId")] Post post,IFormFile ful_img)
        {
            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", post.Id + Path.GetExtension(ful_img.FileName));
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await ful_img.CopyToAsync(stream);
                }
                post.Img = post.Id.ToString() + Path.GetExtension(ful_img.FileName);
                _context.Update(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatId"] = new SelectList(_context.Categories, "Id", "Name", post.CatId);
            return View(post);
        }

        // GET: Admin/Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            string? img = _context.Posts.Where(p => p.Id == post.Id).Select(x => new { x.Img }).Single().Img;
            
            ViewData["img"] = img;
            List<Status> ListStatus = new List<Status>();
            ListStatus.Add(new Status(0, "Không hoạt động"));
            ListStatus.Add(new Status(1, "Hoạt động"));
            ViewData["listStatus"] = new SelectList(ListStatus, "id", "name");
            ViewData["CatId"] = new SelectList(_context.Categories, "Id", "Name", post.CatId);
            return View(post);
        }

        // POST: Admin/Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,FullContent,Img,Status,CatId")] Post post,IFormFile? ful_img)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ful_img != null)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", post.Id + Path.GetExtension(ful_img.FileName));
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await ful_img.CopyToAsync(stream);
                        }
                        post.Img = post.Id.ToString() + Path.GetExtension(ful_img.FileName);
                    }
                    else
                    {
                        post.Img = _context.Posts.Where(p => p.Id == post.Id).Select(x => new { x.Img }).Single().Img;
                    }
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            List<Status> ListStatus = new List<Status>();
            ListStatus.Add(new Status(0, "Không hoạt động"));
            ListStatus.Add(new Status(1, "Hoạt động"));
            ViewData["listStatus"] = new SelectList(ListStatus, "id", "name");
            ViewData["CatId"] = new SelectList(_context.Categories, "Id", "Name", post.CatId);
            return View(post);
        }

        // GET: Admin/Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Cat)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Admin/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'db_newsContext.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                post.Status = 0;
                _context.Posts.Update(post);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
          return _context.Posts.Any(e => e.Id == id);
        }
    }
}
