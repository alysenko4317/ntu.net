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
    public partial class Preview : Form
    {
        Image _toView;   // об'єкт Image для зберігання зображення

        // модифікуємо конструктор класу таким чином, щоб він отримував
        // як параметр зображення для відображення
        public Preview(Image view)
        {
            _toView = view;
            InitializeComponent();
        }

        private void Preview_Load(object sender, EventArgs e)
        {
            // якщо зображення завантажено 
            if (_toView != null)
            {
                // встановлюємо нові розміри елемента PictureBox1, які дорівнюють
                // ширині та висоті завантажуваного зображення. 
                pictureBox1.Size = new Size(_toView.Width, _toView.Height);
                // встановлюємо зображення для відображення в елементі pictureBox1 
                pictureBox1.Image = _toView;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
