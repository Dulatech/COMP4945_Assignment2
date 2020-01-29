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
            this.SuspendLayout();
            // 
            // GameArea
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::COMP4945_Assignment2.Properties.Resources._10_tiled_1;
            this.ClientSize = new System.Drawing.Size(700, 600);
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

        }

        #endregion
    }
}