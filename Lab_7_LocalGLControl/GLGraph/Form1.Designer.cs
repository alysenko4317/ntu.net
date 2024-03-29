﻿
namespace GLGraph
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
            this.components = new System.ComponentModel.Container();
            this.AnT = new OpenTK.WinForms.GLControl();
            this.PointInGraph = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // AnT
            // 
            this.AnT.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            this.AnT.APIVersion = new System.Version(3, 3, 0, 0);
            this.AnT.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            this.AnT.IsEventDriven = true;
            this.AnT.Location = new System.Drawing.Point(21, 12);
            this.AnT.Name = "AnT";
            this.AnT.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            this.AnT.Size = new System.Drawing.Size(754, 426);
            this.AnT.TabIndex = 0;
            this.AnT.Text = "glControl1";
            this.AnT.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AnT_MouseMove);
            // 
            // PointInGraph
            // 
            this.PointInGraph.Interval = 30;
            this.PointInGraph.Tick += new System.EventHandler(this.PointInGraph_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.AnT);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.WinForms.GLControl AnT;
        private System.Windows.Forms.Timer PointInGraph;
    }
}

