namespace BackgroundWorker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            textBox1.Text = "Запущено...";
            progressBar1.Value = 0;
            progressBar1.Show();
            backgroundWorker1.RunWorkerAsync();
        }

        // ── DoWork — фоновий потік ────────────────────────────────────────────
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            const long iterationsCount = 20_000_000;
            decimal res = 0;

            for (long i = 0; i < iterationsCount; i++)
            {
                if ((i & 0x3FFFF) == 0)  // кожні 262 144 ітерацій (2^18)
                {
                    backgroundWorker1.ReportProgress((int)((i + 1.0) / iterationsCount * 100));

                    if (backgroundWorker1.CancellationPending)
                    {
                        // ✔ ВИПРАВЛЕННЯ 1: правильне кооперативне скасування
                        e.Cancel = true;
                        return;  // виходимо чисто — без виняткiв
                    }
                }
                res += (decimal)Math.Sin(i);
            }
            e.Result = res;
        }

        // ── ProgressChanged — UI-потік ────────────────────────────────────────
        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            textBox1.Text = $"Виконується... {e.ProgressPercentage}%";
            // ✔ ВИПРАВЛЕННЯ 2: Style = Continuous → пряме присвоєння без хаку +1/-1
            progressBar1.Value = e.ProgressPercentage;
        }

        // ── RunWorkerCompleted — UI-потік ─────────────────────────────────────
        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            progressBar1.Hide();
            btnStart.Enabled = true;
            btnStop.Enabled = false;

            if (e.Error != null)
            {
                // Тепер сюди потрапляємо лише при справжніх непередбачених помилках
                textBox1.Text = $"Помилка: {e.Error.Message}";
                return;
            }
            if (e.Cancelled)
            {
                // ✔ Тепер e.Cancelled == true при натисканні Stop (а не e.Error)
                textBox1.Text = "Скасовано!";
                return;
            }
            textBox1.Text = $"Завершено! Результат = {e.Result}";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }
    }
}