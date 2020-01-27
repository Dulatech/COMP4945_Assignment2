using System.Windows.Forms;
using System.Drawing;

namespace COMP4945_Assignment2
{
    class Bullet : Projectile
    {
        public static readonly Image IMAGE = Properties.Resources.bullet_bob1;
        public static readonly Size SIZE = new Size(10, 18);
        public Bullet(Point location, int player) : base(0, location, SIZE, player)
        {
            Speed = 6;
        }
    }
}
