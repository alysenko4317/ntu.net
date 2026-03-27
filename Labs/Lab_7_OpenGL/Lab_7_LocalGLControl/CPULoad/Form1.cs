using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;

namespace CPULoad
{
    public partial class Form1 : Form
    {
        private float[] cpuLoadData; // Масив для збереження даних про навантаження CPU
        private int dataIndex = 0;
        private const int MaxDataPoints = 100;

        public Form1()
        {
            InitializeComponent();
            //AnT.Load += AnT_Load;
            //AnT.Paint += AnT_Paint;
            cpuLoadData = new float[MaxDataPoints];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color.White);
            GL.Viewport(0, 0, AnT.Width, AnT.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, MaxDataPoints, 0, 100, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);

            timer1.Start();
        }

        private void UpdateCPULoadData_()
        {
            // Replace this with actual CPU load data retrieval logic
            Random random = new Random();
            float newLoad = random.Next(0, 101);

            cpuLoadData[dataIndex] = newLoad;
            dataIndex = (dataIndex + 1) % MaxDataPoints;
        }

        private void UpdateCPULoadData()
        {
            // Отримуємо інформацію про навантаження на CPU
            float cpuLoad = GetCPULoad();

            // Зберігаємо дані у масив
            cpuLoadData[dataIndex] = cpuLoad;
            dataIndex = (dataIndex + 1) % MaxDataPoints;
        }

        private float GetCPULoad()
        {
            // Отримуємо інформацію про навантаження на CPU
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();
            System.Threading.Thread.Sleep(1000); // Затримка для оновлення значень
            float cpuLoad = cpuCounter.NextValue();
            return cpuLoad;
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
            GL.LoadIdentity();

            SetAntiAliasing();
            DrawCPULoadGraph();

            AnT.SwapBuffers();
        }

        private void DrawCPULoadGraph()
        {
            GL.Begin(PrimitiveType.LineStrip);

            for (int i = 0; i < MaxDataPoints; i++)
            {
                float x = i;
                float y = cpuLoadData[(dataIndex + i) % MaxDataPoints];
                GL.Color3(Color.Blue);
                GL.Vertex2(x, y);
            }

            GL.End();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateCPULoadData();
            Draw();
        }
    }
}
