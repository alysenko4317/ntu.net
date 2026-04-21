namespace SecondThreadDemo
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
            lblDescription = new Label();
            btnStart = new Button();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = false;
            lblTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTitle.Location = new Point(12, 12);
            lblTitle.Size = new Size(776, 20);
            lblTitle.Text = "3.1 — Інкапсуляція керування потоком у класі BackgroundExecutor";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = false;
            lblDescription.ForeColor = Color.DarkSlateGray;
            lblDescription.Location = new Point(12, 42);
            lblDescription.Size = new Size(776, 50);
            lblDescription.Text = "✔ Логіка виконання відокремлена від Form у клас BackgroundExecutor\n" +
                                  "⚠ Проблеми: IsBackground не встановлено, немає зупинки, немає зворотного зв'язку\n" +
                                  "→ Рішення у наступних проектах (StartStopDemo_2, StopPauseDemo)";
            // 
            // btnStart
            // 
            btnStart.Location = new Point(12, 105);
            btnStart.Size = new Size(370, 40);
            btnStart.Text = "▶  Запустити BackgroundExecutor";
            btnStart.BackColor = Color.AliceBlue;
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 160);
            textBox1.Size = new Size(776, 31);
            textBox1.ReadOnly = true;
            textBox1.Text = "Статус...";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 215);
            Controls.Add(lblTitle);
            Controls.Add(lblDescription);
            Controls.Add(btnStart);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "3.1 — BackgroundExecutor Encapsulation";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblDescription;
        private Button btnStart;
        private TextBox textBox1;
    }
}