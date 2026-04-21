namespace StartStopDemo_2
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
            lblProblems = new Label();
            btnStart = new Button();
            btnStop = new Button();
            btnPause = new Button();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = false;
            lblTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTitle.Location = new Point(12, 12);
            lblTitle.Size = new Size(776, 20);
            lblTitle.Text = "3.2 — Start / Stop / Pause: ПОГАНИЙ дизайн (навмисно для демонстрації)";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblProblems
            // 
            lblProblems.AutoSize = false;
            lblProblems.ForeColor = Color.DarkRed;
            lblProblems.Location = new Point(12, 42);
            lblProblems.Size = new Size(776, 65);
            lblProblems.Text =
                "❌ Проблема 1: Thread.Abort() — кидає виняток у довільній точці, псує стан, видалено у .NET 5+\n" +
                "❌ Проблема 2: Thread.Suspend()/Resume() — може спричинити дедлок, видалено у .NET 5+\n" +
                "❌ Проблема 3: bool без volatile — зміна з UI-потоку може бути невидима фоновому потоку\n" +
                "❌ Проблема 4: кнопка Pause лише змінює текст — реальної паузи НЕМАЄ!";
            // 
            // btnStart
            // 
            btnStart.Location = new Point(12, 120);
            btnStart.Size = new Size(180, 36);
            btnStart.Text = "▶  Start";
            btnStart.BackColor = Color.Honeydew;
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(205, 120);
            btnStop.Size = new Size(180, 36);
            btnStop.Text = "⏹  Stop (встановлює прапор)";
            btnStop.BackColor = Color.MistyRose;
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += btnStop_Click;
            // 
            // btnPause
            // 
            btnPause.Location = new Point(398, 120);
            btnPause.Size = new Size(220, 36);
            btnPause.Text = "Pause ⚠ (не працює!)";
            btnPause.BackColor = Color.LightYellow;
            btnPause.UseVisualStyleBackColor = false;
            btnPause.Click += btnPause_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 172);
            textBox1.Size = new Size(776, 31);
            textBox1.ReadOnly = true;
            textBox1.Text = "Статус...";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 225);
            Controls.Add(lblTitle);
            Controls.Add(lblProblems);
            Controls.Add(btnStart);
            Controls.Add(btnStop);
            Controls.Add(btnPause);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "3.2 — Start/Stop/Pause: Bad Design";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblProblems;
        private Button btnStart;
        private Button btnStop;
        private Button btnPause;
        private TextBox textBox1;
    }
}