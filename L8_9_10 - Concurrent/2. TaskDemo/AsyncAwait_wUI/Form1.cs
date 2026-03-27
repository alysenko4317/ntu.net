using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncAwait_wUI
{
    public partial class Form1 : Form
    {
        public Form1() {
            InitializeComponent();
        }

        // --------------------------------------------------------
        // sample 1 - no additional threads
        // --------------------------------------------------------

        private double getResult(double x)
        {
            Thread.Sleep(1000);
            Task.Delay(1000).Wait();
            return Math.Sin(x) + Math.Cos(x);
        }

        private void button1_Click(object sender, EventArgs e) {
            textBox1.Text = getResult(5).ToString();
        }

        // --------------------------------------------------------
        // sample 2 - task based implementation
        // --------------------------------------------------------

        private Task<double> getResultTask(double x)
        {
            Task<double> task = new Task<double>(() =>
            {
                Thread.Sleep(1000);
                Task.Delay(1000).Wait();

                double res = Math.Sin(x) + Math.Cos(x);
                return res;
            });

            task.Start();

            return task;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Task<double> task = getResultTask(5);

            double result = task.Result; // This will block until the task completes

            textBox2.Text = result.ToString();
        }

        // --------------------------------------------------------
        // sample 3 - non blocking task based implementation
        // --------------------------------------------------------

        private Task<double> getResultTask(double x, Action<Task<double>> action)
        {
            Task<double> task = new Task<double>(() =>
            {
                Thread.Sleep(1000);
                Task.Delay(1000).Wait();

                double res = Math.Sin(x) + Math.Cos(x);
                return res;
            });

            task.ContinueWith(action);
            task.Start();

            return task;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            const double x = 5;

            Task<double> task = getResultTask(x, (t) =>
            {
                this.Invoke(new Action(() =>
                {
                    textBox3.Text = "result=" + t.Result.ToString();
                }));
            });
        }

        // --------------------------------------------------------
        // sample 4 - async/await based implementation
        // --------------------------------------------------------

        private double Solve(double x)
        {
            Thread.Sleep((int)x * 500);
         //   Task.Delay((int)x * 500).Wait();
            return Math.Sin(x) + Math.Cos(x);
        }

        // асинхронний метод має повертати Task! (або void, але так робити не рекомендовано)
        private async Task<double> getResultAsync(double x)
        {
            Task<double> task = new Task<double>(() =>
            {
                return Solve(x);
            });

            task.Start();

            double res = await task;  // await приймає на вхід Task!

            // код після await-y виконується як задача продовження, причому в основному потоці, тому
            // тут не потрібно викликати Invoke для оновлення елементів графічного інтерфейсу користувача
            // тобто await буде очікувати результату виконання task, але не буде блокувати основний потік
            textBox4.Text = res.ToString();
            return res;
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            double res = await getResultAsync(5);
            textBox5.Text = res.ToString();
        }

        // --------------------------------------------------------
        // sample 5 - async/await based implementation
        // --------------------------------------------------------

        private async void button5_Click(object sender, EventArgs e)
        {
            double r1 = await getResultAsync(5);
            double r2 = await getResultAsync(6);
            double r3 = await getResultAsync(7);
            textBox6.Text = $"t1={r1} t2={r2} t3={r3}";
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            Task<double> t1 = getResultAsync(1);
            Task<double> t2 = getResultAsync(2);
            Task<double> t3 = getResultAsync(3);
            await Task.WhenAll(t1, t2, t3);
            textBox6.Text = $"t1={t1.Result} t2={t2.Result} t3={t3.Result}";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Task<double> t1 = getResultTask(1); // getResultAsync(1);
            Task<double> t2 = getResultTask(1); //  getResultAsync(2);
            Task<double> t3 = getResultTask(1); // getResultAsync(3);

            // When you call Task.WaitAll(t1, t2, t3) in the UI thread, it blocks the UI thread
            //   while waiting for the tasks to complete. Meanwhile, the tasks themselves are trying
            //   to resume on the UI thread after awaiting, but they can't because the UI thread
            //   is blocked by Task.WaitAll().

            Task.WaitAll(t1, t2, t3);

            // If we ever get here (we won't due to the deadlock), then the tasks are done.

            textBox6.Text = $"t1={t1.Result} t2={t2.Result} t3={t3.Result}";
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            // will lead to exception unless you comment textBox4.Text in getResultAsync()

            double[] results = await Task.Run(() =>
            {
                Task<double> t1 = getResultAsync(1);
                Task<double> t2 = getResultAsync(2);
                Task<double> t3 = getResultAsync(3);

                Task.WaitAll(t1, t2, t3);

                return new[] { t1.Result, t2.Result, t3.Result };
            });

            textBox6.Text = $"t1={results[0]} t2={results[1]} t3={results[2]}";
        }
    }
}
