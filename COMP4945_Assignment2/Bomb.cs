using System.Windows.Forms;
using System.Drawing;

namespace COMP4945_Assignment2
{
    class Bomb : Projectile
    {
        public static readonly Image IMAGE = Properties.Resources.Bomb;
        public Bomb(Point location, int player) : base(2, location, player)
        {
            Speed = 3;
            image.Size = new Size(20, 28);
            image.Image = IMAGE;
        }
    }
}
