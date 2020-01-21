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
        private string dir = "Up";
        private int dir2 = 0; // Represents the direction of the tank, starting at the top as 0 and increments in clockwise
        private List<Bullet> bullets;

        public GameArea()
        {
            InitializeComponent();
            Timer gameTime = new Timer();
            gameTime.Enabled = true;
            gameTime.Interval = 1;
            gameTime.Tick += new EventHandler(OnGameTimeTick);
            bullets = new List<Bullet>();
            Target.Location = new Point(rnd.Next(0, this.ClientRectangle.Width), rnd.Next(0, this.ClientRectangle.Height));
        }

        private void Form1_KeyEvent(object sender, KeyEventArgs e)
        {


            int offset = 10;
            if (e.KeyCode.ToString() == "A" || e.KeyCode.ToString() == "Left")
            {
                if (ProtoTank.Left > 0)
                {
                    dir = "Left";
                    dir2 = 3;
                    ProtoTank.Location = new Point(ProtoTank.Location.X - offset, ProtoTank.Location.Y);

                    ProtoTank.Image = Properties.Resources.Tank_Left;
                }

            }
            else if (e.KeyCode.ToString() == "W" || e.KeyCode.ToString() == "Up")
            {
                if (ProtoTank.Top > 0)
                {
                    dir = "Up";
                    dir2 = 0;
                    ProtoTank.Image = Properties.Resources.Tank_Up;
                    ProtoTank.Location = new Point(ProtoTank.Location.X, ProtoTank.Location.Y - offset);
                }

            }
            else if (e.KeyCode.ToString() == "S" || e.KeyCode.ToString() == "Down")
            {
                if (ProtoTank.Top + ProtoTank.Height < this.ClientRectangle.Height)
                {
                    dir = "Down";
                    dir2 = 2;
                    ProtoTank.Image = Properties.Resources.Tank_Down;
                    ProtoTank.Location = new Point(ProtoTank.Location.X, ProtoTank.Location.Y + offset);
                }
            }
            else if (e.KeyCode.ToString() == "D" || e.KeyCode.ToString() == "Right")
            {

                if (ProtoTank.Left + ProtoTank.Width < this.ClientRectangle.Width)
                {
                    dir = "Right";
                    dir2 = 1;
                    ProtoTank.Image = Properties.Resources.Tank_Right;
                    ProtoTank.Location = new Point(ProtoTank.Location.X + offset, ProtoTank.Location.Y);
                }
            }
            else if (e.KeyCode.ToString() == "Space")
            {
                Bullet b = null;
                if (dir.Equals("Up") || dir.Equals("Down"))
                {
                    b = new Bullet(dir2, new Point(ProtoTank.Location.X + 20, ProtoTank.Location.Y), 0);
                }
                if (dir.Equals("Left") || dir.Equals("Right"))
                {
                    b = new Bullet(dir2, new Point(ProtoTank.Location.X, ProtoTank.Location.Y+20), 0);
                }
                bullets.Add(b);
                this.Controls.Add(b.image);

                //this.Controls.Add(picture);
            }
        }

        void OnGameTimeTick(object sender, EventArgs e)
        {
            if (bullets.Count == 0)
                return;
            for (int i = bullets.Count-1; i > -1; i--)
            {
                Bullet b = bullets[i];
                b.Move();
                PictureBox p = b.image;
                if (p.Location.X < 0 || p.Location.Y < 0 || p.Location.X > this.ClientRectangle.Width || p.Location.Y > this.ClientRectangle.Height)
                {
                    RemoveBullet(b);
                } else
                {
                    if (p.Bounds.IntersectsWith(Target.Bounds))
                    {
                        TargetDestroyed();
                        RemoveBullet(b);
                    }
                }
            }
        }
        void RemoveBullet(Bullet b)
        {
            this.Controls.Remove(b.image);
            bullets.Remove(b);
            b.image.Dispose();
        }

        void TargetDestroyed()
        {
            Target.Location = new Point(rnd.Next(0, this.ClientRectangle.Width), rnd.Next(0, this.ClientRectangle.Height));
        }
    }
}
