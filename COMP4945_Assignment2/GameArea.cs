using System;
using System.Collections.Generic;
using System.Drawing;
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
        public List<Guid> players;
        private List<Bullet> bullets;
        private List<Tank> tanks;
        private List<Plane> planes;
        private Vehicle[] vehicles;
        private List<Bomb> bombs;
        public static int playerNum; // 0 & 2 is tank, 1 & 3 is plane
        public static int currentNumOfPlayers; // also represents next player's index number
        public static readonly int MAX_PLAYERS = 4;
        //Tank t;
        //Plane p;
        Vehicle me;
        public static Guid gameID = Guid.Empty;
        MulticastReceiver recv;
        int prev_x = -1;
        int prev_y = -1;
        Thread receiverThread;
        Thread hostThread;
        private static System.Timers.Timer aTimer;
        private bool aTimer_Elapsed = false;
        private bool bTimer_Elapsed = false;

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
            SetATimer();
            SetBTimer();
            players = new List<Guid>();
            vehicles = new Vehicle[MAX_PLAYERS];
            for (int i = 0; i < 4; i++)
                vehicles[i] = null;
            recv = new MulticastReceiver(this);

        }
        private void Form1_KeyEvent(object sender, KeyEventArgs e)
        {
            float FireRate = 2F;
            switch (e.KeyCode)
            {
                case Keys.A:
                case Keys.Left:
                    dir = 3;
                    me.Move(dir);
                    break;
                case Keys.W:
                case Keys.Up:
                    dir = 0;
                    me.Move(dir);
                    break;
                case Keys.S:
                case Keys.Down:
                    dir = 2;
                    me.Move(dir);
                    break;
                case Keys.D:
                case Keys.Right:
                    dir = 1;
                    me.Move(dir);
                    break;
                case Keys.Space:
                case Keys.ShiftKey:
                    if (playerNum % 2 == 0) // tank
                    {
                        if (aTimer_Elapsed)
                        {
                            aTimer_Elapsed = false;
                            Bullet b = new Bullet(new Point(me.X_Coor + 20, me.Y_Coor), 0);
                            int bulletSize = bullets.Count;
                            if (bulletSize > 2)
                            {
                                break;
                            }
                            bullets.Add(b);

                        }
                    } else // plane
                    {
                        if (bTimer_Elapsed)
                        {
                            bTimer_Elapsed = false;
                            Bomb b2 = new Bomb(new Point(me.X_Coor + 20, me.Y_Coor), 1);
                            int bombSize = bombs.Count;
                            if (bombSize > 2)
                            {
                                break;
                            }
                            bombs.Add(b2);

                        }
                    }
                    break;
                default:
                    break;
            }
        }

        void OnGameTimeTick(object sender, EventArgs e)
        {
            if (prev_x != me.X_Coor || prev_y != me.Y_Coor)
                MulticastSender.SendGameMsg(0, me.X_Coor + "," + me.Y_Coor + "," + me.Direction);
            prev_x = me.X_Coor;
            prev_y = me.Y_Coor;

            //added test
            for (int i = 0; i < bullets.Count; i++)
            {
                //Bullet p1 = bullets[i];
                MulticastSender.SendGameMsg(1, bullets[i].X_Coor + "," + bullets[i].Y_Coor + "," + bullets[i].Direction);
            }

            foreach (Projectile p1 in bombs)
            {
                MulticastSender.SendGameMsg(2, p1.X_Coor + "," + p1.Y_Coor + "," + p1.Direction);
            }
            //added test

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
                            if (new Rectangle(b.X_Coor, b.Y_Coor, b.Width, b.Height).IntersectsWith(new Rectangle(ta.X_Coor, ta.Y_Coor, ta.Width, ta.Height))) // checks if target is in bounds
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
                            if (new Rectangle(b.X_Coor, b.Y_Coor, b.Width, b.Height).IntersectsWith(new Rectangle(ta.X_Coor, ta.Y_Coor, ta.Width, ta.Height))) // checks if target is in bounds
                            {
                                Tank targ = ta; // assigns as hit tank
                                TankDestroyed(targ);
                                bombs.Remove(b);
                            }
                }
            }
            Invalidate(); // calls the Paint event
        }

        void TankDestroyed(Tank t)
        {
            t.X_Coor = rnd.Next(0, this.ClientRectangle.Width - t.Width);
            t.Y_Coor = rnd.Next((int)(this.ClientRectangle.Height * 0.55), this.ClientRectangle.Height - t.Height);
        }

        void PlaneDestroyed(Plane p)
        {
            p.X_Coor = rnd.Next(0, this.ClientRectangle.Width - p.Width);
            p.Y_Coor = rnd.Next(0, (int)(this.ClientRectangle.Height * 0.45) - p.Height);
        }

        public void MovePlayer(Guid id, int playerNumber, int x, int y, int dir)
        {
            if (vehicles[playerNumber] == null)
            {
                if (playerNumber % 2 == 0)
                {
                    Tank t = new Tank(id, x, y);
                    t.SetDirection(dir);
                    vehicles[playerNumber] = t;
                    if (!tanks.Contains(t))
                        tanks.Add(t);
                } else
                {
                    Plane p = new Plane(id, x, y);
                    p.SetDirection(dir);
                    vehicles[playerNumber] = p;
                    if (!planes.Contains(p))
                        planes.Add(p);
                }
                currentNumOfPlayers++;
            }
            Vehicle player = vehicles[playerNumber];
            player.X_Coor = x;
            player.Y_Coor = y;
            player.SetDirection(dir);
        }

        public void MoveBullet(int playerNumber, int x, int y, int dir)
        {
            Bullet b = new Bullet(new Point(x, y), playerNumber);
            bullets.Add(b);
            b.X_Coor = x;
            b.Y_Coor = y;
        }

        private void GameArea_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int i = bullets.Count - 1; i > -1; i--)
            {
                Bullet b = bullets[i];
                g.DrawImage(Bullet.IMAGE, b.X_Coor, b.Y_Coor, Bullet.SIZE.Width, Bullet.SIZE.Height);
            }
            foreach (Bomb b in bombs)
                g.DrawImage(Bomb.IMAGE, b.X_Coor, b.Y_Coor, Bomb.SIZE.Width, Bomb.SIZE.Height);
            foreach(Tank t in tanks)
                switch(t.Direction)
                {
                    case 0:
                    case 2:
                        g.DrawImage(Tank.IMG_UP, t.X_Coor, t.Y_Coor, Tank.SIZE.Width, Tank.SIZE.Height);
                        break;
                    case 1:
                    case 3:
                        g.DrawImage(Tank.IMG_SIDE, t.X_Coor, t.Y_Coor, Tank.SIZE.Width, Tank.SIZE.Height);
                        break;
                }
            foreach (Plane p in planes)
                switch (p.Direction)
                {
                    case 1:
                        g.DrawImage(Plane.IMG_RIGHT, p.X_Coor, p.Y_Coor, Plane.SIZE.Width, Plane.SIZE.Height);
                        break;
                    case 3:
                        g.DrawImage(Plane.IMG_LEFT, p.X_Coor, p.Y_Coor, Plane.SIZE.Width, Plane.SIZE.Height);
                        break;
                }
        }
        public void CreateNewGame()
        {
            gameID = Guid.NewGuid();
            playerNum = 0;
            currentNumOfPlayers = 1;
            System.Diagnostics.Debug.WriteLine("created new game");
        }
        private void GameArea_Shown(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(recv.EnterGame));
            t.IsBackground = true;
            t.Start();
            Thread.Sleep(1000);
            t.Abort();
            if (gameID == Guid.Empty)
                CreateNewGame();
            System.Diagnostics.Debug.WriteLine("entered game");
            //me = (playerNum % 2 == 0) ? new Tank(new Point(0, 0), MulticastSender.ID) : new Plane(new Point(0, 0), MulticastSender.ID);
            if (playerNum == 0)
            {
                hostThread = new Thread(new ThreadStart(MulticastSender.SendInvitations));
                hostThread.IsBackground = true;
                hostThread.Start();
                System.Diagnostics.Debug.WriteLine("hostThread started");
            }
            if (playerNum % 2 == 0)
            {
                me = new Tank(MulticastSender.ID,
                    rnd.Next(0, this.ClientRectangle.Width - Tank.SIZE.Width),
                    rnd.Next((int)(this.ClientRectangle.Height * 0.55),this.ClientRectangle.Height - Tank.SIZE.Height));
                tanks.Add((Tank) me);
                System.Diagnostics.Debug.WriteLine("tank added");
            }
            else
            {
                me = new Plane(MulticastSender.ID,
                    rnd.Next(0, this.ClientRectangle.Width - Plane.SIZE.Width),
                    rnd.Next(0, (int)(this.ClientRectangle.Height * 0.45) - Plane.SIZE.Height));
                planes.Add((Plane) me);
            }
            System.Diagnostics.Debug.WriteLine("my vehicle instatiated");
            //tanks.Add(t);
            //planes.Add(p);
            recv.IsHost = (playerNum == 0);
            receiverThread = new Thread(new ThreadStart(recv.run));
            receiverThread.IsBackground = true; // thread becomes zombie if this is not explicitly set to true
            receiverThread.Start();
        }
        private void SetATimer()
        {
            // Create a timer with a half second interval.
            aTimer = new System.Timers.Timer(500);
            aTimer.Elapsed += new ElapsedEventHandler(aTimer_ElapsedEvent);
            aTimer.AutoReset = true;
            aTimer.Enabled = true;

        }
        private void aTimer_ElapsedEvent(object source, ElapsedEventArgs e)
        {
            aTimer_Elapsed = true; 
        }

        private void SetBTimer()
        {
            // Create a timer with a half second interval.
            aTimer = new System.Timers.Timer(500);
            aTimer.Elapsed += new ElapsedEventHandler(bTimer_ElapsedEvent);
            aTimer.AutoReset = true;
            aTimer.Enabled = true;

        }
        private void bTimer_ElapsedEvent(object source, ElapsedEventArgs e)
        {
            bTimer_Elapsed = true;
        }

    }
}