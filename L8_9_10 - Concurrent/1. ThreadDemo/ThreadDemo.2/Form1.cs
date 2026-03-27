namespace OneThreadDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // ------------- SAMPLE 3: UI hanging problem solved but how to return the RESULT ?

        class BackgroundExecutorWithResult
        {
            public static void Solve()
            {
                decimal res = 0;
                for (long i = 0; i < 20000000; i++)
                    res += (decimal) Math.Sin(i);

                // треба робити приведення до типу нашої форми,
                // так як масив OpenForms зберігає посилання посилання,
                // типизовані базовим класом Form
                Form1 form1 = Application.OpenForms["Form1"] as Form1;

              //   form1.textBox1.Text = "Thread Finished!"; // will fire EXCEPTION

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
            // посилання на метод, який ми хочемо запустити. можливий один параметр у вигляді Object
            Thread t = new Thread(BackgroundExecutorWithResult.Solve);  
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

                // BRAINSTORM:  чи можна тут спростити код і написати так ?
                // form1.textBox1.Text = "Thread Finished!"; 
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
    }
}