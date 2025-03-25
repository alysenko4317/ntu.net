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
            // ����������� BindingSource
            personBindingSource = new BindingSource();

            // ���������� BindingSource ��������� �����
            personBindingSource.DataSource = new List<Person>
            {
                new Person { LastName = "2 ������", FirstName = "3 ����", MiddleName = "��������", BirthDate = new DateTime(1980, 1, 1) },
                new Person { LastName = "1 ������", FirstName = "2 �����", MiddleName = "��������", BirthDate = new DateTime(1990, 2, 2) },
                new Person { LastName = "3 �������", FirstName = "1 ���������", MiddleName = "�������������", BirthDate = new DateTime(1985, 3, 15) }
            };

            // ����'���� BindingSource �� DataGridView
            dataGridView1.DataSource = personBindingSource;
        }
        /*
                private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
                {
                    // �������� ��'� ����������, ��� ����'����� �� �������
                    string propertyName = dataGridView1.Columns[e.ColumnIndex].DataPropertyName;

                    // �������� ������� ����� � BindingSource �� ��������� ���� �� ������ ���
                    List<Person> people = (List<Person>) personBindingSource.DataSource;

                    // ������� �� ����, ���� ��� ��� �� ������� "FirstName"
                    if (propertyName == "FirstName") {
                        people = people.OrderBy(p => p.FirstName).ToList();
                    }

                    // ������� �� �������, ���� ��� ��� �� ������� "LastName"
                    if (propertyName == "LastName") {
                        people = people.OrderBy(p => p.LastName).ToList();
                    }

                    // ��������� ������� ����� � BindingSource � ������������ �������
                    personBindingSource.DataSource = people;
                }
        */
        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // �������� ��'� ����������, ��� ����'����� �� �������
            string propertyName = dataGridView1.Columns[e.ColumnIndex].DataPropertyName;

            // �������� ������� ����� � BindingSource �� ��������� ���� �� ������ ���
            List<Person> people = (List<Person>)personBindingSource.DataSource;

            // ��� ��� ���������, �� � ��� � ����� ���������� OrderBy, ���� ���� �������� _��'� ���������� � ������ �����_
            people = people.OrderBy(propertyName).ToList();

            // ��������� ������� ����� � BindingSource � ������������ �������
            personBindingSource.DataSource = people;
        }
    }

    // ���� Person, �� ����������� ������ � ������ ��� �����������
    public class Person
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
