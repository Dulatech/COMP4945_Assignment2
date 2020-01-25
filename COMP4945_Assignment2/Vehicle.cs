using System;
using System.Drawing;
using System.Windows.Forms;

namespace COMP4945_Assignment2
{
    abstract class Vehicle
    {
        public PictureBox image;
        public int Speed = 10;
        public int X_Coor { get; set; }
        public int Y_Coor { get; set; }
        public int Direction { private set; get; }
        public int Player { set; get; }
        public Guid ID { set; get; }

        public Vehicle(Point location, Guid id)
        {
            image = new PictureBox();
            //image.BackColor = Color.Transparent;
            // just to hightlight current hitbox vs image size ratio <- we need to fix this
            image.BackColor = Color.Red;
            image.SizeMode = PictureBoxSizeMode.Zoom;
            X_Coor = location.X;
            Y_Coor = location.Y;
            Direction = 0;
            ID = id;
        }

        public void move(int direction)
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
        public void SetDirection(int direction)
        {
            if (direction != Direction)
            {
                Direction = direction;
                SetImage(direction);
            }
        }
        protected abstract void SetImage(int direction);
        protected abstract void CheckBounds();
    }
}
