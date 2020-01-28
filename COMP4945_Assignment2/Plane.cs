using System;
using System.Drawing;

namespace COMP4945_Assignment2
{
    class Plane : Vehicle
    {
        public static readonly int SIZE = 80;
        public Plane(Guid id, int x, int y) : base(id, x, y)
        {
            Speed = 15;
            image.Size = new Size(SIZE, SIZE);
            image.Image = Properties.Resources.s_left;
            Player = 1;
        }

        protected override void CheckBounds()
        {
            if (X_Coor < 0)
                X_Coor = 0;
            if (Y_Coor < 0)
                Y_Coor = 0;
            if (X_Coor + image.Width > GameArea.WIDTH)
                X_Coor = GameArea.WIDTH - image.Width;
            if (Y_Coor + image.Height > (GameArea.HEIGHT * 0.40))
            {
                Y_Coor = (int)(GameArea.HEIGHT * 0.40) - image.Height;
            }
        }

        protected override void SetImage(int direction)
        {
            switch (direction)
            {
                case 0:
                    //plane.Image = Properties.Resources.s_up;
                    break;
                case 1:
                    image.Image = Properties.Resources.s_right;
                    break;
                case 2:
                    //plane.Image = Properties.Resources.s_down;
                    break;
                case 3:
                    image.Image = Properties.Resources.s_left;
                    break;
                default: // shouldn't reach
                    break;
            }
        }
    }
}
