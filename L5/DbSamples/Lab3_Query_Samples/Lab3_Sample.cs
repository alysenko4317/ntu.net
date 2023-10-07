
using System.Data;
using Microsoft.EntityFrameworkCore;

// install Microsoft.EntityFrameworkCore
//         Microsoft.EntityFrameworkCore.SqlServer
//         Microsoft.EntityFrameworkCore.Proxies

namespace EntityLINQ
{
    // Represents subjects with an associated SubjectType
    //    and a many-to-many relationship with StudentGroup.
    public class Subject
    {
        public Subject() {
            // for clarity and safety, it's good practice to initialize collection navigation
            // properties in the entity's constructor.
            this.StudentGroups = new HashSet<StudentGroup>();
        }

        public int Id { get; set; }
        public string SubjectName { get; set; }
        public int Value { get; set; }

        // Ensure that navigation properties (like SubjectType in Subject or ParentSubjectType in SubjectType)
        //    are virtual. This is important if you plan to leverage lazy loading in EF Core.
        public virtual SubjectType SubjectType { get; set; }  // many to one
        public virtual ICollection<StudentGroup> StudentGroups { get; set; }  // many to many
    }

    // Represents types of subjects and includes a potential parent SubjectType,
    //    facilitating a hierarchical structure.
    //
    // Self-referencing Relationship in SubjectType:
    //    the ParentSubjectType property in SubjectType implies a self-referencing
    //    one-to-many relationship, which is supported by EF Core. However, for clarity
    //    and to potentially avoid issues, you might also want to include a collection
    //    property in the SubjectType for child SubjectTypes:
    //        public virtual ICollection<SubjectType> ChildSubjectTypes { get; set; }
    public class SubjectType
    {
        public SubjectType() {
            this.ChildSubjectTypes = new HashSet<SubjectType>();
        }

        public int Id { get; set; }
        public string TypeName { get; set; }

        // addition of this property is a good practice - it explicitly defines
        // the relationship in the model and allows for the possibility of a SubjectType
        // not having a parent (i.e., being at the top of the hierarchy); explicitly
        // modeling relationships with clear foreign keys (even if nullable) makes
        // the data model more understandable and less prone to ambiguities.
        //
        // Without the ParentSubjectTypeId property, Entity Framework (EF) has to infer
        // the foreign key property it needs to use for the relationship. EF conventions
        // typically generate a foreign key with a non-nullable column in this scenario.
        // This means that every SubjectType record MUST have a valid parent SubjectType ID,
        // even if logically there are types that don't have parents.
        // This can lead to a problem: what if there are root SubjectType entities that
        // don't have parents? In the database, the foreign key column would not allow
        // null values, leading to constraint violations.
        //
        // By explicitly defining ParentSubjectTypeId you are making a couple of things clear:
        //
        //    Nullability: The ? indicates that ParentSubjectTypeId is a nullable integer (int?).
        //    This means it can store null values, representing SubjectType entities without parents.
        //
        //    Explicit Foreign Key: You're giving EF an explicit foreign key property to use for the
        //    relationship, removing any ambiguity that might arise from convention-based inferences.
        //    When EF Core sees this property, it understands that the ParentSubjectTypeId column in
        //    the database should allow null values. As a result, it sets up the foreign key constraint
        //    in such a way that it's possible for a SubjectType to not have a parent
        //       (its ParentSubjectTypeId can be null).
        //
        // By explicitly specifying the foreign key property, you're making your model's intentions
        // clearer, both to the ORM (EF Core) and to developers who might work on the code. This reduces
        // the likelihood of misinterpretations by the ORM and minimizes the risk of unintended behaviors
        // or constraints in the generated database schema.
        //
        public int? ParentSubjectTypeId { get; set; }  // allows for null

        public virtual SubjectType ParentSubjectType { get; set; }
        public virtual ICollection<SubjectType> ChildSubjectTypes { get; set; }
    }

