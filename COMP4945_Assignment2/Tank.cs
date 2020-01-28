using System;
using System.Drawing;

namespace COMP4945_Assignment2
{
    class Tank : Vehicle
    {
        public static readonly Size SIZE = new Size(50, 50);
        public static readonly Image IMG_UP = Properties.Resources.Tank_Up;
        public static readonly Image IMG_SIDE = Properties.Resources.Tank_Side;
        public Tank(Guid id, int x, int y) : base(id, SIZE, x, y)
        {
            Speed = 10;
            Player = 0;
        }
        //protected override void SetImage(int direction)
        //{
        //    switch (direction)
        //    {
        //        case 0:
        //            image.Image = Properties.Resources.Tank_Up;
        //            break;
        //        case 1:
        //            image.Image = Properties.Resources.Tank_Side;
        //            break;
        //        case 2:
        //            image.Image = Properties.Resources.Tank_Up;
        //            break;
        //        case 3:
        //            image.Image = Properties.Resources.Tank_Side;
        //            break;
        //        default: // shouldn't reach
        //            break;
        //    }
        //}

        protected override void CheckBounds()
        {
            if (X_Coor < 0)
                X_Coor = 0;
            if (Y_Coor > GameArea.HEIGHT - Height)
                Y_Coor = GameArea.HEIGHT - Height;
            if (X_Coor + Width > GameArea.WIDTH)
                X_Coor = GameArea.WIDTH - Width;
            if (Y_Coor < (GameArea.HEIGHT * 0.6))
                Y_Coor = (int)(GameArea.HEIGHT * 0.6);
        }

        public override void SetDirection(int direction)
        {
            if (direction != Direction)
            {
                Direction = direction;
            }
        }
    }
}
