using System.Windows.Forms;
using System.Drawing;

namespace COMP4945_Assignment2
{
    class Bullet
    {
        public PictureBox image;
        public int Speed = 9;
        public int Direction { set; get; }
        public Bullet(int direction, Point location, int player)
        {
            image = new PictureBox();
            image.Size = direction % 2 == 0 ? new Size(3, 6) : new Size(6, 3);
            image.BackColor = Color.Black;
            image.Location = location;
            Direction = direction;
        }
        public void Move()
        {
            switch(Direction)
            {
                case 0: // UP
                    image.Location = new Point(image.Location.X, image.Location.Y + Speed);
                    break;
                case 1: // RIGHT
                    image.Location = new Point(image.Location.X + Speed, image.Location.Y);
                    break;
                case 2: // DOWN
                    image.Location = new Point(image.Location.X, image.Location.Y - Speed);
                    break;
                case 3: // LEFT
                    image.Location = new Point(image.Location.X - Speed, image.Location.Y);
                    break;
            }
        }
    }
}
