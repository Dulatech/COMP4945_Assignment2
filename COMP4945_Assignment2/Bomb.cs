using System.Windows.Forms;
using System.Drawing;

namespace COMP4945_Assignment2
{
    class Bomb : Projectile
    {
        public static readonly Image IMAGE = Properties.Resources.Bomb;
        public static readonly Size SIZE = new Size(20, 28);
        public Bomb(Point location, int player) : base(2, location, SIZE, player)
        {
            Speed = 3;
        }
    }
}
