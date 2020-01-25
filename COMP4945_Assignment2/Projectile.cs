using System.Windows.Forms;
using System.Drawing;

namespace COMP4945_Assignment2
{
    class Projectile
    {
        public PictureBox image;
        public int X_Coor { set; get; }
        public int Y_Coor { set; get; }
        public int Speed = 6;
        public int Direction { set; get; }
        public int Player { set; get; }
        public Projectile(int direction, Point location, int player)
        {
            image = new PictureBox();
            image.BackColor = Color.White;
            image.SizeMode = PictureBoxSizeMode.Zoom;
            image.Location = location;
            X_Coor = location.X;
            Y_Coor = location.Y;
            Direction = direction;
            Player = player;
        }
        public void Move()
        {
            switch (Direction)
            {
                case 0: // UP
                    Y_Coor -= Speed;
                    //image.Location = new Point(image.Location.X, image.Location.Y - Speed);
                    break;
                case 1: // RIGHT
                    X_Coor += Speed;
                    //image.Location = new Point(image.Location.X + Speed, image.Location.Y);
                    break;
                case 2: // DOWN
                    Y_Coor += Speed;
                    //image.Location = new Point(image.Location.X, image.Location.Y + Speed);
                    break;
                case 3: // LEFT
                    X_Coor -= Speed;
                    //image.Location = new Point(image.Location.X - Speed, image.Location.Y);
                    break;
            }
        }
    }
}
