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
            this.ProtoTank = new System.Windows.Forms.PictureBox();
            this.Target = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ProtoTank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Target)).BeginInit();
            this.SuspendLayout();
            // 
            // ProtoTank
            // 
            this.ProtoTank.BackColor = System.Drawing.Color.Transparent;
            this.ProtoTank.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ProtoTank.ErrorImage = null;
            this.ProtoTank.Image = global::COMP4945_Assignment2.Properties.Resources.Tank_Up;
            this.ProtoTank.InitialImage = null;
            this.ProtoTank.Location = new System.Drawing.Point(250, 250);
            this.ProtoTank.Name = "ProtoTank";
            this.ProtoTank.Size = new System.Drawing.Size(50, 50);
            this.ProtoTank.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ProtoTank.TabIndex = 0;
            this.ProtoTank.TabStop = false;
            // 
            // pictureBox1
            // 
            this.Target.BackColor = System.Drawing.Color.Transparent;
            this.Target.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Target.ErrorImage = null;
            this.Target.Image = global::COMP4945_Assignment2.Properties.Resources.Tank_Up;
            this.Target.InitialImage = null;
            this.Target.Location = new System.Drawing.Point(81, 99);
            this.Target.Name = "pictureBox1";
            this.Target.Size = new System.Drawing.Size(50, 50);
            this.Target.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Target.TabIndex = 1;
            this.Target.TabStop = false;
            // 
            // GameArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(500, 500);
            this.Controls.Add(this.Target);
            this.Controls.Add(this.ProtoTank);
            this.Name = "GameArea";
            this.Text = "GameArea";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyEvent);
            ((System.ComponentModel.ISupportInitialize)(this.ProtoTank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Target)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox ProtoTank;
        private PictureBox Target;
    }
}