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
            this.Target = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Target)).BeginInit();
            this.SuspendLayout();
            // 
            // Target
            // 
            this.Target.BackColor = System.Drawing.Color.Transparent;
            this.Target.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Target.ErrorImage = null;
            this.Target.Image = global::COMP4945_Assignment2.Properties.Resources.Tank_Up;
            this.Target.InitialImage = null;
            this.Target.Location = new System.Drawing.Point(216, 236);
            this.Target.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Target.Name = "Target";
            this.Target.Size = new System.Drawing.Size(133, 119);
            this.Target.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Target.TabIndex = 1;
            this.Target.TabStop = false;
            // 
            // GameArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1333, 1192);
            this.Controls.Add(this.Target);
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "GameArea";
            this.Text = "GameArea";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyEvent);
            ((System.ComponentModel.ISupportInitialize)(this.Target)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private PictureBox Target;
    }
}