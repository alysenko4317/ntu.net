using System.CodeDom.Compiler;
using System.Diagnostics;

namespace StopPauseDemo
{
    // better design
    // TO DEMONSTRATE: but background thread will not stop gracefully if UI will be closed
    public partial class Form1 : Form, BackgroundExecutor.IListener
    {
        BackgroundExecutor _executor;

        public Form1()
        {
            InitializeComponent();
            _executor = new BackgroundExecutor(this);
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

            _executor.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _executor.Cancel();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (_executor.IsSuspended())
                _executor.Resume();
            else
                _executor.Suspend();
        }

        public void OnStatusChanged(BackgroundExecutor.Status newStatus, string statusText)
        {
            Invoke(new Action(() =>
            {
                textBox1.Text = statusText;

                switch (newStatus)
                {
                    case BackgroundExecutor.Status.SUSPENDED:
                        btnPause.Text = "Resume";
                        break;

                    case BackgroundExecutor.Status.CANCELLED:
                    case BackgroundExecutor.Status.FINISHED:
                        btnStop.Enabled = false;
                        btnPause.Enabled = false;
                        btnStart.Enabled = true;
                        break;

                    case BackgroundExecutor.Status.STARTED:
                        btnPause.Text = "Pause";
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("Calc");
        }
    }
}