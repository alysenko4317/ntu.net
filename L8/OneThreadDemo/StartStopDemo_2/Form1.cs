using static System.Windows.Forms.AxHost;

namespace StartStopDemo_2
{
    // idea demonstration but the design is bad
    // better design illustrated in StopPauseDemo_1 sample

    public partial class Form1 : Form
    {
        public Thread _workerThread;
        public bool _isCancellationRequested = false;

        public Form1()
        {
            InitializeComponent();
        }

         private void Form1_Load(object sender, EventArgs e)
         {
             btnStop.Enabled = false;
             btnPause.Enabled = false;
             btnStart.Enabled = true;
         }

         private void btnStart_Click(object sender, EventArgs e)
         {
             btnStop.Enabled = true;
             btnPause.Enabled = true;
             btnStart.Enabled = false;

             _workerThread = new Thread(() =>
             {
                 Invoke(new Action(() => {
                     textBox1.Text = "Thread started...";
                 }));

                 decimal res = 0;
                 for (long i = 0; i < 20000000; i++)
                 {
                     if (_isCancellationRequested)
                         break;

                     res += (decimal)Math.Sin(i);
                 }

                 Invoke(new Action(() => {

                     textBox1.Text = !_isCancellationRequested
                         ? $"Thread Finished with res={res.ToString()}"
                         : "Canceled!";

                     btnStop.Enabled = false;
                     btnPause.Enabled = false;
                     btnStart.Enabled = true;
                 }));

                 _isCancellationRequested = false;
             });

             _workerThread.Start();
         }

         private void btnStop_Click(object sender, EventArgs e)
         {
             //    btnStop.Enabled = false;
             //    btnPause.Enabled = false;
             //    btnStart.Enabled = true;
             //    _workerThread.Abort();

             _isCancellationRequested = true;
         }

         private void btnPause_Click(object sender, EventArgs e)
         {
             btnPause.Text = btnPause.Text == "Pause" ? "Resume" : "Pause";
         }
    }
}