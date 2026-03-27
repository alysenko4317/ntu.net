namespace CalcApp
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
            button0 = new Button();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button6 = new Button();
            button5 = new Button();
            button4 = new Button();
            button9 = new Button();
            button8 = new Button();
            button7 = new Button();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // button0
            // 
            button0.Location = new Point(98, 165);
            button0.Name = "button0";
            button0.Size = new Size(87, 23);
            button0.TabIndex = 0;
            button0.Text = "0";
            button0.UseVisualStyleBackColor = true;
            button0.Click += button_digit_click;
            // 
            // button1
            // 
            button1.Location = new Point(98, 78);
            button1.Name = "button1";
            button1.Size = new Size(25, 23);
            button1.TabIndex = 1;
            button1.Text = "1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button_digit_click;
            // 
            // button2
            // 
            button2.Location = new Point(129, 78);
            button2.Name = "button2";
            button2.Size = new Size(25, 23);
            button2.TabIndex = 2;
            button2.Text = "2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button_digit_click;
            // 
            // button3
            // 
            button3.Location = new Point(160, 78);
            button3.Name = "button3";
            button3.Size = new Size(25, 23);
            button3.TabIndex = 3;
            button3.Text = "3";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button_digit_click;
            // 
            // button6
            // 
            button6.Location = new Point(160, 107);
            button6.Name = "button6";
            button6.Size = new Size(25, 23);
            button6.TabIndex = 6;
            button6.Text = "6";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button_digit_click;
            // 
            // button5
            // 
            button5.Location = new Point(129, 107);
            button5.Name = "button5";
            button5.Size = new Size(25, 23);
            button5.TabIndex = 5;
            button5.Text = "5";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button_digit_click;
            // 
            // button4
            // 
            button4.Location = new Point(98, 107);
            button4.Name = "button4";
            button4.Size = new Size(25, 23);
            button4.TabIndex = 4;
            button4.Text = "4";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button_digit_click;
            // 
            // button9
            // 
            button9.Location = new Point(160, 136);
            button9.Name = "button9";
            button9.Size = new Size(25, 23);
            button9.TabIndex = 9;
            button9.Text = "9";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button_digit_click;
            // 
            // button8
            // 
            button8.Location = new Point(129, 136);
            button8.Name = "button8";
            button8.Size = new Size(25, 23);
            button8.TabIndex = 8;
            button8.Text = "8";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button_digit_click;
            // 
            // button7
            // 
            button7.Location = new Point(98, 136);
            button7.Name = "button7";
            button7.Size = new Size(25, 23);
            button7.TabIndex = 7;
            button7.Text = "7";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button_digit_click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(98, 49);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(87, 23);
            textBox1.TabIndex = 10;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textBox1);
            Controls.Add(button9);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(button0);
            Name = "Form1";
            Text = "Form1";
            KeyPress += Form1_KeyPress;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button0;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button6;
        private Button button5;
        private Button button4;
        private Button button9;
        private Button button8;
        private Button button7;
        private TextBox textBox1;
    }
}