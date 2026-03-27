using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    internal class dbinit
    {
        public static void InitData()   // Заповнення даних
        {
            Console.WriteLine("Initialize the database with sample data...");

            LearningModelContainer db = new LearningModelContainer();

            // Додавання типів предметів
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
                TypeName = "новітня історія",
                ParentSubjectType = st3
            };

            db.SubjectTypeSet.Add(st_tech);
            db.SubjectTypeSet.Add(st_hum);
            db.SubjectTypeSet.Add(st1);
            db.SubjectTypeSet.Add(st2);
            db.SubjectTypeSet.Add(st3);
            db.SubjectTypeSet.Add(st3_1);
            db.SubjectTypeSet.Add(st3_2);

            // Додавання предметів
            Subject sb1 = new Subject
            {
                SubjectName = "математика",
                Value = 100, //годин
                SubjectType = st1
            };

            Subject sb2 = new Subject
            {
                SubjectName = "фізика",
                Value = 80, //годин
                SubjectType = st1
            };

            Subject sb3 = new Subject
            {
                SubjectName = "інформатика",
                Value = 120, //годин
                SubjectType = st2
            };

            Subject sb4 = new Subject
            {
                SubjectName = "бази даних",
                Value = 150, //годин
                SubjectType = st2
            };

            Subject sb5 = new Subject
            {
                SubjectName = "мережеві технології",
                Value = 170, //годин
                SubjectType = st2
            };

            db.SubjectSet.Add(sb1);
            db.SubjectSet.Add(sb2);
            db.SubjectSet.Add(sb3);
            db.SubjectSet.Add(sb4);
            db.SubjectSet.Add(sb5);

            // Додавання груп

            StudentGroup g1 = new StudentGroup
            {
                GroupName = "ИУ5-11"
            };

            StudentGroup g2 = new StudentGroup
            {
                GroupName = "ИУ5-51"
            };

            StudentGroup g3 = new StudentGroup
            {
                GroupName = "ИУ5c-11",
            };

            db.StudentGroupSet.Add(g1);
            db.StudentGroupSet.Add(g2);
            db.StudentGroupSet.Add(g3);

            // Встановлення зв'язку багато-до-багатьох
            g1.Subjects.Add(sb1);
            g1.Subjects.Add(sb2);
            g2.Subjects.Add(sb3);
            g2.Subjects.Add(sb4);
            g2.Subjects.Add(sb5);
            g3.Subjects.Add(sb1);
            g3.Subjects.Add(sb2);
            g3.Subjects.Add(sb4);

            // Збереження даних у БД
            db.SaveChanges();
        }
    }
}
