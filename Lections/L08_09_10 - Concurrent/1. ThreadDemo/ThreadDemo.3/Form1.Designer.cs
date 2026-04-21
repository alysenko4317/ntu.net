namespace OneThreadDemo
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
            lblSample5a = new Label();
            lblSample5b = new Label();
            lblWarning = new Label();
            btnParameterizedThread = new Button();
            btnClosureParameter = new Button();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = false;
            lblTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTitle.Location = new Point(12, 12);
            lblTitle.Size = new Size(776, 20);
            lblTitle.Text = "2.3 — Передача параметра у потік: Thread.Start(param) vs замикання";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSample5a
            // 
            lblSample5a.AutoSize = false;
            lblSample5a.ForeColor = Color.DarkOrange;
            lblSample5a.Location = new Point(12, 45);
            lblSample5a.Size = new Size(370, 40);
            lblSample5a.Text = "Зразок 5а: Thread.Start(object) — старий підхід\nОдин параметр, тип object, потрібен явний cast";
            // 
            // lblSample5b
            // 
            lblSample5b.AutoSize = false;
            lblSample5b.ForeColor = Color.DarkGreen;
            lblSample5b.Location = new Point(400, 45);
            lblSample5b.Size = new Size(388, 40);
            lblSample5b.Text = "Зразок 5б: замикання — сучасний підхід ✔\nБудь-яка кількість параметрів, повний type safety";
            // 
            // btnParameterizedThread
            // 
            btnParameterizedThread.Location = new Point(12, 100);
            btnParameterizedThread.Size = new Size(370, 40);
            btnParameterizedThread.Text = "▶  Thread.Start(param) — з cast-ом";
            btnParameterizedThread.BackColor = Color.LightYellow;
            btnParameterizedThread.UseVisualStyleBackColor = false;
            btnParameterizedThread.Click += btnParameterizedThread_Click;
            // 
            // btnClosureParameter
            // 
            btnClosureParameter.Location = new Point(400, 100);
            btnClosureParameter.Size = new Size(388, 40);
            btnClosureParameter.Text = "▶  Замикання — без cast-у";
            btnClosureParameter.BackColor = Color.Honeydew;
            btnClosureParameter.UseVisualStyleBackColor = false;
            btnClosureParameter.Click += btnClosureParameter_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 158);
            textBox1.Size = new Size(776, 31);
            textBox1.ReadOnly = true;
            textBox1.Text = "Результат з'явиться тут...";
            // 
            // lblWarning
            // 
            lblWarning.AutoSize = false;
            lblWarning.ForeColor = Color.DarkRed;
            lblWarning.Font = new Font("Segoe UI", 8.5F);
            lblWarning.Location = new Point(12, 200);
            lblWarning.Size = new Size(776, 35);
            lblWarning.Text = "⚠ Класична помилка замикань у циклі: лямбда захоплює змінну, а не її значення!\n" +
                              "   for(int i=0;i<5;i++) new Thread(()=>Print(i)).Start();  → виведе 5,5,5,5,5 (не 0,1,2,3,4). Виправлення: var copy=i;";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 248);
            Controls.Add(lblTitle);
            Controls.Add(lblSample5a);
            Controls.Add(lblSample5b);
            Controls.Add(btnParameterizedThread);
            Controls.Add(btnClosureParameter);
            Controls.Add(textBox1);
            Controls.Add(lblWarning);
            Name = "Form1";
            Text = "2.3 — Thread Parameter: Start(param) vs Closure";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblSample5a;
        private Label lblSample5b;
        private Label lblWarning;
        private Button btnParameterizedThread;
        private Button btnClosureParameter;
        private TextBox textBox1;
    }
}