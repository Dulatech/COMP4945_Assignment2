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
                    ProtoTank.Location = new Point(ProtoTank.Location.X - offset, ProtoTank.Location.Y);

                    ProtoTank.Image = Properties.Resources.Tank_Left;
                }

            }
            else if (e.KeyCode.ToString() == "W" || e.KeyCode.ToString() == "Up")
            {
                if (ProtoTank.Top > 0)
                {
                    ProtoTank.Image = Properties.Resources.Tank_Up;
                    ProtoTank.Location = new Point(ProtoTank.Location.X, ProtoTank.Location.Y - offset);
                }

            }
            else if (e.KeyCode.ToString() == "S" || e.KeyCode.ToString() == "Down")
            {
                if (ProtoTank.Top + ProtoTank.Height < this.ClientRectangle.Height)
                {
                    ProtoTank.Image = Properties.Resources.Tank_Down;
                    ProtoTank.Location = new Point(ProtoTank.Location.X, ProtoTank.Location.Y + offset);
                }
            }
            else if (e.KeyCode.ToString() == "D" || e.KeyCode.ToString() == "Right")
            {

                if (ProtoTank.Left + ProtoTank.Width < this.ClientRectangle.Width)
                {
                    ProtoTank.Image = Properties.Resources.Tank_Right;
                    ProtoTank.Location = new Point(ProtoTank.Location.X + offset, ProtoTank.Location.Y);
                }
            }
            else if (e.KeyCode.ToString() == "Space")
            {

                //Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                
                //this.BackColor = randomColor;

                //var picture = new PictureBox
                //{
                //    Name = "pictureBox",
                //    Size = new Size(5, 5),
                //    Location = new Point(ProtoTank.Location.X+22, ProtoTank.Location.Y),
                //    BackColor = Color.FromArgb(255, 50, 50)
                //};
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
                p.Location = new Point(p.Location.X + b.Speed, p.Location.Y);
            }
            //controller.CollisionGameArea(ball);
            //controller.PaddleCollision(player, player2, ball);
        }
    }
}
