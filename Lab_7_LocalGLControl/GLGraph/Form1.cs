using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GLGraph
{
    public partial class Form1 : Form
    {
        double ScreenW, ScreenH;  // розміри вікна
        private float devX;       // відношення сторін вікна візуалізації для коректного перетворення
        private float devY;       //    координат миші в координати, прийняті у програмі
        private float[,] GrapValuesArray;  // масив, який буде зберігати значення x та y точок графіка
        private int elements_count = 0;    // кількість елементів у масиві
        private bool not_calculate = true; // флаг, який позначає, що масив із значеннями координат
                                           //    графіка поки що не заповнено
        private int pointPosition = 0;     // номер ячейки масиву, з якої будуть взяті координати
                                           //    для червоної точки, для візуалізації поточного кадру
        float lineX, lineY;  // допоміжні змінні для побудови ліній від курсора миші до координатних осей
        float Mcoord_X = 0, Mcoord_Y = 0;  // поточні координати курсора миші

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color.White);  // встановлення кольору очищення екрану (RGBA)
            GL.Viewport(0, 0, AnT.Width, AnT.Height);  // встановлення порту виводу
            GL.MatrixMode(MatrixMode.Projection);  // активація проекційної матриці
            GL.LoadIdentity();  // очищення матриці

            // визначення параметрів налаштування проекції в залежності від розмірів сторін елемента AnT.
            if ((float)AnT.Width <= (float)AnT.Height)
            {
                ScreenW = 30.0;
                ScreenH = 30.0 * (float)AnT.Height / (float)AnT.Width;
                GL.Ortho(0.0, ScreenW, 0.0, ScreenH, -1, 1);
            }
            else
            {
                ScreenW = 30.0 * (float)AnT.Width / (float)AnT.Height;
                ScreenH = 30.0;
                GL.Ortho(0.0, 30.0 * (float)AnT.Width / (float)AnT.Height, 0.0, 30.0, -1, 1);
            }

            // збереження коефіцієнтів, які нам необхідні для перетворення координат вказівника
            // в віконній системі до координат, прийнятих у нашій OpenGL сцені
            devX = (float)ScreenW / (float)AnT.Width;
            devY = (float)ScreenH / (float)AnT.Height;

            GL.MatrixMode(MatrixMode.Modelview);  // встановлення об'єктно-видової матриці
            PointInGraph.Start();  // запуск таймера, що відповідає за виклик функції візуалізації сцени
        }

        private void PointInGraph_Tick(object sender, EventArgs e)
        {
            // якщо ми досягли останнього елемента масиву
            if (pointPosition == elements_count - 1)
                pointPosition = 0;  // переходимо до початкового елемента

            Draw();  // функція візуалізації

            // перехід до наступного елемента масиву
            pointPosition++;
        }

        private void AnT_MouseMove(object sender, MouseEventArgs e)
        {
            Mcoord_X = e.X;   // зберігаємо координати миші
            Mcoord_Y = e.Y;

            // обчислюємо параметри для майбутнього дорисування ліній від
            // вказівника миші до координатних осей
            lineX = devX * e.X;
            lineY = (float)(ScreenH - devY * e.Y);
        }

        // функція, яка обчислює значення координат графіка функції і записує їх в масив GrapValuesArray
        private void functionCalculation()
        {
            float x = 0, y = 0;

            // ініціалізація масиву, який буде зберігати значення 300 точок,
            // значення яких будуть визначати графік функції
            GrapValuesArray = new float[300, 2];

            elements_count = 0;  // лічильник елементів масиву

            // обчислення всіх значень y для x, що належить проміжку від -15 до 15 з кроком 0.01f
            for (x = -15; x < 15; x += 0.1f)
            {
                // обчислення y для поточного x
                // цей рядок задає формулу, що описує графік функції для нашого рівняння y = f(x)
                y = (float)Math.Sin(x) * 3 + 1;
                GrapValuesArray[elements_count, 0] = x;  // запис координати x
                GrapValuesArray[elements_count, 1] = y;  // запис координати y
                elements_count++;  // підрахунок елементів
            }

            // змінюємо прапорець, який сигналізував про те, що всі координати,
            // що визначають графік функції, обчислені
            not_calculate = false;
        }

        private void DrawDiagram()   // візуалізація графіка
        {
            // перевірка прапорця, який сигналізує про те, що координати графіка обчислені
            if (not_calculate)
                functionCalculation();  // якщо ні, то викликаємо функцію обчислення координат

            // починаємо малювати у режимі відображення точок, які об'єднані в лінії (GL_LINE_STRIP)
            GL.Begin(PrimitiveType.LineStrip);

            // малюємо початкову точку
            GL.Vertex2(GrapValuesArray[0, 0], GrapValuesArray[0, 1]);

            // проходимо по масиву з координатами обчислених точок
            for (int ax = 1; ax < elements_count; ax += 2)
            {
                // передаємо у OpenGL інформацію про вершину, яка бере участь у побудові ліній
                GL.Vertex2(GrapValuesArray[ax, 0], GrapValuesArray[ax, 1]);
            }

            GL.End();  // завершуємо поточний режим малювання (GL_LINE_STRIP)

            GL.PointSize(5);  // встановлюємо розмір точок, рівний 5 пікселям
            GL.Color3(Color.Red);  // встановлюємо поточний колір - червоний
            GL.Begin(PrimitiveType.Points);  // активуємо режим виведення точок (GL_POINTS)
                                             // виводимо червону точку, використовуючи ту комірку масиву, до якої ми досягли (визначається в обробнику подій таймера)
            GL.Vertex2(GrapValuesArray[pointPosition, 0], GrapValuesArray[pointPosition, 1]);
            GL.End();  // завершуємо режим малювання (GL_POINTS)
            GL.PointSize(1);  // встановлюємо розмір точок рівний одиниці
        }

        private void SetAntiAliasing()
        {
            GL.Enable(EnableCap.Multisample);  // Включити режим багатозразкового антиаліасингу
            GL.Enable(EnableCap.Blend);  // Включити змішування кольорів
            GL.Enable(EnableCap.LineSmooth);  // Включити сглажування ліній
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);  // Встановити найвищий рівень якості сглажування ліній

            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);  // Налаштування блендування для прозорості
            GL.Enable(EnableCap.AlphaTest);  // Включити тест на альфа-канал (для коректного блендування)
        }

        private void Draw()  // функція, яка керує візуалізацією сцени
        {
            GL.ClearColor(Color.Bisque);

            // очищення буфера кольору і буфера глибини
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.LoadIdentity();  // очищення поточної матриці
            GL.Color3(Color.Black);  // встановлення чорного кольору
            GL.PushMatrix();  // поміщаємо стан матриці в стек матриць
            GL.Translate(20, 15, 0);  // виконуємо переміщення в просторі по осях X і Y
            GL.Begin(PrimitiveType.Points);  // активуємо режим малювання GL_POINTS

            // з допомогою вкладених циклів, створюємо сітку з точок
            for (int ax = -15; ax < 15; ax++)
                for (int bx = -15; bx < 15; bx++)
                    GL.Vertex2(ax, bx);  // виведення точки

            GL.End();  // завершення режиму малювання примітивів

            // активуємо режим малювання GL_LINES,
            //   кожні 2 послідовно викликані команди glVertex з'єднуються в лінії
            GL.Begin(PrimitiveType.Lines);

            // далі ми малюємо координатні осі та стрілки на їхніх кінцях
            GL.Vertex2(0, -15);
            GL.Vertex2(0, 15);
            GL.Vertex2(-15, 0);
            GL.Vertex2(15, 0);

            GL.Vertex2(0, 15);      // вертикальна стрілка
            GL.Vertex2(0.1, 14.5);
            GL.Vertex2(0, 15);
            GL.Vertex2(-0.1, 14.5);

            GL.Vertex2(15, 0);      // горизонтальна стрілка
            GL.Vertex2(14.5, 0.1);
            GL.Vertex2(15, 0);
            GL.Vertex2(14.5, -0.1);

            GL.End();  // завершуємо режим малювання GL_LINES

            SetAntiAliasing();
            DrawDiagram();  // викликаємо функцію малювання графіка

            GL.PopMatrix();  // повертаємо матрицю зі стеку
            GL.Color3(Color.Red);  // встановлюємо червоний колір

            // включаємо режим малювання ліній, для того щоб намалювати
            // лінії від вказівника миші до координатних осей
            GL.Begin(PrimitiveType.Lines);

            GL.Vertex2(lineX, 15);
            GL.Vertex2(lineX, lineY);
            GL.Vertex2(20, lineY);
            GL.Vertex2(lineX, lineY);

            GL.End();

            AnT.SwapBuffers();  // перемикаємо буфер виведення
        }
    }
}
