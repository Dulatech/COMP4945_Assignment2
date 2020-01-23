using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COMP4945_Assignment2
{
    class Plane
    {
        public PictureBox plane;
        public int Speed = 10;
        private int X_Coor { get; set; }
        private int Y_Coor { get; set; }
        public int Direction { set; get; }

        public Plane(Point location, int player)
        {
            plane = new PictureBox();
            plane.Size = new Size(50, 50);
            plane.BackColor = Color.Transparent;
            plane.Image = Properties.Resources.s_up;
            plane.Location = location;
            plane.SizeMode = PictureBoxSizeMode.Zoom;
            Direction = 0;
        }

        public void move(int clientH, int clientW, int direction)
        {
            switch (direction)
            {
                case 0: // UP
                    if (plane.Top > 0)
                    {

                        plane.Image = Properties.Resources.s_up;
                        plane.Location = new Point(plane.Location.X, plane.Location.Y - Speed);
                    }
                    break;
                case 1: // RIGHT
                    if (plane.Left + plane.Width < clientW)
                    {
                        

                        plane.Image = Properties.Resources.s_right;
                        plane.Location = new Point(plane.Location.X + Speed, plane.Location.Y);
                    }
                    break;
                case 2: // DOWN
                    if (plane.Top + plane.Height < clientH)
                    {

                        plane.Image = Properties.Resources.s_down;
                        plane.Location = new Point(plane.Location.X, plane.Location.Y + Speed);
                    }
                    break;
                case 3: // LEFT
                    if (plane.Left > 0)
                    {


                        plane.Location = new Point(plane.Location.X - Speed, plane.Location.Y);

                        plane.Image = Properties.Resources.s_left;
                    }

                    break;
            }
        }


    }
}
