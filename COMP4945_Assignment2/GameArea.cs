using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COMP4945_Assignment2
{
    public partial class GameArea : Form
        
    {

        private Random rnd = new Random();
        private string dir = "";
        private List<Bullet> bullets;

        public GameArea()
        {
            InitializeComponent();
            Timer gameTime = new Timer();
            gameTime.Enabled = true;
            gameTime.Interval = 1;
            gameTime.Tick += new EventHandler(OnGameTimeTick);
            bullets = new List<Bullet>();
        }

        private void Form1_KeyEvent(object sender, KeyEventArgs e)
        {


            int offset = 10;
            if (e.KeyCode.ToString() == "A" || e.KeyCode.ToString() == "Left")
            {
                if (ProtoTank.Left > 0)
                {
                    dir = "Left";
                    ProtoTank.Location = new Point(ProtoTank.Location.X - offset, ProtoTank.Location.Y);

                    ProtoTank.Image = Properties.Resources.Tank_Left;
                }

            }
            else if (e.KeyCode.ToString() == "W" || e.KeyCode.ToString() == "Up")
            {
                if (ProtoTank.Top > 0)
                {
                    dir = "Up";
                    ProtoTank.Image = Properties.Resources.Tank_Up;
                    ProtoTank.Location = new Point(ProtoTank.Location.X, ProtoTank.Location.Y - offset);
                }

            }
            else if (e.KeyCode.ToString() == "S" || e.KeyCode.ToString() == "Down")
            {
                if (ProtoTank.Top + ProtoTank.Height < this.ClientRectangle.Height)
                {
                    dir = "Down";
                    ProtoTank.Image = Properties.Resources.Tank_Down;
                    ProtoTank.Location = new Point(ProtoTank.Location.X, ProtoTank.Location.Y + offset);
                }
            }
            else if (e.KeyCode.ToString() == "D" || e.KeyCode.ToString() == "Right")
            {

                if (ProtoTank.Left + ProtoTank.Width < this.ClientRectangle.Width)
                {
                    dir = "Right";
                    ProtoTank.Image = Properties.Resources.Tank_Right;
                    ProtoTank.Location = new Point(ProtoTank.Location.X + offset, ProtoTank.Location.Y);
                }
            }
            else if (e.KeyCode.ToString() == "Space")
            {

                
                Bullet b = new Bullet(1, 2, ProtoTank.Location, 0);
                bullets.Add(b);
                this.Controls.Add(b.image);

                //this.Controls.Add(picture);
            }
        }

        void OnGameTimeTick(object sender, EventArgs e)
        {
            foreach (Bullet b in bullets) {
                PictureBox p = b.image;
                if (dir.Equals("Up"))
                {
                    if (p.Top > 0)
                    {
                        p.Location = new Point(p.Location.X, p.Location.Y - b.Speed);
                    } else
                    {
                        this.Controls.Remove(p);
                        p.Dispose();
                    }
                }
                if (dir.Equals("Down"))
                {
                    if (p.Top + p.Height < this.ClientRectangle.Height)
                    {
                        p.Location = new Point(p.Location.X, p.Location.Y + b.Speed);
                    } else
                    {
                        this.Controls.Remove(p);
                        p.Dispose();
                    }
                }
                if (dir.Equals("Left"))
                {
                    if (p.Left > 0)
                    {
                        p.Location = new Point(p.Location.X - b.Speed, p.Location.Y);
                    } else
                    {
                        this.Controls.Remove(p);
                        p.Dispose();
                    }
                }
                if (dir.Equals("Right"))
                {
                    if (p.Left + p.Width < this.ClientRectangle.Width)
                    {
                        p.Location = new Point(p.Location.X + b.Speed, p.Location.Y);
                    } else
                    {
                        this.Controls.Remove(p);
                        p.Dispose();
                    }
                }
            }
            //controller.CollisionGameArea(ball);
            //controller.PaddleCollision(player, player2, ball);
        }
    }
}
