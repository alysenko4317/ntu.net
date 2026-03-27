namespace SecondThreadDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        class BackgroundExecutor
        {
            private Thread _thread;

            public void Start()
            {
                _thread = new Thread(Solve);
                _thread.Start();
            }

            private void Solve()
            {
                decimal res = 0;
                for (long i = 0; i < 100000000; i++)
                    res += (decimal) Math.Sin(i);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BackgroundExecutor executor = new BackgroundExecutor();
            executor.Start();  // Start the background execution
        }
    }
}