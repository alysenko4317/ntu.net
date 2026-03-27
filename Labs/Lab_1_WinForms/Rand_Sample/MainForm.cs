using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinF_Sample
{
    public partial class MainForm : Form
    {
        int _w = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
        int _h = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;

        public MainForm()
        {
            InitializeComponent();
        }

private void MainForm_MouseMove(object sender, MouseEventArgs e)
{
    Random rnd = new Random();

    textBox1.Text = e.X.ToString()
        + " [" + button1.Bounds.X.ToString()
        + "-" + (button1.Bounds.X + button1.Bounds.Width).ToString() + "]";

    textBox2.Text = e.Y.ToString()
        + " [" + button1.Bounds.Y.ToString()
        + "-" + (button1.Bounds.Y + button1.Bounds.Height).ToString() + "]";

    const int bound = 10;

    // якщо координата по осі X та по осі Y лежить біля кнопки "Так, звісно!"
    if (e.X > button1.Bounds.X - bound
            && e.X < button1.Bounds.X + button1.Bounds.Width + bound
            && e.Y > button1.Bounds.Y - bound
            && e.Y < button1.Bounds.Y + button1.Bounds.Height + bound)
    {
        Point tmp_location = this.Location;  // запам'ятовуємо поточне положення вікна

        // генеруємо переміщення по осях X та Y і додаємо їх до збереженого значення
        // поточної позиціі вікна; числа генеруються у діапазоні від -100 до 100
        tmp_location.X += rnd.Next(-100, 100);
        tmp_location.Y += rnd.Next(-100, 100);

        // якщо вікно потрапило за межі екрана однією з осей
        if (tmp_location.X < 0 || tmp_location.X > (_w - this.Width / 2)
                || tmp_location.Y < 0 || tmp_location.Y > (_h - this.Height / 2))
        {
            tmp_location.X = _w / 2;   // новими координатами стане центр вікна
            tmp_location.Y = _h / 2;
        }

        this.Location = tmp_location;  // оновлюємо положення вікна, на нове згенероване
    }
}

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ви ретельні!");
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Вивести повідомлення, з текстом "Ми не сумнівалися у вашій байдужості!"
            // Другий параметр - заголовок вікна повідомлення: "Увага"
            // MessageBoxButtons.OK - тип кнопки, що розміщується на формі повідомлення
            // MessageBoxIcon.Information - тип повідомлення
            //   матиме іконку "інформація" та відповідний звуковий сигнал
            MessageBox.Show("Ми не сумнівалися у вашій байдужості!",
                            "Увага",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }
    }
}
