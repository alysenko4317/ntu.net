namespace BackgroundWorker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
            textBox1.Text = "Started...";
            progressBar1.Show();
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            const long iterationsCount = 20000000;

            decimal res = 0;
            for (long i = 0; i < iterationsCount; i++)
            {
                if ((i & 0x3FFFF) == 0)
                {
                    int progress = (int)((i + 1.0) / iterationsCount * 100);
                    backgroundWorker1.ReportProgress(progress);

                    if (backgroundWorker1.CancellationPending)
                    {
                        throw new Exception("щось пішло не так!");
                        e.Cancel = true;
                        break;
                    }
                }

                res += (decimal) Math.Sin(i);
                // read chunk
                // xor encription
                // write encrypted chunk
            }
            e.Result = res;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(string.Format("Ошибка: {0}", e.Error.Message));
            }

            if (!e.Cancelled)
                textBox1.Text = "Task Completed!" + " Result=" + e.Result.ToString();
            else
                textBox1.Text = "Cancelled!";
            progressBar1.Hide();
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            textBox1.Text = "Started, progress=" + e.ProgressPercentage;

            progressBar1.Value = e.ProgressPercentage + 1;  // костыль для покращення UX
            progressBar1.Value = e.ProgressPercentage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;

            backgroundWorker1.CancelAsync();
        }
    }
}