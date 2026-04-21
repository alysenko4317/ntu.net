namespace StopPauseDemo
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
            lblOptimization = new Label();
            btnStart = new Button();
            btnStop = new Button();
            btnPause = new Button();
            btnOpenCalc = new Button();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = false;
            lblTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTitle.Location = new Point(12, 12);
            lblTitle.Size = new Size(776, 20);
            lblTitle.Text = "3.3 — Start / Stop / Pause: ПРАВИЛЬНИЙ дизайн";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSolution
            // 
            lblSolution.AutoSize = false;
            lblSolution.ForeColor = Color.DarkGreen;
            lblSolution.Location = new Point(12, 40);
            lblSolution.Size = new Size(776, 40);
            lblSolution.Text =
                "✔ Пауза через ManualResetEvent: Reset() → блокує потік, Set() → відновлює\n" +
                "✔ volatile bool — зміна прапора скасування завжди видна між потоками   " +
                "✔ IListener — повна ізоляція виконавця від UI";
            // 
            // lblOptimization
            // 
            lblOptimization.AutoSize = false;
            lblOptimization.ForeColor = Color.DarkSlateBlue;
            lblOptimization.Location = new Point(12, 85);
            lblOptimization.Size = new Size(776, 40);
            lblOptimization.Text =
                "⚡ Виклик підпрограми (call/ret) ≈ 5–10 тактів; WaitOne()/Invoke() = системний виклик ≈ сотні–тисячі тактів → не викликати кожну ітерацію!\n" +
                "   Фільтр: if (i % 65536==0) ← % = ділення ~25 тактів  |  if ((i & 0xFFFF)==0) ← & = 1 такт ✔  (працює лише для степеня двійки)";
            // 
            // btnStart
            // 
            btnStart.Location = new Point(12, 138);
            btnStart.Size = new Size(140, 36);
            btnStart.Text = "▶  Start";
            btnStart.BackColor = Color.Honeydew;
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(162, 138);
            btnStop.Size = new Size(140, 36);
            btnStop.Text = "⏹  Stop";
            btnStop.BackColor = Color.MistyRose;
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += btnStop_Click;
            // 
            // btnPause
            // 
            btnPause.Location = new Point(312, 138);
            btnPause.Size = new Size(140, 36);
            btnPause.Text = "⏸  Pause";
            btnPause.BackColor = Color.LightYellow;
            btnPause.UseVisualStyleBackColor = false;
            btnPause.Click += btnPause_Click;
            // 
            // btnOpenCalc
            // 
            btnOpenCalc.Location = new Point(600, 138);
            btnOpenCalc.Size = new Size(188, 36);
            btnOpenCalc.Text = "🖩  Відкрити Калькулятор";
            btnOpenCalc.BackColor = Color.AliceBlue;
            btnOpenCalc.UseVisualStyleBackColor = false;
            btnOpenCalc.Click += btnOpenCalc_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 190);
            textBox1.Size = new Size(776, 31);
            textBox1.ReadOnly = true;
            textBox1.Text = "Статус...";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 243);
            Controls.Add(lblTitle);
            Controls.Add(lblSolution);
            Controls.Add(lblOptimization);
            Controls.Add(btnStart);
            Controls.Add(btnStop);
            Controls.Add(btnPause);
            Controls.Add(btnOpenCalc);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "3.3 — Start/Stop/Pause: Good Design";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblSolution;
        private Label lblOptimization;
        private Button btnStart;
        private Button btnStop;
        private Button btnPause;
        private Button btnOpenCalc;
        private TextBox textBox1;
    }
}