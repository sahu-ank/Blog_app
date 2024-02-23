using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog_app.Models;
using mvc.Data;

namespace Blog_app.Controllers
{
    public class BlogsController : Controller
    {
        private readonly mvcContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlogsController(mvcContext context
            , IHttpContextAccessor httpContextAccessor
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private Boolean SetSessionDataInViewBag()
        {
            var username = _httpContextAccessor.HttpContext.Session.GetString("Username");
            var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
            if (username != null && role != null)
            {
                return true;
            }
            return false;
        }
        public async Task<IActionResult> Index()
        {
            if (!SetSessionDataInViewBag())
            {
                ViewBag.errorList = "Please Login first";
                return RedirectToAction("Index", "Auth");
            }
              return _context.Blogs != null ? 
                          View(await _context.Blogs.ToListAsync()) :
                          Problem("Entity set 'mvcContext.Blogs'  is null.");
        }

        public async Task<IActionResult> Details(int? id)
        {
            
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blogs = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogs == null)
            {
                return NotFound();
            }

            return View(blogs);
        }

        public IActionResult Create()
        {
            if (!SetSessionDataInViewBag())
            {
                ViewBag.errorList = "Please Login first";
                return RedirectToAction("Index", "Auth");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tttle,DateCreated,DateModified")] Blogs blogs)
        {
            
                blogs.DateCreated = DateTime.UtcNow.ToString();
                blogs.DateModified = DateTime.UtcNow.ToString();
                _context.Add(blogs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!SetSessionDataInViewBag())
            {
                ViewBag.errorList = "Please Login first";
                return RedirectToAction("Index", "Auth");
            }
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blogs = await _context.Blogs.FindAsync(id);
            if (blogs == null)
            {
                return NotFound();
            }
            return View(blogs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tttle,DateCreated,DateModified")] Blogs blogs)
        {
            if (id != blogs.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blogs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogsExists(blogs.Id))
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
            return View(blogs);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!SetSessionDataInViewBag())
            {
                ViewBag.errorList = "Please Login first";
                return RedirectToAction("Index", "Auth");
            }
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blogs = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogs == null)
            {
                return NotFound();
            }

            return View(blogs);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Blogs == null)
            {
                return Problem("Entity set 'mvcContext.Blogs'  is null.");
            }
            var blogs = await _context.Blogs.FindAsync(id);
            if (blogs != null)
            {
                _context.Blogs.Remove(blogs);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogsExists(int id)
        {
          return (_context.Blogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
