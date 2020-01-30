namespace COMP4945_Assignment2

{
    using System;
    using System.Drawing;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Data;
    using System.Drawing.Drawing2D;

    partial class GameArea : System.Windows.Forms.Form
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
            this.tank_label = new System.Windows.Forms.Label();
            this.plane_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tank_label
            // 
            this.tank_label.AutoSize = true;
            this.tank_label.BackColor = System.Drawing.Color.White;
            this.tank_label.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tank_label.Location = new System.Drawing.Point(-1, 0);
            this.tank_label.Name = "tank_label";
            this.tank_label.Size = new System.Drawing.Size(119, 31);
            this.tank_label.TabIndex = 0;
            this.tank_label.Text = "Tanks: 0";
            // 
            // plane_label
            // 
            this.plane_label.AutoSize = true;
            this.plane_label.BackColor = System.Drawing.Color.White;
            this.plane_label.Location = new System.Drawing.Point(-1, 31);
            this.plane_label.Name = "plane_label";
            this.plane_label.Size = new System.Drawing.Size(127, 31);
            this.plane_label.TabIndex = 1;
            this.plane_label.Text = "Planes: 0";
            // 
            // GameArea
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::COMP4945_Assignment2.Properties.Resources._10_tiled_1;
            this.ClientSize = new System.Drawing.Size(700, 600);
            this.Controls.Add(this.plane_label);
            this.Controls.Add(this.tank_label);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "GameArea";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tanks vs Plane";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameArea_FormClosing);
            this.Shown += new System.EventHandler(this.GameArea_Shown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GameArea_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyEvent);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label tank_label;
        private Label plane_label;
    }
}