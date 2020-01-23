using GPNetworkClient;
using GPNetworkMessage;
//using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace ClientForm
{
	public class CMain : Form
	{
		private IClient client = new UDPClient();

		private string username;

		private Thread gameThread;

		private bool isGameRunning;

		private IContainer components;

		private Label HostLabel;

		private TextBox HostText;

		private Label PortLabel;

		private TextBox PortText;

		private Label UsernameLabel;

		private TextBox UsernameText;

		private Button ConnectButton;

		private Button SendMessageButton;

		private TextBox MessageBox;

		private TextBox SendMessageText;

		private Button DisconnectButton;

		private Button PlayGameButton;

		private System.Windows.Forms.Timer ChatTimer;

		public CMain()
		{
			this.InitializeComponent();
			Application.ApplicationExit += new EventHandler(this.Application_ApplicationExit);
		}

		private void Application_ApplicationExit(object sender, EventArgs e)
		{
			this.client.Disconnect(string.Concat(this.username, " has left the chat"));
		}

		private void ChatTimer_Tick(object sender, EventArgs e)
		{
			if (!this.isGameRunning)
			{
				while (this.client.Messages.Count != 0)
				{
					AMessage aMessage = this.client.Messages.Dequeue();
					if (aMessage.Type == MessageType.LEAVE)
					{
						if (aMessage.ID == -1)
						{
							this.Disconnect();
						}
						this.WriteMessage(aMessage.Message);
					}
					else if (aMessage.Type == MessageType.CLIENTCOUNT)
					{
						aMessage.Message = string.Concat("Total Players: ", this.client.ClientAmount);
						this.WriteMessage(aMessage.Message);
					}
					else if (aMessage.Type != MessageType.ERROR)
					{
						string[] strArrays = aMessage.Message.Split(new char[] { ';' });
						if ((int)strArrays.Length < 2)
						{
							continue;
						}
						if (strArrays[0] == "MSG")
						{
							aMessage.Message = aMessage.Message.Substring(4);
							this.WriteMessage(aMessage.Message);
						}
						else if (strArrays[0] != "GAME")
						{
							this.WriteMessage(aMessage.Message);
						}
						else
						{
							aMessage.Message = aMessage.Message.Substring(5);
							this.WriteMessage(aMessage.Message);
							this.StartNewGame(aMessage.ID == this.client.ClientID);
						}
					}
					else
					{
						this.WriteMessage(aMessage.Message);
						this.Disconnect();
					}
				}
			}
		}

		public void Connect()
		{
			try
			{
				this.username = this.UsernameText.Text;
				int num = Convert.ToInt32(this.PortText.Text);
				if (this.client.Connect(this.HostText.Text, num, string.Concat(this.username, " has joined the chat")))
				{
					this.HostText.Enabled = false;
					this.PortText.Enabled = false;
					this.UsernameText.Enabled = false;
					this.ConnectButton.Enabled = false;
					this.SendMessageButton.Enabled = true;
					this.SendMessageText.Enabled = true;
					this.DisconnectButton.Enabled = true;
					this.PlayGameButton.Enabled = true;
					this.MessageBox.Clear();
				}
			}
			catch (Exception exception)
			{
				this.WriteMessage(exception.Message);
				this.HostText.Enabled = true;
				this.PortText.Enabled = true;
				this.UsernameText.Enabled = true;
				this.ConnectButton.Enabled = true;
				this.SendMessageButton.Enabled = false;
				this.SendMessageText.Enabled = false;
				this.DisconnectButton.Enabled = false;
				this.PlayGameButton.Enabled = false;
			}
		}

		private void ConnectButton_Click(object sender, EventArgs e)
		{
			this.Connect();
		}

		public void Disconnect()
		{
			try
			{
				this.client.Disconnect(string.Concat(this.username, " has left the chat"));
				this.client = new UDPClient();
				if (this.gameThread != null && this.gameThread.IsAlive)
				{
					this.gameThread.Abort();
					this.gameThread.Join();
					this.gameThread = null;
				}
				this.HostText.Enabled = true;
				this.PortText.Enabled = true;
				this.UsernameText.Enabled = true;
				this.ConnectButton.Enabled = true;
				this.SendMessageButton.Enabled = false;
				this.SendMessageText.Enabled = false;
				this.DisconnectButton.Enabled = false;
				this.PlayGameButton.Enabled = false;
			}
			catch (Exception exception)
			{
				this.WriteMessage(exception.Message);
				this.HostText.Enabled = true;
				this.PortText.Enabled = true;
				this.UsernameText.Enabled = true;
				this.ConnectButton.Enabled = true;
				this.SendMessageButton.Enabled = false;
				this.SendMessageText.Enabled = false;
				this.DisconnectButton.Enabled = false;
				this.PlayGameButton.Enabled = false;
				if (this.gameThread != null && this.gameThread.IsAlive)
				{
					this.gameThread.Abort();
					this.gameThread.Join();
					this.gameThread = null;
				}
			}
		}

		private void DisconnectButton_Click(object sender, EventArgs e)
		{
			this.Disconnect();
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
			this.HostLabel = new System.Windows.Forms.Label();
			this.HostText = new System.Windows.Forms.TextBox();
			this.PortLabel = new System.Windows.Forms.Label();
			this.PortText = new System.Windows.Forms.TextBox();
			this.ConnectButton = new System.Windows.Forms.Button();
			this.UsernameText = new System.Windows.Forms.TextBox();
			this.UsernameLabel = new System.Windows.Forms.Label();
			this.SendMessageButton = new System.Windows.Forms.Button();
			this.MessageBox = new System.Windows.Forms.TextBox();
			this.SendMessageText = new System.Windows.Forms.TextBox();
			this.DisconnectButton = new System.Windows.Forms.Button();
			this.PlayGameButton = new System.Windows.Forms.Button();
			this.ChatTimer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// HostLabel
			// 
			this.HostLabel.AutoSize = true;
			this.HostLabel.Location = new System.Drawing.Point(461, 188);
			this.HostLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.HostLabel.Name = "HostLabel";
			this.HostLabel.Size = new System.Drawing.Size(93, 17);
			this.HostLabel.TabIndex = 0;
			this.HostLabel.Text = "Host Address";
			// 
			// HostText
			// 
			this.HostText.Location = new System.Drawing.Point(563, 185);
			this.HostText.Margin = new System.Windows.Forms.Padding(4);
			this.HostText.Name = "HostText";
			this.HostText.Size = new System.Drawing.Size(132, 22);
			this.HostText.TabIndex = 1;
			// 
			// PortLabel
			// 
			this.PortLabel.AutoSize = true;
			this.PortLabel.Location = new System.Drawing.Point(520, 219);
			this.PortLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.PortLabel.Name = "PortLabel";
			this.PortLabel.Size = new System.Drawing.Size(34, 17);
			this.PortLabel.TabIndex = 2;
			this.PortLabel.Text = "Port";
			// 
			// PortText
			// 
			this.PortText.Location = new System.Drawing.Point(563, 215);
			this.PortText.Margin = new System.Windows.Forms.Padding(4);
			this.PortText.Name = "PortText";
			this.PortText.Size = new System.Drawing.Size(132, 22);
			this.PortText.TabIndex = 3;
			// 
			// ConnectButton
			// 
			this.ConnectButton.Location = new System.Drawing.Point(501, 278);
			this.ConnectButton.Margin = new System.Windows.Forms.Padding(4);
			this.ConnectButton.Name = "ConnectButton";
			this.ConnectButton.Size = new System.Drawing.Size(93, 28);
			this.ConnectButton.TabIndex = 5;
			this.ConnectButton.Text = "Connect";
			this.ConnectButton.UseVisualStyleBackColor = true;
			this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
			// 
			// UsernameText
			// 
			this.UsernameText.Location = new System.Drawing.Point(563, 247);
			this.UsernameText.Margin = new System.Windows.Forms.Padding(4);
			this.UsernameText.Name = "UsernameText";
			this.UsernameText.Size = new System.Drawing.Size(132, 22);
			this.UsernameText.TabIndex = 4;
			// 
			// UsernameLabel
			// 
			this.UsernameLabel.AutoSize = true;
			this.UsernameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.UsernameLabel.Location = new System.Drawing.Point(471, 251);
			this.UsernameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.UsernameLabel.Name = "UsernameLabel";
			this.UsernameLabel.Size = new System.Drawing.Size(81, 17);
			this.UsernameLabel.TabIndex = 6;
			this.UsernameLabel.Text = "Username";
			// 
			// SendMessageButton
			// 
			this.SendMessageButton.Enabled = false;
			this.SendMessageButton.Location = new System.Drawing.Point(16, 278);
			this.SendMessageButton.Margin = new System.Windows.Forms.Padding(4);
			this.SendMessageButton.Name = "SendMessageButton";
			this.SendMessageButton.Size = new System.Drawing.Size(100, 28);
			this.SendMessageButton.TabIndex = 7;
			this.SendMessageButton.Text = "Send";
			this.SendMessageButton.UseVisualStyleBackColor = true;
			this.SendMessageButton.Click += new System.EventHandler(this.SendMessageButton_Click);
			// 
			// MessageBox
			// 
			this.MessageBox.Location = new System.Drawing.Point(16, 15);
			this.MessageBox.Margin = new System.Windows.Forms.Padding(4);
			this.MessageBox.Multiline = true;
			this.MessageBox.Name = "MessageBox";
			this.MessageBox.ReadOnly = true;
			this.MessageBox.Size = new System.Drawing.Size(369, 256);
			this.MessageBox.TabIndex = 9;
			// 
			// SendMessageText
			// 
			this.SendMessageText.Enabled = false;
			this.SendMessageText.Location = new System.Drawing.Point(123, 281);
			this.SendMessageText.Margin = new System.Windows.Forms.Padding(4);
			this.SendMessageText.Name = "SendMessageText";
			this.SendMessageText.Size = new System.Drawing.Size(261, 22);
			this.SendMessageText.TabIndex = 9;
			// 
			// DisconnectButton
			// 
			this.DisconnectButton.Enabled = false;
			this.DisconnectButton.Location = new System.Drawing.Point(603, 278);
			this.DisconnectButton.Margin = new System.Windows.Forms.Padding(4);
			this.DisconnectButton.Name = "DisconnectButton";
			this.DisconnectButton.Size = new System.Drawing.Size(93, 28);
			this.DisconnectButton.TabIndex = 8;
			this.DisconnectButton.Text = "Disconnect";
			this.DisconnectButton.UseVisualStyleBackColor = true;
			this.DisconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
			// 
			// PlayGameButton
			// 
			this.PlayGameButton.Enabled = false;
			this.PlayGameButton.Location = new System.Drawing.Point(393, 278);
			this.PlayGameButton.Margin = new System.Windows.Forms.Padding(4);
			this.PlayGameButton.Name = "PlayGameButton";
			this.PlayGameButton.Size = new System.Drawing.Size(100, 28);
			this.PlayGameButton.TabIndex = 6;
			this.PlayGameButton.Text = "Play As Host";
			this.PlayGameButton.UseVisualStyleBackColor = true;
			this.PlayGameButton.Click += new System.EventHandler(this.PlayGameButton_Click);
			// 
			// ChatTimer
			// 
			this.ChatTimer.Enabled = true;
			this.ChatTimer.Tick += new System.EventHandler(this.ChatTimer_Tick);
			// 
			// CMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(712, 322);
			this.Controls.Add(this.PlayGameButton);
			this.Controls.Add(this.DisconnectButton);
			this.Controls.Add(this.SendMessageText);
			this.Controls.Add(this.MessageBox);
			this.Controls.Add(this.SendMessageButton);
			this.Controls.Add(this.UsernameLabel);
			this.Controls.Add(this.UsernameText);
			this.Controls.Add(this.ConnectButton);
			this.Controls.Add(this.PortText);
			this.Controls.Add(this.PortLabel);
			this.Controls.Add(this.HostText);
			this.Controls.Add(this.HostLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "CMain";
			this.Text = "GP Chat Lobby Client";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private void Main_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return && this.SendMessageText.TextLength != 0)
			{
				this.client.SendMessage(MessageType.INFO, string.Concat("MSG;", this.username, ": ", this.SendMessageText.Text));
				this.SendMessageText.Clear();
			}
		}

		private void PlayGameButton_Click(object sender, EventArgs e)
		{
			this.client.SendMessage(MessageType.CLIENTCOUNT, "ClientCount");
			this.client.SendMessage(MessageType.INFO, string.Concat("GAME;", this.username, " started a new game"));
		}

		private void SendMessageButton_Click(object sender, EventArgs e)
		{
			if (this.SendMessageText.TextLength != 0)
			{
				this.client.SendMessage(MessageType.INFO, string.Concat("MSG;", this.username, ": ", this.SendMessageText.Text));
				this.SendMessageText.Clear();
			}
		}

		private void StartNewGame(bool host = false)
		{
			//this.gameThread = new Thread(() => {
			//	using (Game1 game1 = new Game1(this.client, this.username))
			//	{
			//		this.isGameRunning = true;
			//		game1.Run();
			//	}
			//});
			//this.gameThread.Start();
			//this.gameThread.Join();
		}

		private void WriteMessage(string message)
		{
			List<string> strs = new List<string>();
			while (this.MessageBox.TextLength + message.Length > this.MessageBox.MaxLength)
			{
				strs.Clear();
				strs.InsertRange(1, this.MessageBox.Lines);
				this.MessageBox.Lines = strs.ToArray();
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