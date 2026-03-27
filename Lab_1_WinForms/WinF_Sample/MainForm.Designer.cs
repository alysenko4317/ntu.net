
namespace WinF_Sample
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_loadPNG = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_loadJPG = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_openPreview = new System.Windows.Forms.ToolStripButton();
            this.pictureBoxControl = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxControl)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(544, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem,
            this.loadToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.loadToolStripMenuItem.Text = "Load...";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.onLoadJPG_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_loadPNG,
            this.toolStripButton_loadJPG,
            this.toolStripButton_openPreview});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(44, 359);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_loadPNG
            // 
            this.toolStripButton_loadPNG.AutoSize = false;
            this.toolStripButton_loadPNG.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_loadPNG.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_loadPNG.Image")));
            this.toolStripButton_loadPNG.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton_loadPNG.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_loadPNG.Name = "toolStripButton_loadPNG";
            this.toolStripButton_loadPNG.Size = new System.Drawing.Size(42, 42);
            this.toolStripButton_loadPNG.Text = "toolStripButton1";
            this.toolStripButton_loadPNG.Click += new System.EventHandler(this.onLoadJPG_Click);
            // 
            // toolStripButton_loadJPG
            // 
            this.toolStripButton_loadJPG.AutoSize = false;
            this.toolStripButton_loadJPG.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_loadJPG.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_loadJPG.Image")));
            this.toolStripButton_loadJPG.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton_loadJPG.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_loadJPG.Name = "toolStripButton_loadJPG";
            this.toolStripButton_loadJPG.Size = new System.Drawing.Size(42, 42);
            this.toolStripButton_loadJPG.Text = "toolStripButton2";
            this.toolStripButton_loadJPG.Click += new System.EventHandler(this.onLoadPNG_Click);
            // 
            // toolStripButton_openPreview
            // 
            this.toolStripButton_openPreview.AutoSize = false;
            this.toolStripButton_openPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_openPreview.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_openPreview.Image")));
            this.toolStripButton_openPreview.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton_openPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_openPreview.Name = "toolStripButton_openPreview";
            this.toolStripButton_openPreview.Size = new System.Drawing.Size(42, 42);
            this.toolStripButton_openPreview.Text = "toolStripButton3";
            this.toolStripButton_openPreview.Click += new System.EventHandler(this.onOpenPreview_Click);
            // 
            // pictureBoxControl
            // 
            this.pictureBoxControl.Location = new System.Drawing.Point(47, 27);
            this.pictureBoxControl.Name = "pictureBoxControl";
            this.pictureBoxControl.Size = new System.Drawing.Size(497, 356);
            this.pictureBoxControl.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxControl.TabIndex = 2;
            this.pictureBoxControl.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 383);
            this.Controls.Add(this.pictureBoxControl);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_loadPNG;
        private System.Windows.Forms.ToolStripButton toolStripButton_loadJPG;
        private System.Windows.Forms.ToolStripButton toolStripButton_openPreview;
        private System.Windows.Forms.PictureBox pictureBoxControl;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

