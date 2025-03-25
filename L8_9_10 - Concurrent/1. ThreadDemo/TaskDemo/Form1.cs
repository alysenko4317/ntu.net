namespace TaskDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // --------------------------------------------
        // SAMPLE 1: Task class introduction
        // --------------------------------------------

        private void button1_Click(object sender, EventArgs e)
        {
            Task t = new Task(() =>
            {
                decimal res = 0;
                for (long i = 0; i < 20000000; i++)
                    res += (decimal) Math.Sin(i);

                Invoke(new Action(() =>
                {
                    textBox1.Text = $"Task Finished with res={res.ToString()}";
                }));
            });

            t.Start();
            textBox1.Text = "Calculation started in Backgound Thread...";
        }

        // --------------------------------------------
        // SAMPLE 2: Waiting for multiple tasks
        // --------------------------------------------

        private void button2_Click(object sender, EventArgs e)
        {
            List<Task> tasks = new List<Task>();

            tasks.Add(new Task(() =>
            {
                double res = 0;
                long i = 0;
                for (i = 0; i < 10000000; i++)
                    res += Math.Sin(i) * Math.Cos(i);
            }));

            tasks.Add(new Task(() =>
            {
                double res = 0;
                long i = 0;
                for (i = 0; i < 10000000; i++)
                    res += Math.Sin(i);
            }));

            tasks.Add(new Task(() =>
            {
                double res = 0;
                long i = 0;
                for (i = 0; i < 20000000; i++)
                    res += Math.Cos(i);
            }));

            foreach (Task task in tasks)
                task.Start();

         //   await Task.WhenAll(tasks);
            Task.WaitAll(tasks.ToArray());  // � ��� ������� deadlock

            Invoke(new Action(() => {
                textBox2.Text = "�� ������ ��������� ������!}";
            }));
        }

        // --------------------------------------------
        // SAMPLE 3: Waiting for multiple threads
        // --------------------------------------------

        static void Work()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"���� {Thread.CurrentThread.ManagedThreadId}: �������� {i + 1}");
                Thread.Sleep(1000); // ��������� ������
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // ��������� ������ ������
            Thread[] threads = new Thread[3];

            // ����������� � ������ ������
            threads[0] = new Thread(Work);
            threads[1] = new Thread(Work);
            threads[2] = new Thread(Work);

            foreach (var thread in threads)
                thread.Start();

            // ���������� ���������� ������ ��� ������
            foreach (var thread in threads)
                thread.Join();

            Invoke(new Action(() => {
                textBox3.Text = "�� ������ ��������� ������!}";
            }));
        }
    }
}
