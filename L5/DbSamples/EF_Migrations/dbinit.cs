using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLINQ
{
    internal class dbinit
    {
        public static void InitData()   // Заполнение данных
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
    }
}
