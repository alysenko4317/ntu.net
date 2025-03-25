using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ExpressionTreeUsage
{
    public partial class Form1 : Form
    {
        private BindingSource personBindingSource;

        public Form1()
        {
            InitializeComponent();
            InitializeBindingSource();
        }

        private void InitializeBindingSource()
        {
            // Ініціалізація BindingSource
            personBindingSource = new BindingSource();

            // Заповнення BindingSource колекцією даних
            personBindingSource.DataSource = new List<Person>
            {
                new Person { LastName = "2 Іванов", FirstName = "3 Іван", MiddleName = "Іванович", BirthDate = new DateTime(1980, 1, 1) },
                new Person { LastName = "1 Петров", FirstName = "2 Петро", MiddleName = "Петрович", BirthDate = new DateTime(1990, 2, 2) },
                new Person { LastName = "3 Сидоров", FirstName = "1 Олександр", MiddleName = "Олександрович", BirthDate = new DateTime(1985, 3, 15) }
            };

            // Прив'язка BindingSource до DataGridView
            dataGridView1.DataSource = personBindingSource;
        }
        /*
                private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
                {
                    // Отримуємо ім'я властивості, яка прив'язана до стовпця
                    string propertyName = dataGridView1.Columns[e.ColumnIndex].DataPropertyName;

                    // Отримуємо джерело даних з BindingSource та приводимо його до списку осіб
                    List<Person> people = (List<Person>) personBindingSource.DataSource;

                    // Сортуємо по імені, якщо клік був по стовпцю "FirstName"
                    if (propertyName == "FirstName") {
                        people = people.OrderBy(p => p.FirstName).ToList();
                    }

                    // Сортуємо по прізвищу, якщо клік був по стовпцю "LastName"
                    if (propertyName == "LastName") {
                        people = people.OrderBy(p => p.LastName).ToList();
                    }

                    // Оновлюємо джерело даних у BindingSource з відсортованим списком
                    personBindingSource.DataSource = people;
                }
        */
        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Отримуємо ім'я властивості, яка прив'язана до стовпця
            string propertyName = dataGridView1.Columns[e.ColumnIndex].DataPropertyName;

            // Отримуємо джерело даних з BindingSource та приводимо його до списку осіб
            List<Person> people = (List<Person>)personBindingSource.DataSource;

            // Цей код передбачає, що у вас є метод розширення OrderBy, який може приймати _ім'я властивості у вигляді рядка_
            people = people.OrderBy(propertyName).ToList();

            // Оновлюємо джерело даних у BindingSource з відсортованим списком
            personBindingSource.DataSource = people;
        }
    }

    // Клас Person, що представляє людину з полями для відображення
    public class Person
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
