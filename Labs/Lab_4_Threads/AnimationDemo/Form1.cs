using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace AnimationDemo
{
    public partial class Form1 : Form
    {
        Rectangle rect = new Rectangle(280, 110, 350, 100);

        private void MoveCircle()
        {
            Graphics g = this.CreateGraphics();  // створюємо контекст пристрою

            int x;  // координата x кола залежатиме від імені потоку
            if (Thread.CurrentThread.Name == "First")
                x = Width / 2 - 30;
            else
                x = Width / 2 + 30;

            // цикл від верхнього до нижнього краю форми
            for (int y = 10; y < Height - 40; y++)
            {
                g.FillEllipse(Brushes.Red, x - 10, y - 10, 20, 20);  // малюємо коло
                Thread.Sleep(30);  // "присипляємо" потік на 30 мілісекунд

                // якщо коло або його частина - усередині прямокутника,
                if (y + 10 > rect.Y && y - 10 < rect.Y + rect.Height)
                    g.FillRectangle(Brushes.Black, rect);  // то перемальовуємо прямокутник

                g.FillEllipse(SystemBrushes.Control, x - 10, y - 10, 20, 20);  // стираємо коло
            }

            // перевіряємо коректність завершення потоку
            MessageBox.Show("Поток " + Thread.CurrentThread.Name + " завершен!");
        }


        private void MoveCircleSync()
        {
            lock (this)  // встановлюємо блокування
            {
                Graphics g = this.CreateGraphics();  // створюємо контекст пристрою

                int x;  // координата x кола залежатиме від імені потоку
                if (Thread.CurrentThread.Name == "First")
                    x = Width / 2 - 30;
                else
                    x = Width / 2 + 30;

                // цикл від верхнього до нижнього краю форми
                for (int y = 10; y < Height - 40; y++)
                {
                    g.FillEllipse(Brushes.Red, x - 10, y - 10, 20, 20);  // малюємо коло
                    Thread.Sleep(30);  // "присипляємо" потік на 30 мілісекунд

                    // якщо коло або його частина - усередині прямокутника,
                    if (y + 10 > rect.Y && y - 10 < rect.Y + rect.Height)
                        g.FillRectangle(Brushes.Black, rect);  // то перемальовуємо прямокутник
                    else
                    {
                        Monitor.Pulse(this);  // дозволяємо виконання іншого потоку
                        Monitor.Wait(this);   // чекаємо на припинення/завершення іншого потоку
                    }
                    
                    g.FillEllipse(SystemBrushes.Control, x - 10, y - 10, 20, 20);  // стираємо коло
                }
                
                Monitor.Pulse(this);  // виводимо потік з режиму очікування
            }

            // перевіряємо коректність завершення потоку
            MessageBox.Show("Поток " + Thread.CurrentThread.Name + " завершен!");
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // створюємо перший потік (точка входу в нього - метод MoveCircle())
            Thread thread1 = new Thread(new ThreadStart(MoveCircleSync));
            thread1.Name = "First";  // присваиваем потоку имя

            // аналогічно для другого потоку
            Thread thread2 = new Thread(new ThreadStart(MoveCircleSync));
            thread2.Name = "Second";

            thread1.Start();  // запускаємо обидва потоки
            thread2.Start();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;  // отримуємо контекст пристрою
            g.FillRectangle(Brushes.Black, rect);  // малюємо прямокутник
        }
    }
}
