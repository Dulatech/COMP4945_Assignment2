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
        private int dir2 = 0; // Represents the direction of the tank, starting at the top as 0 and increments in clockwise
        private int dir1 = 0; // Represents the direction of the tank, starting at the top as 0 and increments in clockwise
        private List<Bullet> bullets;
        private List<Tank> tanks;
        Tank t;
        Tank f; // target tank
        Tank f2;// target tank

        public GameArea()
        {
            InitializeComponent();
            Timer gameTime = new Timer();
            gameTime.Enabled = true;
            gameTime.Interval = 1;
            gameTime.Tick += new EventHandler(OnGameTimeTick);
            bullets = new List<Bullet>();
            tanks = new List<Tank>();
            //Target.Location = new Point(rnd.Next(0, this.ClientRectangle.Width), rnd.Next(0, this.ClientRectangle.Height));
            t = new Tank(new Point(250, 250), 0);
            this.Controls.Add(t.tank);
            tanks.Add(t);
            f = new Tank(new Point(100, 100), 1);
            this.Controls.Add(f.tank);
            tanks.Add(f);
            //targets
            //f = new Tank(new Point(100, 250), 0); 
            //f2 = new Tank(new Point(110, 50), 0);
            //this.Controls.Add(f.tank);
            //this.Controls.Add(f2.tank);
            //tanks.Add(f2);
            //tanks.Add(f);
            //targets

        }

        private void Form1_KeyEvent(object sender, KeyEventArgs e)
        {


            int offset = 10;
            //if (e.KeyCode.ToString() == "A" || e.KeyCode.ToString() == "Left")
            //{
            //    dir2 = 3;
            //    t.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 3);

            //}
            //else if (e.KeyCode.ToString() == "W" || e.KeyCode.ToString() == "Up")
            //{
            //    dir2 = 0;
            //    t.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 0);

            //}
            //else if (e.KeyCode.ToString() == "S" || e.KeyCode.ToString() == "Down")
            //{
            //    dir2 = 2;
            //    t.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 2);
            //}
            //else if (e.KeyCode.ToString() == "D" || e.KeyCode.ToString() == "Right")
            //{
            //    dir2 = 1;
            //    t.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 1);
            //}
            if (e.KeyCode.ToString() == "Left")
            {
                dir1 = 3;
                f.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 3);

            }
            else if (e.KeyCode.ToString() == "Up")
            {
                dir1 = 0;
                f.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 0);

            }
            else if (e.KeyCode.ToString() == "Down")
            {
                dir1 = 2;
                f.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 2);
            }
            else if (e.KeyCode.ToString() == "Right")
            {
                dir1 = 1;
                f.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 1);
            }
            if (e.KeyCode.ToString() == "A" )
            {
                dir2 = 3;
                t.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 3);

            }
            else if (e.KeyCode.ToString() == "W" )
            {
                dir2 = 0;
                t.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 0);

            }
            else if (e.KeyCode.ToString() == "S" )
            {
                dir2 = 2;
                t.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 2);
            }
            else if (e.KeyCode.ToString() == "D" )
            {
                dir2 = 1;
                t.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 1);
            }

            else if (e.KeyCode.ToString() == "Space")
           
            {
                Bullet b = null;
                if (dir2 == 0 || dir2 == 2)
                {
                    b = new Bullet(dir2, new Point(t.tank.Location.X + 20, t.tank.Location.Y), 0);
                }
                if (dir2 == 1 || dir2 == 3)
                {
                    b = new Bullet(dir2, new Point(t.tank.Location.X, t.tank.Location.Y+20), 0);
                }
                bullets.Add(b);
                this.Controls.Add(b.image);

                //this.Controls.Add(picture);
            }

            else if (e.Shift)

            {
                Bullet b = null;
                if (dir1 == 0 || dir1 == 2)
                {
                    b = new Bullet(dir1, new Point(f.tank.Location.X + 20, f.tank.Location.Y), 1);
                }
                if (dir1 == 1 || dir1 == 3)
                {
                    b = new Bullet(dir1, new Point(f.tank.Location.X, f.tank.Location.Y + 20), 1);
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
                    foreach (Tank ta in tanks) { // loops through targets
                        if (p.Bounds.IntersectsWith(ta.tank.Bounds) && b.Player != ta.Player) // checks if target is in bounds
                        {
                            PictureBox targ = ta.tank; // assigns as hit tank
                            TargetDestroyed(targ);
                            RemoveBullet(b);
                        }
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

        void TargetDestroyed(PictureBox pb)
        {
            pb.Location = new Point(rnd.Next(0, this.ClientRectangle.Width), rnd.Next(0, this.ClientRectangle.Height));
        }
    }
}
