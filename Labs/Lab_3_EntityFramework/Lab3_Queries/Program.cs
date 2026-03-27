
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EntityLINQ {

public class Subject
{
    public int Id { get; set; }
    public string SubjectName { get; set; }
    public int Value { get; set; }
    public virtual SubjectType SubjectType { get; set; }  // many to one
    public virtual ICollection<StudentGroup> StudentGroups { get; set; }  // many to many
}

public class SubjectType
{
    public int Id { get; set; }
    public string TypeName { get; set; }
    public virtual SubjectType ParentSubjectType { get; set; }
}

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
        optionsBuilder.UseSqlServer(
            @"Server=(localdb)\mssqllocaldb;Database=schooldb5;Trusted_Connection=True;MultipleActiveResultSets=True");
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
    static void ClearData()   // видалення даних
    {
        Console.WriteLine("Clean the database: ");

        LearningModelContainer db = new LearningModelContainer();
        // видалення даних для зв'язку many-to-many
        // для кожного запису StudentGroup видаляються всі зв'язки з Subject
        Console.WriteLine("  -remove many-to-many links for Subject <-> StudentGroup");
        foreach (var gr in db.StudentGroupSet.ToList()) {
            foreach (var gr_subj in gr.Subjects.ToList())
                gr.Subjects.Remove(gr_subj);
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
        db.SubjectTypeSet.RemoveRange(db.SubjectTypeSet);
        db.SaveChanges();
    }

    static void InitData()   // створення тестових даних
    {
        Console.WriteLine("Initialize the database with sample data...");

        LearningModelContainer db = new LearningModelContainer();

        // додавання типів дисциплін
        SubjectType st_tech = new SubjectType
        {
            TypeName = "технічний цикл",
            ParentSubjectType = null
        };

        db.SubjectTypeSet.Add(st_tech);

        SubjectType st_hum = new SubjectType
        {
            TypeName = "гуманітарний цикл",
            ParentSubjectType = null
        };

        SubjectType st1 = new SubjectType
        {
            TypeName = "базові",
            ParentSubjectType = st_tech
        };

        SubjectType st2 = new SubjectType
        {
            TypeName = "спеціальні",
            ParentSubjectType = st_tech
        };

        SubjectType st3 = new SubjectType
        {
            TypeName = "історичні",
            ParentSubjectType = st_hum
        };

        SubjectType st3_1 = new SubjectType
        {
            TypeName = "нова історія",
            ParentSubjectType = st3
        };

        SubjectType st3_2 = new SubjectType
        {
            TypeName = "найновіша історія",
            ParentSubjectType = st3
        };

        db.SubjectTypeSet.Add(st_tech);
        db.SubjectTypeSet.Add(st_hum);
        db.SubjectTypeSet.Add(st1);
        db.SubjectTypeSet.Add(st2);
        db.SubjectTypeSet.Add(st3);
        db.SubjectTypeSet.Add(st3_1);
        db.SubjectTypeSet.Add(st3_2);

        // додавання дисциплін
        Subject sb1 = new Subject
        {
            SubjectName = "математика",
            Value = 100, // кількість годин викладання
            SubjectType = st1
        };

        Subject sb2 = new Subject
        {
            SubjectName = "фізика",
            Value = 80,
            SubjectType = st1
        };

        Subject sb3 = new Subject
        {
            SubjectName = "інформатика",
            Value = 120, 
            SubjectType = st2
        };

        Subject sb4 = new Subject
        {
            SubjectName = "бази даних",
            Value = 150,
            SubjectType = st2
        };

        Subject sb5 = new Subject
        {
            SubjectName = "мережеві технології",
            Value = 170,
            SubjectType = st2
        };

        db.SubjectSet.Add(sb1);
        db.SubjectSet.Add(sb2);
        db.SubjectSet.Add(sb3);
        db.SubjectSet.Add(sb4);
        db.SubjectSet.Add(sb5);

        // додавання груп

        StudentGroup g1 = new StudentGroup {
            GroupName = "ІУ5-11"
        };

        StudentGroup g2 = new StudentGroup {
            GroupName = "ІУ5-51"
        };

        StudentGroup g3 = new StudentGroup {
            GroupName = "ІУ5c-11",
        };

        db.StudentGroupSet.Add(g1);
        db.StudentGroupSet.Add(g2);
        db.StudentGroupSet.Add(g3);

        // встанровлення зв'язку many-to-many
        g1.Subjects.Add(sb1);
        g1.Subjects.Add(sb2);
        g2.Subjects.Add(sb3);
        g2.Subjects.Add(sb4);
        g2.Subjects.Add(sb5);
        g3.Subjects.Add(sb1);
        g3.Subjects.Add(sb2);
        g3.Subjects.Add(sb4);

        // запис даних у БД
        db.SaveChanges();
    }

    static void Queries()   // приклади запитів
    {
        Console.WriteLine("Виконання запитів...\n");

        WriteSubjectTypeTree(-1, 0);   // друк структури освітньої програми
        Console.WriteLine();

        LearningModelContainer db = new LearningModelContainer_WithLazyLoad();

        //-------------------------------------------------------------------------

        Console.WriteLine("Отримання всіх дисциплін та груп, яким вони читаються");
        var q1 = from s in db.SubjectSet select s;
        foreach (var s in q1) {
            Console.WriteLine(s.SubjectName + " (" + s.SubjectType.TypeName + ")");
            foreach (var g in s.StudentGroups)
                Console.WriteLine(" " + g.GroupName);
        }

        //-------------------------------------------------------------------------

        Console.WriteLine("\nКількість годин за всіма дисциплінами для кожної групи");
        var q2 = from g in db.StudentGroupSet
                    select new
                    {
                        GroupName = g.GroupName,
                        ValueSum = g.Subjects.Sum(x => x.Value),
                        Subject = g.Subjects
                    };

        foreach (var g in q2) {
            Console.WriteLine(g.GroupName + " (" + g.ValueSum.ToString() + " годин)");
            foreach (var s in g.Subject)
                Console.WriteLine(" " + s.SubjectName + " (" + s.Value.ToString() + " годин)");
        }

        //-------------------------------------------------------------------------

        Console.WriteLine("\nДисципліни, які читаються групам");

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
            Console.WriteLine(g);

        //-------------------------------------------------------------------------

        Console.WriteLine("\nКількість годин за дисципліною для всіх груп");
        var q32 = from t in q3
                    group t by t.SubjectName into temp
                    select new
                    {
                        GroupName = temp.Key,
                        SumValue = temp.Sum(x => x.Value)
                    };

        foreach (var g in q32)
            Console.WriteLine(g);

        //-------------------------------------------------------------------------

        Console.WriteLine("\nГрупи, для яких читаються спеціальні дисципліни (з використанням contains)");
        string[] arr = new string[] { "спеціальна", "інша" };
        var qc = from g in db.StudentGroupSet
                    from s in g.Subjects
                    where arr.Contains(s.SubjectType.TypeName)
                    select new
                    {
                        GroupName = g.GroupName,
                        SubjectTypeName = s.SubjectType.TypeName
                    };

        foreach (var g in qc)
            Console.WriteLine(g);

        //-------------------------------------------------------------------------

        Console.WriteLine("\nТипи дисциплін, які читаються для групи (без видалення дублів)");
        var q4 = from g in db.StudentGroupSet
                    from s in g.Subjects
                    select new
                    {
                        GroupName = g.GroupName,
                        SubjectTypeName = s.SubjectType.TypeName
                    };

        foreach (var g in q4)
            Console.WriteLine(g);

        //-------------------------------------------------------------------------

        Console.WriteLine("\nТипи дисциплін, які читаються для групи");
        var q41 = from t in q4.Distinct()
                    select t;
        foreach (var g in q41)
            Console.WriteLine(g);

        //-------------------------------------------------------------------------

        Console.WriteLine("\nГрупи, яким читаються базові дисципліни");
        var q42 = from t in q4.Distinct()
                    where t.SubjectTypeName == "базові"
                    select t;
        foreach (var g in q42)
            Console.WriteLine(g);

        //-------------------------------------------------------------------------

        Console.WriteLine("\nГрупи, яким читаються базові дисципліни (з використанням any)");

        // we can't do such request as it cannot be converted into SQL due to
        // using .Any() method but we can decompose it into to separate parts:
        //   part 1 will be converted to SQL and executed on the server side
        //   part 2 will be executed on client side as LINQ to Objects
        //
        // var q43 = from t in q4.Distinct()
        //           group t by t.GroupName into temp
        //           where temp.Any(
        //              (data) => data.SubjectTypeName == "базові")
        //           select temp.Key;

        // part 1; note using .AsEnumerable() method
        var q43_1 = q41.AsEnumerable();  

        // part 2
        var q43_2 = from t in q43_1
                    group t by t.GroupName into temp
                    where temp.Any(
                        (data) => data.SubjectTypeName == "базові")
                    select temp.Key;

        foreach (var g in q43_2)
            Console.WriteLine(g);

        //-------------------------------------------------------------------------

        Console.WriteLine("\nГрупи, яким читаються тільки базові дисципліни (використання all)");
        // до вибірки потрапить лише група ІУ5-11, оскільки групі ІУ5с-11 читають як базові,
        // так і спеціальні курси (умова "all" не виконується; порівняйте результат з "any"...)"
        var q44_2 = from t in q41.AsEnumerable()
                group t by t.GroupName into temp
                where temp.All(
                    (data) => data.SubjectTypeName == "базові")
                select temp.Key;

        foreach (var g in q44_2)
            Console.WriteLine(g);

        Console.WriteLine("\nОтримання згенерованого SQL");
        Console.WriteLine(q41.ToQueryString());
    }

    public static void IncludeExample()   
    {
        // Використання Include замість LazyLoad для завантаження пов'язаних даних
        // NOTE: використовується контекст даних де LazyLoad не включено (за замовчанням відключено)
        LearningModelContainer db = new LearningModelContainer();

        Console.WriteLine("\nБез використання Include (LazyLoad is not enabled)");
        var q51 = (from x in db.SubjectSet select x).ToList();
        WriteSubjectList(q51);

        Console.WriteLine("\nЗ використанням Include");
        var q52 = (from x in db.SubjectSet.Include("SubjectType")
                                          .Include("StudentGroups")
                   select x).ToList();
        WriteSubjectList(q52);
    }

    public static void WriteSubjectList(List<Subject> list)   // друк переліку
    {
        foreach (var x in list) {
            string typeName = "";
            if (x.SubjectType != null)
                typeName = x.SubjectType.TypeName;
            else
                typeName = "null";
            Console.WriteLine(x.SubjectName + " (" + typeName + ")");
            if (x.StudentGroups != null) {
                foreach (var y in x.StudentGroups)
                    Console.WriteLine(" " + y.GroupName);
            }
        }
    }

    public static void WriteSubjectTypeTree(int ParentSubjectTypeParam, int LevelParam)
    {
        LearningModelContainer db = new LearningModelContainer();

        var q = from x in db.SubjectTypeSet select x;

        if (ParentSubjectTypeParam == -1) {
            // пошук кореневих елементів (ParentSubjectType == null)
            q = q.Where(x => x.ParentSubjectType == null);
        }
        else {
            // пошук елементів з заданим елементом верхнього рівня
            q = q.Where(x => x.ParentSubjectType.Id == ParentSubjectTypeParam);
        }

        // якщо існують елементи на даному рівні ієрархіі
        if (q.Count() > 0) {
            q = q.OrderBy(x => x.TypeName);  // сортування

            // перелік усіх значень на заданому рівні ієрархії
            foreach (var x in q)
            {
                // друк відступа ліворуч
                if (LevelParam > 0) {
                    for (int i = 0; i < LevelParam; i++)
                        Console.Write("   ");
                    Console.Write("|--");
                }

                Console.WriteLine(x.TypeName);  // друк назви дисципліни

                // рекурсивний виклик функції для всіх елементів, вкладених у поточний рівень ієрархіі
                WriteSubjectTypeTree(x.Id, LevelParam + 1);
            }
        }
    }

    static void Main(string[] args)
    {
        LearningModelContainer db = new LearningModelContainer();

        if (db.SubjectTypeSet.Count() != 0)
            ClearData();

        InitData();
        Queries();
        IncludeExample();

        Console.ReadLine();
    }
}
}
