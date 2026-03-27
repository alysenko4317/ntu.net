
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webblog.Models;
using Microsoft.EntityFrameworkCore;

namespace webblog.Controllers
{
    public class BlogController : Controller
    {
        BlogContext _db;

        public BlogController(BlogContext ctx)
        {
            _db = ctx;
        }

        public async Task<IActionResult> Index()
        {
            //Where(d => !d.IsDraft)
            var posts = await _db.Posts.Include(c => c.Category).ToListAsync();
            return View(posts);
        }

        [HttpGet]
        public async Task<IActionResult> Modify(Guid id)
        {
            Post post;
            try
            {
                post = await _db.Posts.Include(c => c.Category).SingleAsync(i => i.Id == id);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        public void Modify(string category, string post)
        {
            // TODO: save modified data to DB
            Response.Redirect("/Blog/");
        }
    }
}
