using System.Windows.Forms;
using System.Drawing;

namespace COMP4945_Assignment2
{
    class Bullet
    {
        public PictureBox image;
        public int Speed { set; get; }
        public int Direction { set; get; }
        public Bullet(int speed, int direction, Point location, int player)
        {
            image = new PictureBox();
            image.Size = new Size(4, 4);
            image.BackColor = Color.Black;
            image.Location = location;
            Speed = speed;
            Direction = direction;
        }
    }
}
