using System.Windows.Forms;
using System.Drawing;
using System;

namespace COMP4945_Assignment2
{
    class Projectile
    {
        public int X_Coor { set; get; }
        public int Y_Coor { set; get; }
        public int Width { set; get; }
        public int Height { set; get; }
        public int Speed;
        public int Direction { set; get; }
        public int Player { set; get; }
        public Guid ID { set; get; }
        public Projectile(Guid id, int direction, Point location, Size size, int player)
        {
            X_Coor = location.X;
            Y_Coor = location.Y;
            Width = size.Width;
            Height = size.Height;
            Direction = direction;
            Player = player;
            ID = id;
        }
        public void Move()
        {
            switch (Direction)
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
        }
        public bool OutOfBounds()
        {
            return (X_Coor < 0 || Y_Coor < 0 || X_Coor > GameArea.WIDTH || Y_Coor > GameArea.HEIGHT);
        }
    }
}
