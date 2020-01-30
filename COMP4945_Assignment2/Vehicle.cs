using System;
using System.Drawing;
using System.Windows.Forms;

namespace COMP4945_Assignment2
{
    abstract class Vehicle
    {
        public bool IsDead { get; set; } //waiting for respawn
        public int Speed = 10;
        public int X_Coor { get; set; }
        public int Y_Coor { get; set; }
        public int Width { set; get; }
        public int Height { set; get; }
        public int Direction { set; get; }
        public int Player { set; get; }
        public Guid ID { set; get; }

        public Vehicle(Guid id, Size size, int x, int y)
        {
            //image.BackColor = Color.Transparent;
            // just to hightlight current hitbox vs image size ratio <- we need to fix this
            X_Coor = x;
            Y_Coor = y;
            Width = size.Width;
            Height = size.Height;
            Direction = 0;
            ID = id;
        }

        public void Move(int direction)
        {
            SetDirection(direction);

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
            CheckBounds();
        }
        public abstract void SetDirection(int direction);
        //protected abstract void SetImage(int direction);
        protected abstract void CheckBounds();
    }
}
