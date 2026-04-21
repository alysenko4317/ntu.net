namespace BackgroundWorker
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            lblTitle = new Label();
            lblEvents = new Label();
            lblBugs = new Label();
            btnStart = new Button();
            btnStop = new Button();
            progressBar1 = new ProgressBar();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // backgroundWorker1
            // 
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = false;
            lblTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTitle.Location = new Point(12, 12);
            lblTitle.Size = new Size(776, 20);
            lblTitle.Text = "4.1 — BackgroundWorker: інкапсуляція фонового потоку (з навмисними недоліками)";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblEvents
            // 
            lblEvents.AutoSize = false;
            lblEvents.ForeColor = Color.DarkGreen;
            lblEvents.Location = new Point(12, 40);
            lblEvents.Size = new Size(776, 50);
            lblEvents.Text =
                "✔ Автоматичний маршалінг ProgressChanged і RunWorkerCompleted у UI-потік через SynchronizationContext — без Invoke()!\n" +
                "   DoWork → фоновий потік (ThreadPool)  |  ProgressChanged → UI-потік  |  RunWorkerCompleted → UI-потік";
            // 
            // lblBugs
            // 
            lblBugs.AutoSize = false;
            lblBugs.ForeColor = Color.DarkRed;
            lblBugs.Location = new Point(12, 95);
            lblBugs.Size = new Size(776, 35);
            lblBugs.Text =
                "❌ Недолік 1: throw перед e.Cancel=true → unreachable code, скасування приходить як e.Error, а не e.Cancelled\n" +
                "❌ Недолік 2: progressBar +1/-1 костиль через WinForms ProgressBar анімацію  →  виправлено у 4.2";
            // 
            // btnStart
            // 
            btnStart.Location = new Point(12, 143);
            btnStart.Size = new Size(200, 36);
            btnStart.Text = "▶  Start (RunWorkerAsync)";
            btnStart.BackColor = Color.Honeydew;
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(225, 143);
            btnStop.Size = new Size(200, 36);
            btnStop.Text = "⏹  Stop (CancelAsync)";
            btnStop.BackColor = Color.MistyRose;
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Enabled = false;
            btnStop.Click += btnStop_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 193);
            progressBar1.Size = new Size(776, 24);
            progressBar1.Step = 1;
            progressBar1.Visible = false;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 230);
            textBox1.Size = new Size(776, 31);
            textBox1.ReadOnly = true;
            textBox1.Text = "Статус...";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 283);
            Controls.Add(lblTitle);
            Controls.Add(lblEvents);
            Controls.Add(lblBugs);
            Controls.Add(btnStart);
            Controls.Add(btnStop);
            Controls.Add(progressBar1);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "4.1 — BackgroundWorker (з недоліками)";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Label lblTitle;
        private Label lblEvents;
        private Label lblBugs;
        private Button btnStart;
        private Button btnStop;
        private ProgressBar progressBar1;
        private TextBox textBox1;
    }
}