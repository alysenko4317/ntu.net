
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
                        Title = "Разработка",
                        Description = "Разработка программного обеспечения"
                    },
                    new Category
                    {
                        Title = "Рецепты",
                        Description = "Кулинарные рецепты на все случаи жизни"
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
                        Content = "Сегодня мы создали MVC веб-приложение!",
                        Category = context.Categories.Where(b => b.Title == "Разработка").FirstOrDefault(),
                        PublishDate = DateTime.Now
                    });

                context.SaveChanges();
            }
        }
    }
}

