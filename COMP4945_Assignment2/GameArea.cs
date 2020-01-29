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
        public List<Guid> bullet_ids;
        public List<Guid> bomb_ids;
        private List<Bullet> bullets;
        private List<Tank> tanks;
        private List<Plane> planes;
        private Vehicle[] vehicles;
        private List<Bomb> bombs;
        public static int playerNum; // 0 & 2 is tank, 1 & 3 is plane
        public static int currentNumOfPlayers;
        public static int nextPlayer; // player number to invite
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
            bullet_ids = new List<Guid>();
            bomb_ids = new List<Guid>();
            vehicles = new Vehicle[MAX_PLAYERS];
            for (int i = 0; i < 4; i++)
                vehicles[i] = null;
            recv = new MulticastReceiver(this);

        }
        private void Form1_KeyEvent(object sender, KeyEventArgs e)
        {
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
                            int bulletSize = bullets.Count;
                            if (bulletSize > 2)
                                break;
                            Bullet b = new Bullet(Guid.NewGuid(), new Point(me.X_Coor + 20, me.Y_Coor));
                            bullets.Add(b);
                            bullet_ids.Add(b.ID);
                            MulticastSender.SendGameMsg(1, b.X_Coor + "," + b.Y_Coor + "," + b.Direction + "," + b.ID);
                        }
                    } else // plane
                    {
                        if (bTimer_Elapsed)
                        {
                            bTimer_Elapsed = false;
                            int bombSize = bombs.Count;
                            if (bombSize > 2)
                                break;
                            Bomb b2 = new Bomb(Guid.NewGuid(), new Point(me.X_Coor + 20, me.Y_Coor));
                            bombs.Add(b2);
                            bomb_ids.Add(b2.ID);
                            MulticastSender.SendGameMsg(3, b2.X_Coor + "," + b2.Y_Coor + "," + b2.Direction + "," + b2.ID);
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
                SendMovementMsg(me.X_Coor, me.Y_Coor, me.Direction);
            prev_x = me.X_Coor;
            prev_y = me.Y_Coor;

            //added test
            //for (int i = 0; i < bullets.Count; i++)
            //{
            //    //Bullet p1 = bullets[i];
            //    MulticastSender.SendGameMsg(1, bullets[i].X_Coor + "," + bullets[i].Y_Coor + "," + bullets[i].Direction + "," + bullets[i].ID);
            //}

            //for (int i = 0; i < bombs.Count; i++)
            //{
            //    //Bullet p1 = bullets[i];
            //    MulticastSender.SendGameMsg(3, bombs[i].X_Coor + "," + bombs[i].Y_Coor + "," + bombs[i].Direction + "," + bombs[i].ID);
            //}
            //added test

            if (bullets.Count != 0)
            {
                for (int i = bullets.Count - 1; i > -1; i--)
                {
                    Bullet b = bullets[i];
                    b.Move();
                    if (b.OutOfBounds())
                    {
                        bullets.Remove(b);
                        bullet_ids.Remove(b.ID);
                    }
                    else
                    {
                        foreach (Plane ta in planes) // loops through targets
                            if (new Rectangle(b.X_Coor, b.Y_Coor, b.Width, b.Height).IntersectsWith(new Rectangle(ta.X_Coor, ta.Y_Coor, ta.Width, ta.Height))) // checks if target is in bounds
                            {
                                Plane targ = ta; // assigns as hit plane
                                PlaneDestroyed(targ);
                                bullets.Remove(b);
                                bullet_ids.Remove(b.ID);
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
                    if (b.OutOfBounds())
                    {
                        bombs.Remove(b);
                        bomb_ids.Remove(b.ID);
                    }
                    else
                    {
                        foreach (Tank ta in tanks) // loops through targets
                            if (new Rectangle(b.X_Coor, b.Y_Coor, b.Width, b.Height).IntersectsWith(new Rectangle(ta.X_Coor, ta.Y_Coor, ta.Width, ta.Height))) // checks if target is in bounds
                            {
                                Tank targ = ta; // assigns as hit tank
                                TankDestroyed(targ);
                                bombs.Remove(b);
                                bomb_ids.Remove(b.ID);
                            }
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
                System.Diagnostics.Debug.WriteLine("NEW PLAYER!!!!!!!!\nNumber: " + playerNumber + " ID: " + id);
                if (recv.IsHost)
                    SetNextPlayer();
                PrintGameStateToDebug();
                SendMovementMsg(me.X_Coor, me.Y_Coor, me.Direction); // let new player know about me
                currentNumOfPlayers++;
            }
            Vehicle player = vehicles[playerNumber];
            player.X_Coor = x;
            player.Y_Coor = y;
            player.SetDirection(dir);
        }

        public void MoveBullet(Guid b_id, int x, int y)
        {
            if (!bullet_ids.Contains(b_id)) {
                Bullet b = new Bullet(b_id, new Point(x, y));
                bullets.Add(b);
                bullet_ids.Add(b.ID);
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                if (b_id == bullets[i].ID)
                {
                    bullets[i].X_Coor = x;
                    bullets[i].Y_Coor = y;
                }
            }
        }

        public void MoveBomb(Guid b_id, int x, int y)
        {
            if (!bomb_ids.Contains(b_id))
            {
                Bomb b = new Bomb(b_id, new Point(x, y));
                bombs.Add(b);
                bomb_ids.Add(b.ID);
            }
            for (int i = 0; i < bombs.Count; i++)
            {
                if (b_id == bombs[i].ID)
                {
                    bombs[i].X_Coor = x;
                    bombs[i].Y_Coor = y;
                }
            }
        }

        public void RemovePlayer(Guid id, int playerNum)
        {
            System.Diagnostics.Debug.WriteLine("inside RemovePlayer()");
            Vehicle player = vehicles[playerNum];
            if (player != null && player.ID == id)
            {
                if (playerNum % 2 == 0)
                {
                    if (tanks.Remove((Tank)player))
                    {
                        currentNumOfPlayers--;
                        System.Diagnostics.Debug.WriteLine("tank removed");
                    }
                }
                else
                {
                    if (planes.Remove((Plane)player))
                    {
                        currentNumOfPlayers--;
                        System.Diagnostics.Debug.WriteLine("plane removed");
                    }
                }
                vehicles[playerNum] = null;
                System.Diagnostics.Debug.WriteLine("PLAYER REMOVED!!!!!!!!!!\nNumber: " + playerNum + " ID: " + id);
                if (recv.IsHost)
                    SetNextPlayer();
                PrintGameStateToDebug();
            }
        }

        private void SetNextPlayer() {
            int i;
            for (i = 0; i < MAX_PLAYERS; i++)
            {
                if (vehicles[i] == null)
                    break;
            }
            nextPlayer = i;
        }

        private void GameArea_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int i = bullets.Count - 1; i > -1; i--)
            {
                g.DrawImage(Bullet.IMAGE, bullets[i].X_Coor, bullets[i].Y_Coor, Bullet.SIZE.Width, Bullet.SIZE.Height);
            }
            for (int i = bombs.Count - 1; i > -1; i--)
            {
                g.DrawImage(Bomb.IMAGE, bombs[i].X_Coor, bombs[i].Y_Coor, Bomb.SIZE.Width, Bomb.SIZE.Height);
            }
            for (int i = tanks.Count - 1; i > -1; i--)
            {
                Tank t = tanks[i];
                switch (t.Direction)
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
            }
            for (int i = planes.Count - 1; i > -1; i--)
            {
                Plane p = planes[i];
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
            {
                CreateNewGame();
                MulticastSender.SendGameMsg(-9, "game created");
                // the line above is for when there aren't any packets flowing in the port
                // if there are no packets, Thread t will be blocked on Socket.ReceiveFrom()
                // and calling Abort won't cancel the blocking call
            }
            recv.SetMulticastLoopback(false);
            System.Diagnostics.Debug.WriteLine("started game");
            if (playerNum % 2 == 0)
            {
                me = new Tank(MulticastSender.ID,
                    rnd.Next(0, this.ClientRectangle.Width - Tank.SIZE.Width),
                    rnd.Next((int)(this.ClientRectangle.Height * 0.55),this.ClientRectangle.Height - Tank.SIZE.Height));
                vehicles[playerNum] = me;
                tanks.Add((Tank) me);
            }
            else
            {
                me = new Plane(MulticastSender.ID,
                    rnd.Next(0, this.ClientRectangle.Width - Plane.SIZE.Width),
                    rnd.Next(0, (int)(this.ClientRectangle.Height * 0.45) - Plane.SIZE.Height));
                vehicles[playerNum] = me;
                planes.Add((Plane) me);
            }
            System.Diagnostics.Debug.WriteLine("my vehicle instatiated");
            if (playerNum == 0)
            {
                SetNextPlayer();
                hostThread = new Thread(new ThreadStart(MulticastSender.SendInvitations));
                hostThread.IsBackground = true;
                hostThread.Start();
                System.Diagnostics.Debug.WriteLine("hostThread started");
            }
            //tanks.Add(t);
            //planes.Add(p);
            recv.IsHost = (playerNum == 0);
            receiverThread = new Thread(new ThreadStart(recv.run));
            receiverThread.IsBackground = true; // thread becomes zombie if this is not explicitly set to true
            receiverThread.Start();
        }
        private void SendMovementMsg(int x, int y, int dir)
        {
            MulticastSender.SendGameMsg(0, x + "," + y + "," + dir);
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

        public void PrintGameStateToDebug()
        {
            System.Diagnostics.Debug.WriteLine("********* GAME STATUS: *********");
            System.Diagnostics.Debug.WriteLine("ID: " + gameID);
            System.Diagnostics.Debug.Write("Players: ");
            for (int i = 0; i < MAX_PLAYERS; i++)
                if (vehicles[i] != null)
                    System.Diagnostics.Debug.Write(i + ",");
            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.WriteLine("My Number: " + playerNum);
            if (recv.IsHost)
                System.Diagnostics.Debug.WriteLine("Next Player: " + nextPlayer);
        }

        private void GameArea_FormClosing(object sender, FormClosingEventArgs e)
        {
            MulticastSender.SendGameMsg(-1, "");
            MulticastSender.SendGameMsg(-1, "");
            MulticastSender.SendGameMsg(-1, "");
        }
    }
}