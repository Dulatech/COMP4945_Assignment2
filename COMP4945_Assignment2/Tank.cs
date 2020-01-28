using System;
using System.Drawing;

namespace COMP4945_Assignment2
{
    class Tank : Vehicle
    {

        public Tank(Point location, Guid id) : base(location, id)
        {
            image.Size = new Size(50, 60);
            image.Image = Properties.Resources.Tank_Up;
            Player = 0;
        }
        protected override void SetImage(int direction)
        {
            switch (direction)
            {
                case 0:
                    image.Image = Properties.Resources.Tank_Up;
                    break;
                case 1:
                    image.Image = Properties.Resources.Tank_Side;
                    break;
                case 2:
                    image.Image = Properties.Resources.Tank_Up;
                    break;
                case 3:
                    image.Image = Properties.Resources.Tank_Side;
                    break;
                default: // shouldn't reach
                    break;
            }
        }

        protected override void CheckBounds()
        {
            if (X_Coor < 0)
                X_Coor = 0;
            if (Y_Coor > GameArea.HEIGHT - image.Height)
                Y_Coor = GameArea.HEIGHT - image.Height;
            if (X_Coor + image.Width > GameArea.WIDTH)
                X_Coor = GameArea.WIDTH - image.Width;
            if (Y_Coor < (GameArea.HEIGHT * 0.6))
                Y_Coor = (int)(GameArea.HEIGHT * 0.6);
        }
    }
}
