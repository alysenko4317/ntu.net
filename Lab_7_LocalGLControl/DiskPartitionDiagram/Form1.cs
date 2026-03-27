using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace DiskPartitionDiagram
{
    public partial class Form1 : Form
    {
        double ScreenW, ScreenH;  // розміри вікна
        private float[] partitionFillLevels = { 0.2f, 0.3f, 0.5f }; // Заповненість розділів

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Draw();
        }

        private void UpdatePartitionFillLevels()
        {
            // Get information about available drives
            DriveInfo[] drives = DriveInfo.GetDrives();

            // Initialize the array with the length of available drives
            partitionFillLevels = new float[drives.Length];

            // Calculate the fill levels based on the available space on each drive
            for (int i = 0; i < drives.Length; i++)
            {
                if (drives[i].IsReady)
                {
                    float fillLevel = 1f - (float)drives[i].AvailableFreeSpace / drives[i].TotalSize;
                    partitionFillLevels[i] = Math.Max(0f, Math.Min(1f, fillLevel));
                }
                else
                {
                    partitionFillLevels[i] = 0f; // Drive is not ready, set fill level to 0
                }
            }
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

            GL.MatrixMode(MatrixMode.Modelview);  // встановлення об'єктно-видової матриці

            UpdatePartitionFillLevels();
            timer1.Start();  // запуск таймера, що відповідає за виклик функції візуалізації сцени
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

        private void Draw()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Ortho(-1, 1, -1, 1, -1, 1);

            GL.Begin(PrimitiveType.Triangles);

            float startAngle = 0f;
            float centerX = 0f;
            float centerY = 0f;
            float radius = 0.8f;

            for (int i = 0; i < partitionFillLevels.Length; i++)
            {
                float sweepAngle = 360f * partitionFillLevels[i];
                float endAngle = startAngle + sweepAngle;

                int r = (int)(255 * partitionFillLevels[i]);
                int g = 255 - r;
                int b = 0;

                GL.Color4(Color.FromArgb(255, r, g, b));
                GL.Vertex2(centerX, centerY);

                for (float angle = startAngle; angle <= endAngle; angle += 1f)
                {
                    float x1 = centerX + radius * (float)Math.Cos(MathHelper.DegreesToRadians(angle));
                    float y1 = centerY + radius * (float)Math.Sin(MathHelper.DegreesToRadians(angle));
                    float x2 = centerX + radius * (float)Math.Cos(MathHelper.DegreesToRadians(angle + 1f));
                    float y2 = centerY + radius * (float)Math.Sin(MathHelper.DegreesToRadians(angle + 1f));

                    GL.Vertex2(centerX, centerY);
                    GL.Vertex2(x1, y1);
                    GL.Vertex2(x2, y2);
                }

                startAngle = endAngle;
            }

            GL.End();
            AnT.SwapBuffers();
        }
    }
}
