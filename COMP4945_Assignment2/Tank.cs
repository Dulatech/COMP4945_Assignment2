using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COMP4945_Assignment2
{
    class Tank
    {
        public PictureBox tank;
        public int Speed = 10;
        public int X_Coor { get; set; }
        public int Y_Coor { get; set; }
        public int Direction { set; get; }
        public int Player { set; get; }

        public Tank(Point location, int player)
        {
            tank = new PictureBox();
            tank.Size = new Size(50, 50);
            tank.BackColor = Color.Transparent;
            tank.Image = Properties.Resources.Tank_Up;
            tank.Location = location;
            X_Coor = location.X;
            Y_Coor = location.Y;
            tank.SizeMode = PictureBoxSizeMode.Zoom;
            Direction = 0;
            Player = player;
        }

        public void move(int clientH, int clientW, int direction)
        {
            if (direction != Direction)
                SetImage(direction);

            Direction = direction;

            switch (direction)
            {
                case 0: // UP
                        //tank.Location = new Point(tank.Location.X, tank.Location.Y - Speed);
                        Y_Coor -= Speed;
                    break;
                case 1: // RIGHT
                        //tank.Location = new Point(tank.Location.X + Speed, tank.Location.Y);
                        X_Coor += Speed;
                    break;
                case 2: // DOWN
                        //tank.Location = new Point(tank.Location.X, tank.Location.Y + Speed);
                        Y_Coor += Speed;
                    break;
                case 3: // LEFT
                        //tank.Location = new Point(tank.Location.X - Speed, tank.Location.Y);
                        X_Coor -= Speed;
                    break;
            }
            if (X_Coor < 0)
                X_Coor = 0;
            if (Y_Coor < 0)
                Y_Coor = 0;
            if (X_Coor + tank.Width > clientW)
                X_Coor = clientW - tank.Width;
            if (Y_Coor + tank.Height > clientH)
                Y_Coor = clientH - tank.Height;

        }
        private void SetImage(int direction)
        {
            switch (direction)
            {
                case 0:
                    tank.Image = Properties.Resources.Tank_Up;
                    break;
                case 1:
                    tank.Image = Properties.Resources.Tank_Side;
                    break;
                case 2:
                    tank.Image = Properties.Resources.Tank_Up;
                    break;
                case 3:
                    tank.Image = Properties.Resources.Tank_Side;
                    break;
                default: // shouldn't reach
                    break;
            }
        }

    }
}
