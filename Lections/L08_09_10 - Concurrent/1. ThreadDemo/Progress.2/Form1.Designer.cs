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
            lblInvokeEx = new Label();
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
            lblTitle.Text = "1.2 — Введення у потоки. Рішення: Control.Invoke()";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSolution
            // 
            lblSolution.AutoSize = false;
            lblSolution.ForeColor = Color.DarkGreen;
            lblSolution.Location = new Point(12, 45);
            lblSolution.Size = new Size(776, 40);
            lblSolution.Text = "✔ Фоновий потік — UI не замерзає\n" +
                               "✔ Invoke() маршалізує виклик у UI-потік — немає InvalidOperationException";
            // 
            // lblInvokeEx
            // 
            lblInvokeEx.AutoSize = false;
            lblInvokeEx.ForeColor = Color.DarkSlateGray;
            lblInvokeEx.Location = new Point(12, 95);
            lblInvokeEx.Size = new Size(776, 35);
            lblInvokeEx.Text = "Оптимізація: InvokeEx() перевіряє InvokeRequired → " +
                               "якщо вже в UI-потоці, action() викликається напряму без зайвої черги";
            // 
            // btnStart
            // 
            btnStart.Location = new Point(12, 145);
            btnStart.Size = new Size(370, 40);
            btnStart.TabIndex = 0;
            btnStart.Text = "▶  Запустити у фоновому потоці + Invoke";
            btnStart.BackColor = Color.Honeydew;
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(410, 145);
            btnStop.Size = new Size(378, 40);
            btnStop.TabIndex = 1;
            btnStop.Text = "⏹  Stop";
            btnStop.Click += btnStop_Click;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(12, 205);
            progressBar.Size = new Size(776, 28);
            progressBar.TabIndex = 2;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = false;
            lblStatus.Location = new Point(12, 248);
            lblStatus.Size = new Size(776, 20);
            lblStatus.Text = "Статус: очікування...";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 285);
            Controls.Add(lblTitle);
            Controls.Add(lblSolution);
            Controls.Add(lblInvokeEx);
            Controls.Add(btnStart);
            Controls.Add(btnStop);
            Controls.Add(progressBar);
            Controls.Add(lblStatus);
            Name = "Form1";
            Text = "1.2 — Threads Intro: Invoke";
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitle;
        private Label lblSolution;
        private Label lblInvokeEx;
        private Button btnStart;
        private Button btnStop;
        private ProgressBar progressBar;
        private Label lblStatus;
    }
}
