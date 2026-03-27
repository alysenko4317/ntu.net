namespace OneThreadDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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

            t.Start(iterationsCount); // passing the parameter 
            textBox1.Text = "Calculation started in Background Thread...";
        }
    }
}
