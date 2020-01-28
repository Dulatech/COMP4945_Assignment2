using System;
using System.Drawing;

namespace COMP4945_Assignment2
{
    class Plane : Vehicle
    {
        public static readonly Size SIZE = new Size(80, 80);
        public static readonly Image IMG_RIGHT = Properties.Resources.s_right;
        public static readonly Image IMG_LEFT = Properties.Resources.s_left;
        public Plane(Guid id, int x, int y) : base(id, SIZE, x, y)
        {
            Speed = 15;
            Player = 1;
            Direction = 1;
        }

        protected override void CheckBounds()
        {
            if (X_Coor < 0)
                X_Coor = 0;
            if (Y_Coor < 0)
                Y_Coor = 0;
            if (X_Coor + Width > GameArea.WIDTH)
                X_Coor = GameArea.WIDTH - Width;
            if (Y_Coor + Height > (GameArea.HEIGHT * 0.40))
            {
                Y_Coor = (int)(GameArea.HEIGHT * 0.40) - Height;
            }
        }

        public override void SetDirection(int direction)
        {
            if (direction != Direction && direction != 0 && direction != 2)
            {
                Direction = direction;
            }
        }

        //protected override void SetImage(int direction)
        //{
        //    switch (direction)
        //    {
        //        case 0:
        //            //plane.Image = Properties.Resources.s_up;
        //            break;
        //        case 1:
        //            image.Image = Properties.Resources.s_right;
        //            break;
        //        case 2:
        //            //plane.Image = Properties.Resources.s_down;
        //            break;
        //        case 3:
        //            image.Image = Properties.Resources.s_left;
        //            break;
        //        default: // shouldn't reach
        //            break;
        //    }
        //}
    }
}
