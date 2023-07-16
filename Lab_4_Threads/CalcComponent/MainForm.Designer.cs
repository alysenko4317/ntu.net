
namespace CalcComponent
{
    partial class MainForm
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
            this._labelFactorial1 = new System.Windows.Forms.Label();
            this._labelFactorial2 = new System.Windows.Forms.Label();
            this._labelAddTwo = new System.Windows.Forms.Label();
            this._labelRunLoops = new System.Windows.Forms.Label();
            this._labelTotalCalculations = new System.Windows.Forms.Label();
            this._buttonFactorial1 = new System.Windows.Forms.Button();
            this._buttonFactorial2 = new System.Windows.Forms.Button();
            this._buttonAddTwo = new System.Windows.Forms.Button();
            this._buttonRunLoops = new System.Windows.Forms.Button();
            this._textValue = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _labelFactorial1
            // 
            this._labelFactorial1.AutoSize = true;
            this._labelFactorial1.Location = new System.Drawing.Point(150, 43);
            this._labelFactorial1.Name = "_labelFactorial1";
            this._labelFactorial1.Size = new System.Drawing.Size(38, 15);
            this._labelFactorial1.TabIndex = 0;
            this._labelFactorial1.Text = "label1";
            // 
            // _labelFactorial2
            // 
            this._labelFactorial2.AutoSize = true;
            this._labelFactorial2.Location = new System.Drawing.Point(150, 73);
            this._labelFactorial2.Name = "_labelFactorial2";
            this._labelFactorial2.Size = new System.Drawing.Size(38, 15);
            this._labelFactorial2.TabIndex = 1;
            this._labelFactorial2.Text = "label2";
            // 
            // _labelAddTwo
            // 
            this._labelAddTwo.AutoSize = true;
            this._labelAddTwo.Location = new System.Drawing.Point(150, 103);
            this._labelAddTwo.Name = "_labelAddTwo";
            this._labelAddTwo.Size = new System.Drawing.Size(38, 15);
            this._labelAddTwo.TabIndex = 2;
            this._labelAddTwo.Text = "label3";
            // 
            // _labelRunLoops
            // 
            this._labelRunLoops.AutoSize = true;
            this._labelRunLoops.Location = new System.Drawing.Point(150, 133);
            this._labelRunLoops.Name = "_labelRunLoops";
            this._labelRunLoops.Size = new System.Drawing.Size(38, 15);
            this._labelRunLoops.TabIndex = 3;
            this._labelRunLoops.Text = "label4";
            // 
            // _labelTotalCalculations
            // 
            this._labelTotalCalculations.AutoSize = true;
            this._labelTotalCalculations.Location = new System.Drawing.Point(150, 164);
            this._labelTotalCalculations.Name = "_labelTotalCalculations";
            this._labelTotalCalculations.Size = new System.Drawing.Size(38, 15);
            this._labelTotalCalculations.TabIndex = 4;
            this._labelTotalCalculations.Text = "label5";
            // 
            // _buttonFactorial1
            // 
            this._buttonFactorial1.Location = new System.Drawing.Point(12, 35);
            this._buttonFactorial1.Name = "_buttonFactorial1";
            this._buttonFactorial1.Size = new System.Drawing.Size(103, 23);
            this._buttonFactorial1.TabIndex = 5;
            this._buttonFactorial1.Text = "Факторіал";
            this._buttonFactorial1.UseVisualStyleBackColor = true;
            this._buttonFactorial1.Click += new System.EventHandler(this.buttonFactorial1_Click);
            // 
            // _buttonFactorial2
            // 
            this._buttonFactorial2.Location = new System.Drawing.Point(12, 65);
            this._buttonFactorial2.Name = "_buttonFactorial2";
            this._buttonFactorial2.Size = new System.Drawing.Size(103, 23);
            this._buttonFactorial2.TabIndex = 6;
            this._buttonFactorial2.Text = "Факторіал - 1";
            this._buttonFactorial2.UseVisualStyleBackColor = true;
            this._buttonFactorial2.Click += new System.EventHandler(this.buttonFactorial2_Click);
            // 
            // _buttonAddTwo
            // 
            this._buttonAddTwo.Location = new System.Drawing.Point(12, 95);
            this._buttonAddTwo.Name = "_buttonAddTwo";
            this._buttonAddTwo.Size = new System.Drawing.Size(103, 23);
            this._buttonAddTwo.TabIndex = 7;
            this._buttonAddTwo.Text = "Додати два";
            this._buttonAddTwo.UseVisualStyleBackColor = true;
            this._buttonAddTwo.Click += new System.EventHandler(this.buttonAddTwo_Click);
            // 
            // _buttonRunLoops
            // 
            this._buttonRunLoops.Location = new System.Drawing.Point(12, 125);
            this._buttonRunLoops.Name = "_buttonRunLoops";
            this._buttonRunLoops.Size = new System.Drawing.Size(103, 23);
            this._buttonRunLoops.TabIndex = 8;
            this._buttonRunLoops.Text = "Виконати цикл";
            this._buttonRunLoops.UseVisualStyleBackColor = true;
            this._buttonRunLoops.Click += new System.EventHandler(this.buttonRunLoops_Click);
            // 
            // _textValue
            // 
            this._textValue.Location = new System.Drawing.Point(150, 199);
            this._textValue.Name = "_textValue";
            this._textValue.Size = new System.Drawing.Size(100, 23);
            this._textValue.TabIndex = 9;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 245);
            this.Controls.Add(this._textValue);
            this.Controls.Add(this._buttonRunLoops);
            this.Controls.Add(this._buttonAddTwo);
            this.Controls.Add(this._buttonFactorial2);
            this.Controls.Add(this._buttonFactorial1);
            this.Controls.Add(this._labelTotalCalculations);
            this.Controls.Add(this._labelRunLoops);
            this.Controls.Add(this._labelAddTwo);
            this.Controls.Add(this._labelFactorial2);
            this.Controls.Add(this._labelFactorial1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _labelFactorial1;
        private System.Windows.Forms.Label _labelFactorial2;
        private System.Windows.Forms.Label _labelAddTwo;
        private System.Windows.Forms.Label _labelRunLoops;
        private System.Windows.Forms.Label _labelTotalCalculations;
        private System.Windows.Forms.Button _buttonFactorial1;
        private System.Windows.Forms.Button _buttonFactorial2;
        private System.Windows.Forms.Button _buttonAddTwo;
        private System.Windows.Forms.Button _buttonRunLoops;
        private System.Windows.Forms.TextBox _textValue;
    }
}

