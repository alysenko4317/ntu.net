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
            lblFixes = new Label();
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
            lblTitle.Text = "4.2 — BackgroundWorker: ВИПРАВЛЕНА версія";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblFixes
            // 
            lblFixes.AutoSize = false;
            lblFixes.ForeColor = Color.DarkGreen;
            lblFixes.Location = new Point(12, 40);
            lblFixes.Size = new Size(776, 40);
            lblFixes.Text =
                "✔ Виправлення 1: e.Cancel=true + return замість throw → e.Cancelled=true у RunWorkerCompleted\n" +
                "✔ Виправлення 2: ProgressBar.Style=Continuous → пряме присвоєння Value без костилю +1/-1";
            // 
            // btnStart
            // 
            btnStart.Location = new Point(12, 93);
            btnStart.Size = new Size(200, 36);
            btnStart.Text = "▶  Start (RunWorkerAsync)";
            btnStart.BackColor = Color.Honeydew;
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(225, 93);
            btnStop.Size = new Size(200, 36);
            btnStop.Text = "⏹  Stop (CancelAsync)";
            btnStop.BackColor = Color.MistyRose;
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Enabled = false;
            btnStop.Click += btnStop_Click;
            // 
            // progressBar1 — Style=Continuous вимикає плавну анімацію → +1/-1 не потрібен
            // 
            progressBar1.Location = new Point(12, 143);
            progressBar1.Size = new Size(776, 24);
            progressBar1.Step = 1;
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.Visible = false;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 180);
            textBox1.Size = new Size(776, 31);
            textBox1.ReadOnly = true;
            textBox1.Text = "Статус...";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 233);
            Controls.Add(lblTitle);
            Controls.Add(lblFixes);
            Controls.Add(btnStart);
            Controls.Add(btnStop);
            Controls.Add(progressBar1);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "4.2 — BackgroundWorker (виправлено)";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Label lblTitle;
        private Label lblFixes;
        private Button btnStart;
        private Button btnStop;
        private ProgressBar progressBar1;
        private TextBox textBox1;
    }
}