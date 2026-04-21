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
            lblSample1 = new Label();
            lblSample2 = new Label();
            btnBlockingUI = new Button();
            btnBackgroundNoResult = new Button();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = false;
            lblTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTitle.Location = new Point(12, 12);
            lblTitle.Size = new Size(776, 20);
            lblTitle.Text = "2.1 — Перший потік: блокування UI та проблема повернення результату";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSample1
            // 
            lblSample1.AutoSize = false;
            lblSample1.ForeColor = Color.DarkRed;
            lblSample1.Location = new Point(12, 45);
            lblSample1.Size = new Size(370, 40);
            lblSample1.Text = "Зразок 1: довга операція в UI-потоці\nUI замерзає, textBox оновиться лише наприкінці";
            // 
            // lblSample2
            // 
            lblSample2.AutoSize = false;
            lblSample2.ForeColor = Color.DarkBlue;
            lblSample2.Location = new Point(400, 45);
            lblSample2.Size = new Size(388, 40);
            lblSample2.Text = "Зразок 2: фоновий потік — UI вільний\nАле як повернути результат? (дивись ThreadDemo.2)";
            // 
            // btnBlockingUI
            // 
            btnBlockingUI.Location = new Point(12, 100);
            btnBlockingUI.Size = new Size(370, 40);
            btnBlockingUI.Text = "▶  Запустити в UI-потоці (заморожує)";
            btnBlockingUI.BackColor = Color.MistyRose;
            btnBlockingUI.UseVisualStyleBackColor = false;
            btnBlockingUI.Click += btnBlockingUI_Click;
            // 
            // btnBackgroundNoResult
            // 
            btnBackgroundNoResult.Location = new Point(400, 100);
            btnBackgroundNoResult.Size = new Size(388, 40);
            btnBackgroundNoResult.Text = "▶  Запустити у фоновому потоці (результат недоступний)";
            btnBackgroundNoResult.BackColor = Color.AliceBlue;
            btnBackgroundNoResult.UseVisualStyleBackColor = false;
            btnBackgroundNoResult.Click += btnBackgroundNoResult_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 158);
            textBox1.Size = new Size(776, 31);
            textBox1.ReadOnly = true;
            textBox1.Text = "Результат з'явиться тут...";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 210);
            Controls.Add(lblTitle);
            Controls.Add(lblSample1);
            Controls.Add(lblSample2);
            Controls.Add(btnBlockingUI);
            Controls.Add(btnBackgroundNoResult);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "2.1 — UI Blocking & Background Thread";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblSample1;
        private Label lblSample2;
        private Button btnBlockingUI;
        private Button btnBackgroundNoResult;
        private TextBox textBox1;
    }
}