﻿using System;
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
        Tank t;

        public GameArea()
        {
            InitializeComponent();
            Timer gameTime = new Timer();
            gameTime.Enabled = true;
            gameTime.Interval = 1;
            gameTime.Tick += new EventHandler(OnGameTimeTick);
            bullets = new List<Bullet>();
            tanks = new List<Tank>();
            Target.Location = new Point(rnd.Next(0, this.ClientRectangle.Width), rnd.Next(0, this.ClientRectangle.Height));
            t = new Tank(new Point(250, 250), 0);
            this.Controls.Add(t.tank);
        }
        private void Form1_KeyEvent(object sender, KeyEventArgs e)
        {
            int offset = 10;
            switch (e.KeyCode)
            {
                case Keys.A:
                case Keys.Left:
                    dir = 3;
                    t.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 3);
                    break;
                case Keys.W:
                case Keys.Up:
                    dir = 0;
                    t.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 0);
                    break;
                case Keys.S:
                case Keys.Down:
                    dir = 2;
                    t.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 2);
                    break;
                case Keys.D:
                case Keys.Right:
                    dir = 1;
                    t.move(this.ClientRectangle.Height, this.ClientRectangle.Width, 1);
                    break;
                case Keys.Space:
                    Bullet b = null;
                    if (dir == 0 || dir == 2)
                        b = new Bullet(dir, new Point(t.tank.Location.X + 20, t.tank.Location.Y), 0);
                    else
                        b = new Bullet(dir, new Point(t.tank.Location.X, t.tank.Location.Y + 20), 0);
                    bullets.Add(b);
                    this.Controls.Add(b.image);
                    break;
                default:
                    break;
            }
        }

        void OnGameTimeTick(object sender, EventArgs e)
        {
            t.tank.Location = new Point(t.X_Coor, t.Y_Coor);
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
