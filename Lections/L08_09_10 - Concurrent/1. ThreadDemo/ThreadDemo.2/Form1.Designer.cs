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
            lblSample3 = new Label();
            lblSample4 = new Label();
            lblSample4s = new Label();
            btnOpenFormsInvoke = new Button();
            btnInlineLambdaOpenForms = new Button();
            btnSimplifiedClosure = new Button();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = false;
            lblTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTitle.Location = new Point(12, 12);
            lblTitle.Size = new Size(776, 20);
            lblTitle.Text = "2.2 — Повернення результату: Invoke, OpenForms, замикання";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSample3
            // 
            lblSample3.AutoSize = false;
            lblSample3.ForeColor = Color.DarkRed;
            lblSample3.Location = new Point(12, 42);
            lblSample3.Size = new Size(248, 40);
            lblSample3.Text = "Зразок 3: Invoke через\nApplication.OpenForms (погано)";
            // 
            // lblSample4
            // 
            lblSample4.AutoSize = false;
            lblSample4.ForeColor = Color.DarkOrange;
            lblSample4.Location = new Point(275, 42);
            lblSample4.Size = new Size(248, 40);
            lblSample4.Text = "Зразок 4: inline-лямбда,\nале досі OpenForms (краще, але...)";
            // 
            // lblSample4s
            // 
            lblSample4s.AutoSize = false;
            lblSample4s.ForeColor = Color.DarkGreen;
            lblSample4s.Location = new Point(540, 42);
            lblSample4s.Size = new Size(248, 40);
            lblSample4s.Text = "Зразок 4 (спрощений): замикання\nзахоплює this — правильно! ✔";
            // 
            // btnOpenFormsInvoke
            // 
            btnOpenFormsInvoke.Location = new Point(12, 97);
            btnOpenFormsInvoke.Size = new Size(248, 40);
            btnOpenFormsInvoke.Text = "▶  Invoke через OpenForms";
            btnOpenFormsInvoke.BackColor = Color.MistyRose;
            btnOpenFormsInvoke.UseVisualStyleBackColor = false;
            btnOpenFormsInvoke.Click += btnOpenFormsInvoke_Click;
            // 
            // btnInlineLambdaOpenForms
            // 
            btnInlineLambdaOpenForms.Location = new Point(275, 97);
            btnInlineLambdaOpenForms.Size = new Size(248, 40);
            btnInlineLambdaOpenForms.Text = "▶  Inline-лямбда + OpenForms";
            btnInlineLambdaOpenForms.BackColor = Color.LightYellow;
            btnInlineLambdaOpenForms.UseVisualStyleBackColor = false;
            btnInlineLambdaOpenForms.Click += btnInlineLambdaOpenForms_Click;
            // 
            // btnSimplifiedClosure
            // 
            btnSimplifiedClosure.Location = new Point(540, 97);
            btnSimplifiedClosure.Size = new Size(248, 40);
            btnSimplifiedClosure.Text = "▶  Замикання (правильно)";
            btnSimplifiedClosure.BackColor = Color.Honeydew;
            btnSimplifiedClosure.UseVisualStyleBackColor = false;
            btnSimplifiedClosure.Click += btnSimplifiedClosure_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 153);
            textBox1.Size = new Size(776, 31);
            textBox1.ReadOnly = true;
            textBox1.Text = "Результат з'явиться тут...";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 205);
            Controls.Add(lblTitle);
            Controls.Add(lblSample3);
            Controls.Add(lblSample4);
            Controls.Add(lblSample4s);
            Controls.Add(btnOpenFormsInvoke);
            Controls.Add(btnInlineLambdaOpenForms);
            Controls.Add(btnSimplifiedClosure);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "2.2 — Invoke & Closure";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblSample3;
        private Label lblSample4;
        private Label lblSample4s;
        private Button btnOpenFormsInvoke;
        private Button btnInlineLambdaOpenForms;
        private Button btnSimplifiedClosure;
        private TextBox textBox1;
    }
}