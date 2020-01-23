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
        public int Speed = 15;
        public int X_Coor { get; set; }
        public int Y_Coor { get; set; }
        public int Direction { set; get; }
        public int Player { set; get; }

        public Plane(Point location, int player)
        {
            plane = new PictureBox();
            plane.Size = new Size(80, 80);
            plane.BackColor = Color.Transparent;
            plane.Image = Properties.Resources.s_left;
            plane.Location = location;
            X_Coor = location.X;
            Y_Coor = location.Y;
            plane.SizeMode = PictureBoxSizeMode.Zoom;
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
                    Y_Coor -= Speed;
                    break;
                case 1: // RIGHT
                    X_Coor += Speed;
                    break;
                case 2: // DOWN
                    Y_Coor += Speed;
                    break;
                case 3: // LEFT
                    X_Coor -= Speed;
                    break;
            }
            if (X_Coor < 0)
                X_Coor = 0;
            if (Y_Coor < 0)
                Y_Coor = 0;
            if (X_Coor + plane.Width > clientW)
                X_Coor = clientW - plane.Width;
            if (Y_Coor + plane.Height > clientH)
                Y_Coor = clientH - plane.Height;
        }

        private void SetImage(int direction)
        {
            switch (direction)
            {
                case 0:
                    //plane.Image = Properties.Resources.s_up;
                    break;
                case 1:
                    plane.Image = Properties.Resources.s_right;
                    break;
                case 2:
                    //plane.Image = Properties.Resources.s_down;
                    break;
                case 3:
                    plane.Image = Properties.Resources.s_left;
                    break;
                default: // shouldn't reach
                    break;
            }
        }


    }
}
