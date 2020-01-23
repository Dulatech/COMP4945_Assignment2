using System.Windows.Forms;
using System.Drawing;

namespace COMP4945_Assignment2
{
    class Bomb
    {
        public PictureBox image;
        public int Speed = 4;
        public int Direction { set; get; }
        public Bomb(int direction, Point location, int player)
        {
            image = new PictureBox();
            image.Size = direction % 2 == 0 ? new Size(10, 18) : new Size(18, 10);
            image.Location = location;
            image.Image = Properties.Resources.Bomb;
            Direction = direction;
        }
        public void Move()
        {
            image.Location = new Point(image.Location.X, image.Location.Y + Speed);
/*            switch (Direction)
            {
                case 0: // UP
                    image.Location = new Point(image.Location.X, image.Location.Y - Speed);
                    break;
                case 1: // RIGHT
                    image.Location = new Point(image.Location.X + Speed, image.Location.Y);
                    break;
                case 2: // DOWN
                    image.Location = new Point(image.Location.X, image.Location.Y + Speed);
                    break;
                case 3: // LEFT
                    image.Location = new Point(image.Location.X - Speed, image.Location.Y);
                    break;
            }*/
        }
    }
}
