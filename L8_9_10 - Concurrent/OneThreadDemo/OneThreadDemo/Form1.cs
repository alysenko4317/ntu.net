namespace OneThreadDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // ------------- SAMPLE 1: one thread - UI  problem demonstration
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "Started";
            decimal res = 0;
            for (long i = 0; i < 100000000; i++)
                res += (decimal) Math.Sin(i);
            textBox1.Text = "";
        }

        // ------------- SAMPLE 2: UI problem solved

        class BackgroundExecutor
        {
            public static void Solve()
            {
                decimal res = 0;
                for (long i = 0; i < 100000000; i++)
                    res += (decimal) Math.Sin(i);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(BackgroundExecutor.Solve);  // посилання на метод, який ми хочемо запустити. можливий один параметр у вигляді Object
            t.Start();
            textBox1.Text = "Started in Backgound Thread...";
        }


        // ------------- SAMPLE 3: UI problem solved but how to return the RESULT ?

        class BackgroundExecutorWithResult
        {
            public static void Solve()
            {
                decimal res = 0;
                for (long i = 0; i < 20000000; i++)
                    res += (decimal) Math.Sin(i);

                // треба робити приведення до типу нашої форми, так як масив OpenForms зберігає посилання посилання,
                // типизовані базовим класом Form
                Form1 form1 = Application.OpenForms["Form1"] as Form1;

               // form1.textBox1.Text = "Thread Finished!"; // will fire exception

                // Метод Invoke в C# використовується для виконання коду на потоці інтерфейсу користувача (UI thread)
                // з іншого потока. У Win32 API, для досягнення подібної функціональності, ви можете використовувати
                // функції PostMessage або SendMessage
                form1.Invoke(new Action(() => {
                    form1.textBox1.Text = $"Thread Finished with res={res.ToString()}";
                }));  // так є правильно! 
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(BackgroundExecutorWithResult.Solve);  // посилання на метод, який ми хочемо запустити. можливий один параметр у вигляді Object
            t.Start();
            textBox1.Text = "Calculation started in Backgound Thread...";
        }

        // ------------- SAMPLE 4: thread body as inline delegate

        private void button5_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
                decimal res = 0;
                for (long i = 0; i < 20000000; i++)
                    res += (decimal) Math.Sin(i);

                Form1 form1 = Application.OpenForms["Form1"] as Form1;
                form1.Invoke(new Action(() =>
                {
                    form1.textBox1.Text = $"Thread Finished with res={res.ToString()}";
                }));

                // form1.textBox1.Text = "Thread Finished!"; // чи можна тут спростити код і написати так ?
            });

            t.Start();
            textBox1.Text = "Calculation started in Backgound Thread...";
        }

        private void button5_Click_Simplified(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
                decimal res = 0;
                for (long i = 0; i < 20000000; i++)
                    res += (decimal) Math.Sin(i);

                Invoke(new Action(() =>
                {
                    textBox1.Text = $"Thread Finished with res={res.ToString()}";
                }));
            });

            t.Start();
            textBox1.Text = "Calculation started in Backgound Thread...";
        }

        // ------------- SAMPLE 4: passing parameter to a thread

        private void button6_Click(object sender, EventArgs e)
        {
            const long iterationsCount = 20000000;

            Thread t = new Thread((parameterObject) =>
            {
                long iterations = Convert.ToInt64(parameterObject);

                decimal res = 0;
                for (long i = 0; i < iterations; i++)
                    res += (decimal)Math.Sin(i);

                // Use Invoke to update the UI from the background thread
                this.Invoke(new Action(() =>
                {
                    this.textBox1.Text = $"Thread Finished with res={res.ToString()}";
                }));
            });

            t.Start(iterationsCount);
            textBox1.Text = "Calculation started in Background Thread...";
        }
    }
}