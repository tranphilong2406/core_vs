using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaiTapVideo.Areas.Admin.Models;
using System.Drawing;
using NuGet.Protocol;

namespace BaiTapVideo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly db_newsContext _context;

        public CategoriesController(db_newsContext context)
        {
            _context = context;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index()
        {
            //var query = from cat in _context.Categories
            //join p in _context.Posts
            //on cat.Id
            //equals p.CatId
            //group new { cat, p } by new
            //{
            //   cat.Id,cat.Name,cat.Slug,cat.Status,p.CatId
            //} into gr
            //select new
            //{
            //    id = gr.Key.Id,
            //    name = gr.Key.Name,
            //    slug = gr.Key.Slug,
            //    status = gr.Key.Status,
            //    count = gr.Count()
            //};
            //List<CategoryCustom> ls = new List<CategoryCustom>();
            //foreach (var cat in query)
            //{
            //    ls.Add(new CategoryCustom(cat.id,cat.name,cat.slug,cat.status,cat.count));
            //}



            //context là nguyên cái db được đưa lên ram nhờ vào depency ịnection(cơ chế team phụ thuộc)
            // .categories là truy xuất đến bảng trong db
            // phương thức select được cung cấp cho danh sách, nó truyền cho biết ta cần chọn thuộc tính này và tạo ra một danh sách mới
            // .where lấy tất cả trường dữ liệu với điều kiện kèm theo select sẽ lấy các thuộc tính từ select
            // 
            //ViewBag.query1 = _context.Categories.Where(cat=> cat.Name.Contains("T")).Select(c => new { c.Id, c.Name }).ToJson();
            //ViewBag.query = _context.Posts.Join(_context.Categories, p => p.CatId, cat => cat.Id, (p, cat) => new
            //{
            //    p.Id,
            //    p.Title,
            //    cat.Name,
            //}).ToJson();

            //ViewBag.query3 = _context.Categories.GroupJoin(_context.Posts, cat => cat.Id, p => p.CatId, (cat, p) => new { cat, p }).SelectMany(x => x.p.DefaultIfEmpty(), (a, b) => new
            //{
            //    PostTit=b.Title==null?"":b.Title,Slug = a.cat.Slug,CatName = a.cat.Name
            //}).GroupBy(x => x.CatName).Select(y => new {y.Key,SL=y.Where(m=>m.PostTit!="").Count()}).ToJson();

            ViewBag.query3 = _context.Categories.ToJson();

            return View();
        }

        // GET: Admin/Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/Categories/Create
        public IActionResult Create()
        {
            List<Status> ListStatus = new List<Status>();
            ListStatus.Add(new Status(0, "Không hoạt động"));
            ListStatus.Add(new Status(1, "Hoạt động"));
            ViewData["listStatus"] = new SelectList(ListStatus, "id", "name");
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Slug,Status")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            List<Status> ListStatus = new List<Status>();
            ListStatus.Add(new Status(0, "Không hoạt động"));
            ListStatus.Add(new Status(1, "Hoạt động"));
            ViewData["listStatus"] = new SelectList(ListStatus, "id", "name");
            ViewBag.cat = category.ToJson();
            return RedirectToAction(nameof(Index),category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Slug,Status")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            ViewData["CatDel"] = category.ToJson();

            return View("Index");
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'db_newsContecxt.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                category.Status = 0;
                _context.Categories.Update(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return _context.Categories.Any(e => e.Id == id);
        }
    }
}
