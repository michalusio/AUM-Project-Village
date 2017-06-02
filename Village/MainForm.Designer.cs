using System;
using System.Windows.Forms;

namespace Village
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ui = new System.Windows.Forms.PictureBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.grassGrowth = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.simSpeed = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.agingSpeed = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ui)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grassGrowth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.agingSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // ui
            // 
            this.ui.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ui.Location = new System.Drawing.Point(0, 0);
            this.ui.Name = "ui";
            this.ui.Size = new System.Drawing.Size(100, 50);
            this.ui.TabIndex = 0;
            this.ui.TabStop = false;
            this.ui.Paint += new System.Windows.Forms.PaintEventHandler(this.ui_Paint);
            this.ui.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ui_MouseClick);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 16;
            this.timer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // grassGrowth
            // 
            this.grassGrowth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grassGrowth.Location = new System.Drawing.Point(0, 813);
            this.grassGrowth.Name = "grassGrowth";
            this.grassGrowth.Size = new System.Drawing.Size(104, 45);
            this.grassGrowth.TabIndex = 1;
            this.grassGrowth.Value = 3;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 797);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Grass Growth";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(797, 813);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 36);
            this.button1.TabIndex = 3;
            this.button1.Text = "RESET";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // simSpeed
            // 
            this.simSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simSpeed.Location = new System.Drawing.Point(768, 762);
            this.simSpeed.Name = "simSpeed";
            this.simSpeed.Size = new System.Drawing.Size(104, 45);
            this.simSpeed.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(783, 746);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Simulation Speed";
            // 
            // agingSpeed
            // 
            this.agingSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.agingSpeed.Location = new System.Drawing.Point(0, 746);
            this.agingSpeed.Name = "agingSpeed";
            this.agingSpeed.Size = new System.Drawing.Size(104, 45);
            this.agingSpeed.TabIndex = 6;
            this.agingSpeed.Value = 2;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 730);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Aging Speed";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(884, 861);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.agingSpeed);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.simSpeed);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grassGrowth);
            this.Controls.Add(this.ui);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(900, 858);
            this.Name = "Form1";
            this.Text = "Village Project";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseWheel);
            ((System.ComponentModel.ISupportInitialize)(this.ui)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grassGrowth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.agingSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox ui;
        private System.Windows.Forms.Timer timer;
        private TrackBar grassGrowth;
        private Label label1;
        private Button button1;
        private TrackBar simSpeed;
        private Label label2;
        private TrackBar agingSpeed;
        private Label label3;
    }
}

