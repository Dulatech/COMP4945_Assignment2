﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Timers;

namespace COMP4945_Assignment2
{
    public partial class GameArea : Form
        
    {
        public static int WIDTH;
        public static int HEIGHT;
        private Random rnd = new Random();
        private int dir = 0; // Represents the direction of the tank, starting at the top as 0 and increments in clockwise
        private List<Bullet> bullets;
        private List<Tank> tanks;
        private List<Plane> planes;
        private List<Bomb> bombs;
        Tank t;
        Plane p;
        //Tank target;
        MulticastSender msender;
        int prev_x = -1;
        int prev_y = -1;
        Thread receiverThread;
        private static System.Timers.Timer aTimer;
        private bool timer_Elapsed = false;

        public GameArea()
        {
            InitializeComponent();
            WIDTH = ClientSize.Width;
            HEIGHT = ClientSize.Height;
            System.Windows.Forms.Timer gameTime = new System.Windows.Forms.Timer();
            gameTime.Enabled = true;
            gameTime.Interval = 1;
            gameTime.Tick += new EventHandler(OnGameTimeTick);
            bullets = new List<Bullet>();
            tanks = new List<Tank>();
            planes = new List<Plane>();
            bombs = new List<Bomb>();
            t = new Tank(new Point(450, 450), Guid.NewGuid());
            p = new Plane(new Point(150, 150), Guid.NewGuid());
            this.Controls.Add(p.image);
            this.Controls.Add(t.image);
            tanks.Add(t);
            planes.Add(p);
            msender = new MulticastSender();
            MulticastReceiver recv = new MulticastReceiver(this);
            receiverThread = new Thread(new ThreadStart(recv.run));
            receiverThread.IsBackground = true; // thread becomes zombie if this is not explicitly set to true
            receiverThread.Start();
            SetTimer();
            

        }
        private void Form1_KeyEvent(object sender, KeyEventArgs e)
        {
            float FireRate = 2F;
            switch (e.KeyCode)
            {
                case Keys.A:
                    dir = 3;
                    t.move(dir);
                    break;
                case Keys.Left:
                    dir = 3;
                    p.move(dir);
                    break;
                case Keys.W:
                    dir = 0;
                    t.move(dir);
                    break;
                case Keys.Up:
                    dir = 0;
                    p.move(dir);
                    break;
                case Keys.S:
                    dir = 2;
                    t.move(dir);
                    break;
                case Keys.Down:
                    dir = 2;
                    p.move(dir);
                    break;
                case Keys.D:
                    dir = 1;
                    t.move(dir);
                    break;
                case Keys.Right:
                    dir = 1;
                    p.move(dir);
                    break;
                case Keys.Space:
                    if (timer_Elapsed)
                    {
                        timer_Elapsed = false;
                        Bullet b = new Bullet(new Point(t.X_Coor + 20, t.Y_Coor), 0);
                        int bulletSize = bullets.Count;
                        if (bulletSize > 2)
                        {
                            break;
                        }
                        bullets.Add(b);
                        
                    }
                    break;
                case Keys.ShiftKey:
                    if (timer_Elapsed)
                    {
                        timer_Elapsed = false;
                        Bomb b2 = new Bomb(new Point(p.X_Coor + 20, p.Y_Coor), 1);
                    int bombSize = bombs.Count;
                    if (bombSize > 2)
                    {
                        break;
                    }
                    bombs.Add(b2);
                    
                    }
                    break;

                default:
                    break;
            }
        }

        void OnGameTimeTick(object sender, EventArgs e)
        {
            p.image.Location = new Point(p.X_Coor, p.Y_Coor);
            t.image.Location = new Point(t.X_Coor, t.Y_Coor);
            if (prev_x != t.X_Coor || prev_y != t.Y_Coor)
                msender.SendMsg(t.X_Coor + "," + t.Y_Coor + "," + t.Direction);
            prev_x = t.X_Coor;
            prev_y = t.Y_Coor;

            if (bullets.Count != 0)
            {
                for (int i = bullets.Count - 1; i > -1; i--)
                {
                    Bullet b = bullets[i];
                    b.Move();
                    if (b.OutOfBounds())
                        bullets.Remove(b);
                    else
                        foreach (Plane ta in planes) // loops through targets
                            if (new Rectangle(b.X_Coor, b.Y_Coor, b.Width, b.Height).IntersectsWith(ta.image.Bounds) && b.Player != ta.Player) // checks if target is in bounds
                            {
                                Plane targ = ta; // assigns as hit plane
                                PlaneDestroyed(targ);
                                bullets.Remove(b);
                            }
                }
            }


            if (bombs.Count != 0)
            {
                for (int i = bombs.Count - 1; i > -1; i--)
                {
                    Bomb b = bombs[i];
                    b.Move();
                    if (b.OutOfBounds())
                        bombs.Remove(b);
                    else
                        foreach (Tank ta in tanks) // loops through targets
                            if (new Rectangle(b.X_Coor, b.Y_Coor, b.Width, b.Height).IntersectsWith(ta.image.Bounds) && b.Player != ta.Player) // checks if target is in bounds
                            {
                                Tank targ = ta; // assigns as hit tank
                                TankDestroyed(targ);
                                bombs.Remove(b);
                            }
                }
            }
            Invalidate(); // calls the Paint event
        }

        void TankDestroyed(Tank pb)
        {
            pb.X_Coor = rnd.Next(0, this.ClientRectangle.Width - t.image.Width);
            pb.Y_Coor = rnd.Next((int)(this.ClientRectangle.Height * 0.55), this.ClientRectangle.Height - t.image.Height);
        }

        void PlaneDestroyed(Plane pb)
        {
            pb.X_Coor = rnd.Next(0, this.ClientRectangle.Width - p.image.Width);
            pb.Y_Coor = rnd.Next(0, (int)(this.ClientRectangle.Height * 0.45) - p.image.Height);
        }

        public void draw(int x, int y, int dir)
        {
            p.X_Coor = x;
            p.Y_Coor = y - (int)(GameArea.HEIGHT * 0.6);
            p.SetDirection(dir);
        }

        private void GameArea_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            foreach (Bullet b in bullets)
                g.DrawImage(Bullet.IMAGE, b.X_Coor, b.Y_Coor, Bullet.SIZE.Width, Bullet.SIZE.Height);
            foreach (Bomb b in bombs)
                g.DrawImage(Bomb.IMAGE, b.X_Coor, b.Y_Coor, Bomb.SIZE.Width, Bomb.SIZE.Height);
        }
        private void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(500);
            aTimer.Elapsed += new ElapsedEventHandler(timer_ElapsedEvent);
            aTimer.AutoReset = true;
            aTimer.Enabled = true;

        }
        private void timer_ElapsedEvent(object source, ElapsedEventArgs e)
        {
            timer_Elapsed = true; 
        }

    }
}