    // Represents student groups, which have a many-to-many relationship with Subject.
    public class StudentGroup
    {
        public StudentGroup() {
            this.Subjects = new HashSet<Subject>();
        }

        public int Id { get; set; }
        public string GroupName { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }  // many to many
    }

    public class LearningModelContainer : DbContext
    {
        public DbSet<Subject> SubjectSet { get; set; }
        public DbSet<SubjectType> SubjectTypeSet { get; set; }
        public DbSet<StudentGroup> StudentGroupSet { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=schooldb;Trusted_Connection=True;MultipleActiveResultSets=True")
                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        }

        // If you need more specific configurations:
        //    Depending on your specific needs, you might want to further configure
        //    these relationships using the Fluent API inside the OnModelCreating method
        //    of your DbContext
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Example: Configuring the self-referencing relationship in SubjectType
            modelBuilder.Entity<SubjectType>()
                .HasOne(st => st.ParentSubjectType)
                .WithMany(st => st.ChildSubjectTypes)
                .HasForeignKey(st => st.ParentSubjectTypeId);
                //.OnDelete(DeleteBehavior.Restrict);
                   // or .OnDelete(DeleteBehavior.SetNull) if you want to set FK to NULL when parent is deleted

            // this method can be commented in original sample (i.e. if you haven't done any significant
            // changes in other code related to ORM functionality)
        }
    }

    public class LearningModelContainer_WithLazyLoad : LearningModelContainer
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // LazyLoading is disabled by default. To enable it 3 steps are required:
            //   1. Install the Microsoft.EntityFrameworkCore.Proxies package
            //   2. Use the UseLazyLoadingProxies method to enable the creation of proxies
            //   3. All navigation properties should be declared as public virtual
            // See the link for details:
            //    https://www.learnentityframeworkcore.com/lazy-loading#:~:text=Lazy%20loading%20of%20data%20is,the%20performance%20of%20an%20application.
            //
            // Also note using the "MultipleActiveResultSets=True" option in connection string.
            // Without this option accessing navigation option will fire an exception:
            //    https://stackoverflow.com/questions/5440168/exception-there-is-already-an-open-datareader-associated-with-this-connection-w
            
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }

    class Program
    {
        static void ClearData()  // Очистка данных
        {
            Console.WriteLine("Clean the database: ");

            LearningModelContainer db = new LearningModelContainer();

            // Удаление данных для связи много-ко-многим для каждой записи StudentGroup
            // удаляются все связи с Subject
            Console.WriteLine("  -remove many-to-many links for Subject <-> StudentGroup");
            foreach (var gr in db.StudentGroupSet.ToList())
            {
                foreach (var gr_subj in gr.Subjects.ToList())
                {
                    gr.Subjects.Remove(gr_subj);
                }
            }

            db.SaveChanges();

            // do this first because StudentGroupSet items refer Subjects
            Console.WriteLine("  -remove SubjectSet table...");
            db.SubjectSet.RemoveRange(db.SubjectSet);
            db.SaveChanges();

            Console.WriteLine("  -remove StudentGroupSet table...");
            db.StudentGroupSet.RemoveRange(db.StudentGroupSet);
            db.SaveChanges();

            Console.WriteLine("  -remove SubjectTypeSet table...");

            // SubjectType is self-referenced and therefore you can't just call RemoveRange() here.
            // If you try to remove all entities such a way, you could run into issues due to the
            // hierarchical nature of the data. When you try to delete a "parent" entity that still
            // has "children" pointing to it via a foreign key relationship, the database will raise
            // an error due to the foreign key constraint.
            //    db.SubjectTypeSet.RemoveRange(db.SubjectTypeSet);
            //
            // To handle this, you have a couple of options:
            // Recursive Deletion:
            //    Implement a method to recursively delete the entities starting from the leaf
            //    nodes(those without children) and working your way up to the root.
            // Disable Foreign Key Constraint Temporarily:
            //    This is a more drastic approach and isn't usually recommended for production environments,
            //    but for development or testing, you can temporarily disable the foreign key constraint, clear
            //    the table, and then re-enable the constraint. Remember, this can leave your database in an
            //    inconsistent state if not done carefully.
            // Ordered Deletion:
            //    You can fetch all the entities, order them in a way that "children" come before "parents",
            //    and then delete them in that order.
            //
            // Here's a simple method for the recursive deletion. Note, if your data hierarchy is deep,
            // recursive methods can run into stack limits. In such cases, you'd have to consider more
            // complex iterative methods to handle deletions.
            //
            var rootNodes = db.SubjectTypeSet.Where(st => st.ParentSubjectType == null).ToList();
            foreach (var rootNode in rootNodes) {
                DeleteSubjectTypeRecursively(rootNode, db);
            }

            db.SaveChanges();
        }

        public static void DeleteSubjectTypeRecursively(SubjectType subjectType, LearningModelContainer db)
        {
            // Fetch child SubjectTypes
            var children = db.SubjectTypeSet.Where(st => st.ParentSubjectType.Id == subjectType.Id).ToList();

            // Recursively delete children
            foreach (var child in children)
                DeleteSubjectTypeRecursively(child, db);

            // Now, delete the current entity
            db.SubjectTypeSet.Remove(subjectType);
        }

        static void InitData()   // Заполнение данных
        {
            Console.WriteLine("Initialize the database with sample data...");

            LearningModelContainer db = new LearningModelContainer();

            // Добавление типов предметов
            SubjectType st_tech = new SubjectType
            {
                TypeName = "технический цикл",
                ParentSubjectType = null
            };

            db.SubjectTypeSet.Add(st_tech);

            SubjectType st_hum = new SubjectType
            {
                TypeName = "гуманитарный цикл",
                ParentSubjectType = null
            };

            SubjectType st1 = new SubjectType
            {
                TypeName = "базовые",
                ParentSubjectType = st_tech
            };

            SubjectType st2 = new SubjectType
            {
                TypeName = "специальные",
                ParentSubjectType = st_tech
            };

            SubjectType st3 = new SubjectType
            {
                TypeName = "исторические",
                ParentSubjectType = st_hum
            };

            SubjectType st3_1 = new SubjectType
            {
                TypeName = "новая история",
                ParentSubjectType = st3
            };

            SubjectType st3_2 = new SubjectType
            {
                TypeName = "новейшая история",
                ParentSubjectType = st3
            };

            db.SubjectTypeSet.Add(st_tech);
            db.SubjectTypeSet.Add(st_hum);
            db.SubjectTypeSet.Add(st1);
            db.SubjectTypeSet.Add(st2);
            db.SubjectTypeSet.Add(st3);
            db.SubjectTypeSet.Add(st3_1);
            db.SubjectTypeSet.Add(st3_2);

            // Добавление предметов
            Subject sb1 = new Subject
            {
                SubjectName = "математика",
                Value = 100, //часов
                SubjectType = st1
            };

            Subject sb2 = new Subject
            {
                SubjectName = "физика",
                Value = 80, //часов
                SubjectType = st1
            };

            Subject sb3 = new Subject
            {
                SubjectName = "информатика",
                Value = 120, //часов
                SubjectType = st2
            };

            Subject sb4 = new Subject
            {
                SubjectName = "базы данных",
                Value = 150, //часов
                SubjectType = st2
            };

            Subject sb5 = new Subject
            {
                SubjectName = "сетевые технологии",
                Value = 170, //часов
                SubjectType = st2
            };

            db.SubjectSet.Add(sb1);
            db.SubjectSet.Add(sb2);
            db.SubjectSet.Add(sb3);
            db.SubjectSet.Add(sb4);
            db.SubjectSet.Add(sb5);

            // Добавление групп

            StudentGroup g1 = new StudentGroup {
                GroupName = "ИУ5-11"
            };

            StudentGroup g2 = new StudentGroup {
                GroupName = "ИУ5-51"
            };

            StudentGroup g3 = new StudentGroup {
             GroupName = "ИУ5c-11",
            };

            db.StudentGroupSet.Add(g1);
            db.StudentGroupSet.Add(g2);
            db.StudentGroupSet.Add(g3);

            // Установка связи много-ко многим
            g1.Subjects.Add(sb1);
            g1.Subjects.Add(sb2);
            g2.Subjects.Add(sb3);
            g2.Subjects.Add(sb4);
            g2.Subjects.Add(sb5);
            g3.Subjects.Add(sb1);
            g3.Subjects.Add(sb2);
            g3.Subjects.Add(sb4);

            // Сохранение данных в БД
            db.SaveChanges();
        }

        static void Queries()   // Примеры запросов
        {
            Console.WriteLine("Executing queries...\n");

            WriteSubjectTypeTree(-1, 0);  // Выдача иерархии типов предметов
            Console.WriteLine();

            LearningModelContainer db = new LearningModelContainer_WithLazyLoad();

            //-------------------------------------------------------------------------


            Console.WriteLine("Получение всех курсов и групп, которым они читаются");
            var q1 = from s in db.SubjectSet select s;
            foreach (var s in q1)
            {
                Console.WriteLine(s.SubjectName + " (" + s.SubjectType.TypeName + ")");
                foreach (var g in s.StudentGroups)
                {
                    Console.WriteLine(" " + g.GroupName);
                }
            }

            //-------------------------------------------------------------------------

            Console.WriteLine("\nКоличество часов по всем предметам для каждой группы");
            var q2 = from g in db.StudentGroupSet
                     select new
                     {
                         GroupName = g.GroupName,
                         ValueSum = g.Subjects.Sum(x => x.Value),
                         Subject = g.Subjects
                     };

            foreach (var g in q2)
            {
                Console.WriteLine(g.GroupName + " (" + g.ValueSum.ToString() + " часов)");
                foreach (var s in g.Subject)
                {
                    Console.WriteLine(" " + s.SubjectName + " (" + s.Value.ToString() + " часов)");
                }
            }

            //-------------------------------------------------------------------------

            Console.WriteLine("\nПредметы, читаемые группам");

            var q3 = from g in db.StudentGroupSet
                     from s in g.Subjects
                     select new
                     {
                         SubjectName = s.SubjectName,
                         Value = s.Value,
                         GroupName = g.GroupName
                     };

            var q31 = from t in q3
                      orderby t.SubjectName, t.GroupName
                      select t;

            foreach (var g in q31)
            {
                Console.WriteLine(g);
            }

            //-------------------------------------------------------------------------

            Console.WriteLine("\nКоличество часов по предмету для всех групп");
            var q32 = from t in q3
                      group t by t.SubjectName into temp
                      select new
                      {
                          GroupName = temp.Key,
                          SumValue = temp.Sum(x => x.Value)
                      };

            foreach (var g in q32)
            {
                Console.WriteLine(g);
            }

            //-------------------------------------------------------------------------

            Console.WriteLine("\nГруппы для которых читаются специальные курсы (с использованием contains)");

            string[] arr = new string[] { "специальные", "другой" };

            var qc = from g in db.StudentGroupSet
                     from s in g.Subjects
                     where arr.Contains(s.SubjectType.TypeName)
                     select new
                     {
                         GroupName = g.GroupName,
                         SubjectTypeName = s.SubjectType.TypeName
                     };

            foreach (var g in qc)
            {
                Console.WriteLine(g);
            }

            //-------------------------------------------------------------------------

            Console.WriteLine("\nТипы курсов, читаемых для группы (с повторяющимися записями)");

            var q4 = from g in db.StudentGroupSet
                     from s in g.Subjects
                     select new
                     {
                         GroupName = g.GroupName,
                         SubjectTypeName = s.SubjectType.TypeName
                     };

            foreach (var g in q4)
            {
                Console.WriteLine(g);
            }

            //-------------------------------------------------------------------------

            Console.WriteLine("\nТипы курсов, читаемых для группы");
            var q41 = from t in q4.Distinct()
                      select t;
            foreach (var g in q41)
            {
                Console.WriteLine(g);
            }

            //-------------------------------------------------------------------------

            Console.WriteLine("\nГруппы, которым читаются базовые курсы");
            var q42 = from t in q4.Distinct()
                      where t.SubjectTypeName == "базовые"
                      select t;
            foreach (var g in q42)
            {
                Console.WriteLine(g);
            }

            //-------------------------------------------------------------------------

            Console.WriteLine("\nГруппы, которым читаются базовые курсы (использование any)");

            // we can't do such request as it cannot be converted into SQL due to
            // using .Any() method but we can decompose it into to separate parts:
            //   part 1 will be converted to SQL and executed on the server side
            //   part 2 will be executed on client side as LINQ to Objects
            //
            // var q43 = from t in q4.Distinct()
            //           group t by t.GroupName into temp
            //           where temp.Any(
            //              (data) => data.SubjectTypeName == "базовые")
            //           select temp.Key;

            // part 1; note using .AsEnumerable() method
            var q43_1 = q41.AsEnumerable();  

            // part 2
            var q43_2 = from t in q43_1
                        group t by t.GroupName into temp
                        where temp.Any(
                           (data) => data.SubjectTypeName == "базовые")
                        select temp.Key;

            foreach (var g in q43_2)
            {
                Console.WriteLine(g);
            }

            //-------------------------------------------------------------------------

            Console.WriteLine("\nГруппы, которым читаются только базовые курсы (использование all)");
            // в выборку будет входит только ИУ5-11 потому что группе ИУ5c-11 читают как
            // базовые так и специальные курсы (условие "all" не выполняется, сравните с "any"...)
            var q44_2 = from t in q41.AsEnumerable()
                        group t by t.GroupName into temp
                        where temp.All(
                           (data) => data.SubjectTypeName == "базовые")
                        select temp.Key;

            foreach (var g in q44_2)
            {
                Console.WriteLine(g);
            }

            Console.WriteLine("\nПолучение сгенерированного SQL");
            Console.WriteLine(q41.ToQueryString());
        }

        public static void IncludeExample()   
        {
            // Использованием Include вместо LazyLoad для загрузки связанных данных
            // NOTE: используется контекст данных где LazyLoad не включен (по умолчанию отключён)
            LearningModelContainer db = new LearningModelContainer();

            Console.WriteLine("\nБез использования Include (LazyLoad is not enabled)");
            var q51 = (from x in db.SubjectSet select x).ToList();
            WriteSubjectList(q51);

            Console.WriteLine("\nС использованием Include");
            var q52 = (from x in db.SubjectSet.Include("SubjectType")
                                              .Include("StudentGroups")
                       select x).ToList();
            WriteSubjectList(q52);
        }

        public static void WriteSubjectList(List<Subject> list)   // Вывод списка
        {
            foreach (var x in list)
            {
                string typeName = "";

                if (x.SubjectType != null)
                    typeName = x.SubjectType.TypeName;
                else
                    typeName = "null";

                Console.WriteLine(x.SubjectName + " (" + typeName + ")");

                if (x.StudentGroups != null)
                {
                    foreach (var y in x.StudentGroups)
                    {
                        Console.WriteLine(" " + y.GroupName);
                    }
                }
            }
        }

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
            LearningModelContainer db = new LearningModelContainer();

            db.Database.EnsureCreated();

            if (db.SubjectTypeSet.Count() != 0)
                ClearData();

            InitData();
            Queries();
            IncludeExample();

            Console.ReadLine();
        }
    }
}
