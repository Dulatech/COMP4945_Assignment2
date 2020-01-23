using GPNetworkServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UDPServerConnection
{
	public class SMain : Form
	{
		private IServer server = new UDPServer();

		private IContainer components;

		private Button StartButton;

		private Button StopButton;

		private TextBox IPAddressText;

		private TextBox PortTextBox;

		private TextBox MessageBox;

		private Label IPLabel;

		private Label PortLabel;

		private CheckBox LoopBackCheck;

		private Timer ServerTimer;

		public SMain()
		{
			this.InitializeComponent();
			Application.ApplicationExit += new EventHandler(this.Application_ApplicationExit);
		}

		private void Application_ApplicationExit(object sender, EventArgs e)
		{
			this.server.Stop();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.StartButton = new Button();
			this.StopButton = new Button();
			this.IPAddressText = new TextBox();
			this.PortTextBox = new TextBox();
			this.MessageBox = new TextBox();
			this.IPLabel = new Label();
			this.PortLabel = new Label();
			this.LoopBackCheck = new CheckBox();
			this.ServerTimer = new Timer(this.components);
			base.SuspendLayout();
			this.StartButton.Location = new Point(509, 244);
			this.StartButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.StartButton.Name = "StartButton";
			this.StartButton.Size = new System.Drawing.Size(120, 28);
			this.StartButton.TabIndex = 5;
			this.StartButton.Text = "Start";
			this.StartButton.UseVisualStyleBackColor = true;
			this.StartButton.Click += new EventHandler(this.StartButton_Click);
			this.StopButton.Enabled = false;
			this.StopButton.Location = new Point(509, 279);
			this.StopButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.StopButton.Name = "StopButton";
			this.StopButton.Size = new System.Drawing.Size(120, 28);
			this.StopButton.TabIndex = 6;
			this.StopButton.Text = "Stop";
			this.StopButton.UseVisualStyleBackColor = true;
			this.StopButton.Click += new EventHandler(this.StopButton_Click);
			this.IPAddressText.Location = new Point(509, 38);
			this.IPAddressText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.IPAddressText.Name = "IPAddressText";
			this.IPAddressText.Size = new System.Drawing.Size(119, 22);
			this.IPAddressText.TabIndex = 2;
			this.PortTextBox.Location = new Point(509, 86);
			this.PortTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.PortTextBox.Name = "PortTextBox";
			this.PortTextBox.Size = new System.Drawing.Size(119, 22);
			this.PortTextBox.TabIndex = 3;
			this.PortTextBox.Text = "4444";
			this.MessageBox.Location = new Point(17, 15);
			this.MessageBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.MessageBox.Multiline = true;
			this.MessageBox.Name = "MessageBox";
			this.MessageBox.ReadOnly = true;
			this.MessageBox.Size = new System.Drawing.Size(465, 292);
			this.MessageBox.TabIndex = 4;
			this.IPLabel.AutoSize = true;
			this.IPLabel.Location = new Point(505, 18);
			this.IPLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.IPLabel.Name = "IPLabel";
			this.IPLabel.Size = new System.Drawing.Size(76, 17);
			this.IPLabel.TabIndex = 5;
			this.IPLabel.Text = "IP Address";
			this.PortLabel.AutoSize = true;
			this.PortLabel.Location = new Point(505, 66);
			this.PortLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.PortLabel.Name = "PortLabel";
			this.PortLabel.Size = new System.Drawing.Size(34, 17);
			this.PortLabel.TabIndex = 6;
			this.PortLabel.Text = "Port";
			this.LoopBackCheck.AutoSize = true;
			this.LoopBackCheck.Location = new Point(509, 118);
			this.LoopBackCheck.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.LoopBackCheck.Name = "LoopBackCheck";
			this.LoopBackCheck.Size = new System.Drawing.Size(93, 21);
			this.LoopBackCheck.TabIndex = 4;
			this.LoopBackCheck.Text = "LoopBack";
			this.LoopBackCheck.UseVisualStyleBackColor = true;
			this.ServerTimer.Enabled = true;
			this.ServerTimer.Tick += new EventHandler(this.ServerTimer_Tick);
			base.AutoScaleDimensions = new SizeF(8f, 16f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(645, 322);
			base.Controls.Add(this.LoopBackCheck);
			base.Controls.Add(this.PortLabel);
			base.Controls.Add(this.IPLabel);
			base.Controls.Add(this.MessageBox);
			base.Controls.Add(this.PortTextBox);
			base.Controls.Add(this.IPAddressText);
			base.Controls.Add(this.StopButton);
			base.Controls.Add(this.StartButton);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			base.Name = "SMain";
			this.Text = "GP Network Server";
			base.Load += new EventHandler(this.Main_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void Main_Load(object sender, EventArgs e)
		{
			this.WriteMessage("Welcome to the GP Network Server.");
			this.WriteMessage("Developed by Tim Stoddard for MPGSE 2014.");
		}

		private void ServerTimer_Tick(object sender, EventArgs e)
		{
			while (this.server.MessageQueue.Count != 0)
			{
				this.WriteMessage(this.server.MessageQueue.Dequeue());
				//this.WriteMessage("hello");
			}
		}

		private void StartButton_Click(object sender, EventArgs e)
		{
			try
			{
				int num = Convert.ToInt32(this.PortTextBox.Text);
				if (this.IPAddressText.Text.Length != 0)
				{
					this.server = new UDPServer(this.IPAddressText.Text, num);
				}
				else
				{
					this.server = new UDPServer(this.LoopBackCheck.Checked, num);
				}
				this.server.Start();
				this.IPAddressText.Text = this.server.IPAddressValue;
				this.IPAddressText.ReadOnly = true;
				this.PortTextBox.ReadOnly = true;
				this.LoopBackCheck.Enabled = false;
				this.StartButton.Enabled = false;
				this.StopButton.Enabled = true;
				this.MessageBox.Clear();
				this.WriteMessage("Server is running, waiting for incoming messages...");
			}
			catch (FormatException formatException)
			{
				this.WriteMessage(formatException.Message);
				this.server = new TCPServer();
			}
		}

		private void StopButton_Click(object sender, EventArgs e)
		{
			try
			{
				this.server.Stop();
				this.IPAddressText.Text = "";
				this.IPAddressText.ReadOnly = false;
				this.PortTextBox.ReadOnly = false;
				this.LoopBackCheck.Enabled = true;
				this.StartButton.Enabled = true;
				this.StopButton.Enabled = false;
				this.MessageBox.Clear();
				this.WriteMessage("Server has stopped successfully.");
			}
			catch (Exception exception)
			{
				this.WriteMessage(exception.Message);
				this.IPAddressText.ReadOnly = false;
				this.PortTextBox.Enabled = false;
				this.LoopBackCheck.Enabled = false;
				this.StartButton.Enabled = false;
				this.StopButton.Enabled = false;
			}
		}

		private void WriteMessage(string message)
		{
			if (this.MessageBox.TextLength + "\r\n".Length + message.Length > this.MessageBox.MaxLength)
			{
				string text = this.MessageBox.Text;
				this.MessageBox.Text = text.Substring(message.Length + "\r\n".Length);
			}
			if (this.MessageBox.TextLength == 0)
			{
				this.MessageBox.Text = message;
				return;
			}
			this.MessageBox.AppendText(string.Concat("\r\n", message));
		}
	}
}