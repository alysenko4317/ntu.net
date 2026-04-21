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
            lblSolution = new Label();
            lblContextInfo = new Label();
            lblSendPost = new Label();
            btnStart = new Button();
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
            lblTitle.Text = "1.3 — Контекст синхронізації (SynchronizationContext)";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSolution
            // 
            lblSolution.AutoSize = false;
            lblSolution.ForeColor = Color.DarkGreen;
            lblSolution.Location = new Point(12, 42);
            lblSolution.Size = new Size(776, 35);
            lblSolution.Text = "✔ Worker НЕ залежить від Control.Invoke() або Dispatcher — він отримує абстракцію SynchronizationContext\n" +
                               "✔ Той самий Worker-код працює у WinForms, WPF, ASP.NET та Unit-тестах без жодних змін";
            // 
            // lblContextInfo
            // 
            lblContextInfo.AutoSize = false;
            lblContextInfo.ForeColor = Color.DarkSlateBlue;
            lblContextInfo.Location = new Point(12, 85);
            lblContextInfo.Size = new Size(776, 50);
            lblContextInfo.Text = "Реалізації SynchronizationContext:\n" +
                                  "  WinForms → WindowsFormsSynchronizationContext  (Control.Invoke всередині)\n" +
                                  "  WPF      → DispatcherSynchronizationContext    (Dispatcher.Invoke всередині)";
            // 
            // lblSendPost
            // 
            lblSendPost.AutoSize = false;
            lblSendPost.ForeColor = Color.DarkSlateGray;
            lblSendPost.Location = new Point(12, 143);
            lblSendPost.Size = new Size(776, 18);
            lblSendPost.Text = "Send(cb, state) — синхронно, чекає завершення (≈ Invoke)       |       Post(cb, state) — асинхронно, не чекає (≈ BeginInvoke)";
            // 
            // btnStart
            // 
            btnStart.Location = new Point(12, 173);
            btnStart.Size = new Size(370, 40);
            btnStart.TabIndex = 0;
            btnStart.Text = "▶  Запустити (SynchronizationContext.Send)";
            btnStart.BackColor = Color.LavenderBlush;
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(410, 173);
            btnStop.Size = new Size(378, 40);
            btnStop.TabIndex = 1;
            btnStop.Text = "⏹  Stop";
            btnStop.Click += btnStop_Click;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(12, 228);
            progressBar.Size = new Size(776, 28);
            progressBar.TabIndex = 2;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = false;
            lblStatus.Location = new Point(12, 268);
            lblStatus.Size = new Size(776, 20);
            lblStatus.Text = "Статус: очікування...";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 305);
            Controls.Add(lblTitle);
            Controls.Add(lblSolution);
            Controls.Add(lblContextInfo);
            Controls.Add(lblSendPost);
            Controls.Add(btnStart);
            Controls.Add(btnStop);
            Controls.Add(progressBar);
            Controls.Add(lblStatus);
            Name = "Form1";
            Text = "1.3 — SynchronizationContext";
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitle;
        private Label lblSolution;
        private Label lblContextInfo;
        private Label lblSendPost;
        private Button btnStart;
        private Button btnStop;
        private ProgressBar progressBar;
        private Label lblStatus;
    }
}
