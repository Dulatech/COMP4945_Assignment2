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
        private int X_Coor { get; set; }
        private int Y_Coor { get; set; }
        public int Direction { set; get; }

        public Tank(Point location, int player)
        {
            tank = new PictureBox();
            tank.Size = new Size(50, 50);
            tank.BackColor = Color.Transparent;
            tank.Image = Properties.Resources.Tank_Up;
            tank.Location = location;
            tank.SizeMode = PictureBoxSizeMode.Zoom;
            Direction = 0;
        }

        public void move(int clientH, int clientW, int direction)
        {
            switch (direction)
            {
                case 0: // UP
                    if (tank.Top > 0)
                    {

                        tank.Image = Properties.Resources.Tank_Up;
                        tank.Location = new Point(tank.Location.X, tank.Location.Y - Speed);
                    }
                    break;
                case 1: // RIGHT
                    if (tank.Left + tank.Width < clientW)
                    {
                        

                        tank.Image = Properties.Resources.Tank_Right;
                        tank.Location = new Point(tank.Location.X + Speed, tank.Location.Y);
                    }
                    break;
                case 2: // DOWN
                    if (tank.Top + tank.Height < clientH)
                    {

                        tank.Image = Properties.Resources.Tank_Down;
                        tank.Location = new Point(tank.Location.X, tank.Location.Y + Speed);
                    }
                    break;
                case 3: // LEFT
                    if (tank.Left > 0)
                    {


                        tank.Location = new Point(tank.Location.X - Speed, tank.Location.Y);

                        tank.Image = Properties.Resources.Tank_Left;
                    }

                    break;
            }
        }


    }
}
