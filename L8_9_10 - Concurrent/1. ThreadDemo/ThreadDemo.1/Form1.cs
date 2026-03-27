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
                res += (decimal)Math.Sin(i);
            textBox1.Text = "Result: " + res;
        }

        // ------------- SAMPLE 2: UI problem solved

        class BackgroundExecutor
        {
            public static void Solve()
            {
                decimal res = 0;
                for (long i = 0; i < 100000000; i++)
                    res += (decimal)Math.Sin(i);
            }

            // але як тепер повернути результат?
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(BackgroundExecutor.Solve);

            // BackgroundExecutor.Solve - це посилання на метод, який ми хочемо запустити.
            //   - можливий один параметр у вигляді Object

            t.Start();
            
            textBox1.Text = "Started in Backgound Thread...";
        }
    }
}