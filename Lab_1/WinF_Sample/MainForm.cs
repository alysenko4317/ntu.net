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
        Image _image;

        public MainForm()
        {
            InitializeComponent();
        }

        private void loadImage(bool jpg)
        {
            // папка, яка буде обрана як початкова у вікні для вибору файлу 
            openFileDialog1.InitialDirectory = ".";

            // якщо будемо обирати jpg-файли 
            if (jpg) {
                // встановлюємо формат файлів для завантаження - jpg 
                openFileDialog1.Filter = "image (JPEG) files (*.jpg)|*.jpg|All files (*.*)|*.*";
            }
            else {
                // встановлюємо формат файлів для завантаження – png 
                openFileDialog1.Filter = "image (PNG) files (*.png)|*.png|All files (*.*)|*.*";
            }

            // якщо відкриття вікна вибору файлу завершилося вибором файлу та натисканням кнопки ОК 
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try {
                    // намагаємося завантажити файл з ім'ям FileName – обраний користувачем файл. 
                    _image = Image.FromFile(openFileDialog1.FileName);
                    // устанавливаем картинку в поле элемента PictureBox 
                    pictureBoxControl.Image = _image;
                }
                catch (Exception ex) {
                    // показуємо повідомлення з причиною помилки 
                    MessageBox.Show("Не удалось загрузить файл: " + ex.Message);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // создаем переменную rsl, которая будет хранить результат выбора пользователя
            // в окне с вопросом; пользователь нажал одну из клавиш на окне - это и есть результат 
            // MessageBox будет содержать вопрос, а также кнопки Yes, No и иконку Question (Вопрос) 
            DialogResult rsl = MessageBox.Show(
                "Вы действительно хотите выйти из приложения?",
                "Внимание!",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (rsl == DialogResult.Yes)
                Application.Exit();
        }

        private void onLoadJPG_Click(object sender, EventArgs e) {
            loadImage(true);
        }

        private void onLoadPNG_Click(object sender, EventArgs e) {
            loadImage(false);
        }

        private void onOpenPreview_Click(object sender, EventArgs e)
        {
            // створюємо новий екземпляр класу Preview, який відповідає за роботу
            // з нашою додатковою формою; як параметр ми передаємо наше завантажене зображення 
            Form PreView = new Preview(_image);
            PreView.ShowDialog();  // відображуємо форму
        }
    }
}
