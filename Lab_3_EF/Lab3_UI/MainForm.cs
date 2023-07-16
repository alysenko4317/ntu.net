using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab2_App
{
    public partial class MainForm : Form
    {
        // Колонки таблиці
        private DataGridViewColumn dataGridViewColumn1 = null;
        private DataGridViewColumn dataGridViewColumn2 = null;
        private DataGridViewColumn dataGridViewColumn3 = null;

        // Колекція List
        private IList<Student> studentList = new List<Student>();

        public MainForm()
        {
            InitializeComponent();
            initDataGridView();
        }

        private void initDataGridView()   
        {
            // інициализация таблиці
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Add(getDataGridViewColumn1());
            dataGridView1.Columns.Add(getDataGridViewColumn2());
            dataGridView1.Columns.Add(getDataGridViewColumn3());
            dataGridView1.AutoResizeColumns();
        }

        private DataGridViewColumn getDataGridViewColumn1()
        {
            // динамічне створення першої колонки у таблиці
            if (dataGridViewColumn1 == null) {
                dataGridViewColumn1 = new DataGridViewTextBoxColumn();
                dataGridViewColumn1.Name = "";
                dataGridViewColumn1.HeaderText = "Имя";
                dataGridViewColumn1.ValueType = typeof(string);
                dataGridViewColumn1.Width = dataGridView1.Width / 3;
            }

            return dataGridViewColumn1;
        }

        private DataGridViewColumn getDataGridViewColumn2()
        {
            // динамічне створення другої колонки у таблиці
            if (dataGridViewColumn2 == null) {
                dataGridViewColumn2 = new DataGridViewTextBoxColumn();
                dataGridViewColumn2.Name = "";
                dataGridViewColumn2.HeaderText = "Фамилия";
                dataGridViewColumn2.ValueType = typeof(string);
                dataGridViewColumn2.Width = dataGridView1.Width / 3;
            }

            return dataGridViewColumn2;
        }

        private DataGridViewColumn getDataGridViewColumn3()
        {
            // динамічне створення третьої колонки у таблиці
            if (dataGridViewColumn3 == null) {
                dataGridViewColumn3 = new DataGridViewTextBoxColumn();
                dataGridViewColumn3.Name = "";
                dataGridViewColumn3.HeaderText = "Зачетка";
                dataGridViewColumn3.ValueType = typeof(string);
                dataGridViewColumn3.Width = dataGridView1.Width / 3;
            }

            return dataGridViewColumn3;
        }

        
        private void addStudent(string name, string surname, string recordBookNumber)
        {
            // додавання студента до колекції
            Student s = new Student(name, surname, recordBookNumber);
            studentList.Add(s);
            textBox1.Text = "";
            textBox2.Text = "";
            textBox2.Text = "";
            showListInGrid();
        }

        private void deleteStudent(int elementIndex)
        {
            // видалення студента з колекції
            studentList.RemoveAt(elementIndex);
            showListInGrid();
        }

        private void showListInGrid()
        {
            // відображення колекції у таблиці
            dataGridView1.Rows.Clear();
            foreach (Student s in studentList) {
                DataGridViewRow row = new DataGridViewRow();
                DataGridViewTextBoxCell cell1 = new
                DataGridViewTextBoxCell();
                DataGridViewTextBoxCell cell2 = new
                DataGridViewTextBoxCell();
                DataGridViewTextBoxCell cell3 = new
                DataGridViewTextBoxCell();
                cell1.ValueType = typeof(string);
                cell1.Value = s.getName();
                cell2.ValueType = typeof(string);
                cell2.Value = s.getSurname();
                cell3.ValueType = typeof(string);
                cell3.Value = s.getRecordBookNumber();
                row.Cells.Add(cell1);
                row.Cells.Add(cell2);
                row.Cells.Add(cell3);
                dataGridView1.Rows.Add(row);
            }
        }

        private void addStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // обробник натискання на кнопку додавання
            addStudent(textBox1.Text, textBox2.Text, textBox3.Text);
        }

        private void deleteStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // обробник натискання на видалити
            int selectedRow = dataGridView1.SelectedCells[0].RowIndex;
            DialogResult dr = MessageBox.Show("Видалити студента?", "", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes) {
                try {
                    deleteStudent(selectedRow);
                }
                catch (Exception) { }
            }
        }
    }
}
