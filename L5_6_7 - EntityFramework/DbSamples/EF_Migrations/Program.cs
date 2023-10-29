
using System.Data;
using Microsoft.EntityFrameworkCore;

// install Npgsql.EntityFrameworkCore.PostgreSQL
//         Microsoft.EntityFrameworkCore.Tools
//
// after project created and required packages installed execute:
//    Add-Migration InitialCreate
//    Update-Database

namespace Application
{
    // ---------------------------
    // Entities
    // ---------------------------

    public class Subject
    {
        public Subject() {
            this.StudentGroups = new HashSet<StudentGroup>();
        }

        public int Id { get; set; }
        public string SubjectName { get; set; }
        public int Value { get; set; }

        public virtual SubjectType SubjectType { get; set; }  // many to one
        public virtual ICollection<StudentGroup> StudentGroups { get; set; }  // many to many
    }

    public class SubjectType
    {
        public SubjectType() {
            this.ChildSubjectTypes = new HashSet<SubjectType>();
        }

        public int Id { get; set; }
        public string TypeName { get; set; }

        public int? ParentSubjectTypeId { get; set; }  // allows for null

        public virtual SubjectType ParentSubjectType { get; set; }
        public virtual ICollection<SubjectType> ChildSubjectTypes { get; set; }
    }

    public class StudentGroup
    {
        public StudentGroup() {
            this.Subjects = new HashSet<Subject>();
        }

        public int Id { get; set; }
        public string GroupName { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }  // many to many
        public string? Comment { get; set; }
    }

    // ---------------------------
    // DbContext
    // ---------------------------

    public class LearningModelContainer : DbContext
    {
        public DbSet<Subject> SubjectSet { get; set; }
        public DbSet<SubjectType> SubjectTypeSet { get; set; }
        public DbSet<StudentGroup> StudentGroupSet { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql("Host=localhost;Port=5433;Username=postgres;Password=1234;Database=learning_db");
                //.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information); // Adding EF logging
        }
    }

    // ---------------------------
    // Main
    // ---------------------------

    class App
    {
        public static void WriteSubjectTypeTree(int ParentSubjectTypeParam, int LevelParam)
        {
            LearningModelContainer db = new LearningModelContainer();

            var q = from x in db.SubjectTypeSet select x;

            if (ParentSubjectTypeParam == -1)
            {
                // Поиск корневых элементов (ParentSubjectType == null)
                q = q.Where(x => x.ParentSubjectType == null);
            }
            else
            {
                // Поиск элементов с заданным элементом верхнего уровня
                q = q.Where(x => x.ParentSubjectType.Id == ParentSubjectTypeParam);
            }

            // Если существуют элементы на данном уровне иерархии
            if (q.Count() > 0)
            {
                // Сортировка
                q = q.OrderBy(x => x.TypeName);

                // Перебор всех значений на заданном уровне иерархии
                foreach (var x in q)
                {
                    // Вывод отступа
                    if (LevelParam > 0)
                    {
                        for (int i = 0; i < LevelParam; i++)
                            Console.Write("   ");

                        Console.Write("|--");
                    }

                    // Вывод значения
                    Console.WriteLine(x.TypeName);

                    // Рекурсивный вызов функции для всех элементов, вложенных в текущий
                    WriteSubjectTypeTree(x.Id, LevelParam + 1);
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello");
            LearningModelContainer db = new LearningModelContainer();
            //dbinit.InitData();
            WriteSubjectTypeTree(-1, 0);  // Выдача иерархии типов предметов
            Console.ReadLine();
        }
    }
}
