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
        private int dir = 0; // Represents the direction of the tank, starting at the top as 0 and increments in clockwise
        private List<Bullet> bullets;
        private List<Tank> tanks;
        private List<Plane> planes;
        private List<Bomb> bombs;
        Tank t;
        Plane p;
        Tank target;

        public GameArea()
        {
            InitializeComponent();
            Timer gameTime = new Timer();
            gameTime.Enabled = true;
            gameTime.Interval = 1;
            gameTime.Tick += new EventHandler(OnGameTimeTick);
            bullets = new List<Bullet>();
            tanks = new List<Tank>();
            planes = new List<Plane>();
            bombs = new List<Bomb>();
            //target = new Tank(new Point(rnd.Next(0, this.ClientRectangle.Width), rnd.Next(0, this.ClientRectangle.Height)),1);
            t = new Tank(new Point(450, 450), 0);
            p = new Plane(new Point(150, 150), 1);
            this.Controls.Add(p.plane);
            this.Controls.Add(t.tank);
            //this.Controls.Add(target.tank);
            tanks.Add(t);
            planes.Add(p);
        }
        private void Form1_KeyEvent(object sender, KeyEventArgs e)
        {
            int offset = 10;
            switch (e.KeyCode)
            {
                case Keys.A:
                    p.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 3);
                    break;
                case Keys.Left:
                    dir = 3;
                    t.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 3);
                    break;
                case Keys.W:
                    p.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 0);
                    break;
                case Keys.Up:
                    dir = 0;
                    t.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 0);
                    break;
                case Keys.S:
                    p.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 2);
                    break;
                case Keys.Down:
                    dir = 2;
                    t.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 2);
                    break;
                case Keys.D:
                    p.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 1);
                    break;
                case Keys.Right:
                    dir = 1;
                    t.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 1);
                    break;
                case Keys.Space:
                    Bomb b2 = null;
                    if (dir == 0 || dir == 2)
                        b2 = new Bomb(dir, new Point(p.plane.Location.X + 20, p.plane.Location.Y), 1);
                    else
                        b2 = new Bomb(dir, new Point(p.plane.Location.X+20, p.plane.Location.Y), 1);
                    bombs.Add(b2);
                    this.Controls.Add(b2.image);
                    break;
                case Keys.ShiftKey:
                    Bullet b = null;
                    if (dir == 0 || dir == 2)
                        b = new Bullet(dir, new Point(t.tank.Location.X + 20, t.tank.Location.Y), 0);
                    else
                        b = new Bullet(dir, new Point(t.tank.Location.X + 20, t.tank.Location.Y), 0);
                    bullets.Add(b);
                    this.Controls.Add(b.image);
                    break;

                default:
                    break;
            }
        }

        void OnGameTimeTick(object sender, EventArgs e)
        {
            p.plane.Location = new Point(p.X_Coor, p.Y_Coor);
            t.tank.Location = new Point(t.X_Coor, t.Y_Coor);
            if (bullets.Count != 0)
            {
                for (int i = bullets.Count - 1; i > -1; i--)
                {
                    Bullet b = bullets[i];
                    b.Move();
                    PictureBox p = b.image;
                    if (p.Location.X < 0 || p.Location.Y < 0 || p.Location.X > this.ClientRectangle.Width || p.Location.Y > this.ClientRectangle.Height)
                    {
                        RemoveBullet(b);
                    }
                    else
                    {
                        foreach (Plane ta in planes)
                        { // loops through targets
                            if (p.Bounds.IntersectsWith(ta.plane.Bounds) && b.Player != ta.Player) // checks if target is in bounds
                            {
                                Plane targ = ta; // assigns as hit tank
                                PlaneDestroyed(targ);
                                RemoveBullet(b);
                            }
                        }
                    }
                }
            }


            if (bombs.Count != 0)
            {
                for (int i = bombs.Count - 1; i > -1; i--)
                {
                    Bomb b = bombs[i];
                    b.Move();
                    PictureBox p = b.image;
                    if (p.Location.X < 0 || p.Location.Y < 0 || p.Location.X > this.ClientRectangle.Width || p.Location.Y > this.ClientRectangle.Height)
                    {
                        RemoveBomb(b);
                    }
                    else
                    {
                        foreach (Tank ta in tanks)
                        { // loops through targets
                            if (p.Bounds.IntersectsWith(ta.tank.Bounds) && b.Player != ta.Player) // checks if target is in bounds
                            {
                                Tank targ = ta; // assigns as hit tank
                                TankDestroyed(targ);
                                RemoveBomb(b);
                            }
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

        void RemoveBomb(Bomb b)
        {
            this.Controls.Remove(b.image);
            bombs.Remove(b);
            b.image.Dispose();
        }

        void TankDestroyed(Tank pb)
        {
            pb.X_Coor = rnd.Next(0, this.ClientRectangle.Width);
            pb.Y_Coor = rnd.Next((int)(this.ClientRectangle.Height * 0.55) + t.tank.Height, this.ClientRectangle.Height);
        }

        void PlaneDestroyed(Plane pb)
        {
            pb.X_Coor = rnd.Next(0, this.ClientRectangle.Width);
            pb.Y_Coor = rnd.Next(0, (int)(this.ClientRectangle.Height * 0.45));
        }
    }
}
