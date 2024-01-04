
using System;
using System.Linq;
using System.Data;

namespace webblog.Models
{
    public static class SampleData
    {
        public static void Initialize(BlogContext context)
        {
            if (! context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category
                    {
                        Title = "Розробка",
                        Description = "Розробка програмного забезпечення"
                    },
                    new Category
                    {
                        Title = "Рецепты",
                        Description = "Кулінарні рецепти на всі випадки життя"
                    });

                context.SaveChanges();
            }
            
            if (! context.Posts.Any())
            {
                context.Posts.Add(
                    new Post
                    {
                        Title = "sample post",
                        IsDraft = false,
                        Content = "Сьогодні ми створили MVC веб-додаток!",
                        Category = context.Categories.Where(b => b.Title == "Розробка").FirstOrDefault(),
                        PublishDate = DateTime.Now
                    });

                context.SaveChanges();
            }
        }
    }
}

