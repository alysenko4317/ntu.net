namespace Progress
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
            lblTitle = new Label();
            lblProblem1 = new Label();
            lblProblem2 = new Label();
            btnStartBlocking = new Button();
            btnStartThread = new Button();
            btnStop = new Button();
            progressBar = new ProgressBar();
            lblStatus = new Label();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = false;
            lblTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTitle.Location = new Point(12, 12);
            lblTitle.Size = new Size(776, 20);
            lblTitle.Text = "1.1 — Проблема відповідальності UI-потоку";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblProblem1
            // 
            lblProblem1.AutoSize = false;
            lblProblem1.ForeColor = Color.DarkRed;
            lblProblem1.Location = new Point(12, 50);
            lblProblem1.Size = new Size(370, 40);
            lblProblem1.Text = "Проблема 1: довга операція в UI-потоці\nзамороджує весь інтерфейс";
            // 
            // lblProblem2
            // 
            lblProblem2.AutoSize = false;
            lblProblem2.ForeColor = Color.DarkBlue;
            lblProblem2.Location = new Point(410, 50);
            lblProblem2.Size = new Size(378, 40);
            lblProblem2.Text = "Проблема 2: оновлення UI з іншого потоку\nкидає InvalidOperationException";
            // 
            // btnStartBlocking
            // 
            btnStartBlocking.Location = new Point(12, 105);
            btnStartBlocking.Size = new Size(370, 40);
            btnStartBlocking.TabIndex = 0;
            btnStartBlocking.Text = "▶  Запустити (блокує UI)";
            btnStartBlocking.BackColor = Color.MistyRose;
            btnStartBlocking.UseVisualStyleBackColor = false;
            btnStartBlocking.Click += btnStartBlocking_Click;
            // 
            // btnStartThread
            // 
            btnStartThread.Location = new Point(410, 105);
            btnStartThread.Size = new Size(378, 40);
            btnStartThread.TabIndex = 1;
            btnStartThread.Text = "▶  Запустити в потоці (виняток)";
            btnStartThread.BackColor = Color.AliceBlue;
            btnStartThread.UseVisualStyleBackColor = false;
            btnStartThread.Click += btnStartThread_Click;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(12, 165);
            progressBar.Size = new Size(776, 28);
            progressBar.TabIndex = 2;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(350, 210);
            btnStop.Size = new Size(100, 30);
            btnStop.TabIndex = 3;
            btnStop.Text = "⏹  Stop";
            btnStop.Click += btnStop_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = false;
            lblStatus.Location = new Point(12, 255);
            lblStatus.Size = new Size(776, 20);
            lblStatus.Text = "Статус: очікування...";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 290);
            Controls.Add(lblTitle);
            Controls.Add(lblProblem1);
            Controls.Add(lblProblem2);
            Controls.Add(btnStartBlocking);
            Controls.Add(btnStartThread);
            Controls.Add(progressBar);
            Controls.Add(btnStop);
            Controls.Add(lblStatus);
            Name = "Form1";
            Text = "1.1 — UI Responsiveness Problem";
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitle;
        private Label lblProblem1;
        private Label lblProblem2;
        private Button btnStartBlocking;
        private Button btnStartThread;
        private Button btnStop;
        private ProgressBar progressBar;
        private Label lblStatus;
    }
}
