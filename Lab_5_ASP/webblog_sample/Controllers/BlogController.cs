
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webblog.Models;
using webblog.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace webblog.Controllers
{
    public class BlogController : Controller
    {
        BlogContext _db;

        public BlogController(BlogContext ctx) {
            _db = ctx;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _db.Posts.Include(c => c.Category).ToListAsync();
            //return View(posts);  // no bootstrap styling
            return View("IndexBootstrapStyled", posts);
        }

        public IActionResult Create()  // Додавання нового посту
        {
            var viewModel = new PostViewModel {
                Categories = _db.Categories.Select(c => new SelectListItem {
                    Value = c.Id.ToString(),
                    Text = c.Title
                })
            };

            //return View(viewModel);
            return View("CreateModifyPost", viewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(PostViewModel model)
        {
            if (ModelState.IsValid) {
                var post = new Post {
                    Title = model.Title,
                    PublishDate = model.PublishDate,
                    IsDraft = model.IsDraft,
                    Content = model.Content,
                    CategoryId = model.CategoryId
                };

                _db.Add(post);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));  // перенаправлення до списку постів
            }
            
            // якщо є помилки в моделі
            model.Categories = _db.Categories.Select(c => new SelectListItem {
                Value = c.Id.ToString(),
                Text = c.Title
            });

            //return View(model);
            return View("CreateModifyPost", model);
        }
        
        public async Task<IActionResult> Edit(Guid id)   // Редагування посту
        {
            var post = await _db.Posts.FindAsync(id);
            if (post == null)
                return NotFound();

            var viewModel = new PostViewModel {
                Id = post.Id,
                Title = post.Title,
                PublishDate = post.PublishDate,
                IsDraft = post.IsDraft,
                Content = post.Content,
                CategoryId = post.CategoryId,
                Categories = _db.Categories.Select(c => new SelectListItem {
                    Value = c.Id.ToString(),
                    Text = c.Title,
                    Selected = post.CategoryId == c.Id
                })
            };

            //return View(viewModel);
            return View("CreateModifyPost", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var post = await _db.Posts.FindAsync(model.Id);
                if (post == null)
                    return NotFound();

                post.Title = model.Title;
                post.PublishDate = model.PublishDate;
                post.IsDraft = model.IsDraft;
                post.Content = model.Content;
                post.CategoryId = model.CategoryId;

                _db.Update(post);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index)); // перенаправлення до списку постів
            }

            // якщо є помилки в моделі
            model.Categories = _db.Categories.Select(c => new SelectListItem {
                Value = c.Id.ToString(),
                Text = c.Title
            });

            //return View(model);
            return View("CreateModifyPost", model);
        }

        public async Task<IActionResult> Delete(Guid id)   // Видалення посту
        {
            var post = await _db.Posts.FindAsync(id);
            if (post != null) {
                _db.Posts.Remove(post);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
