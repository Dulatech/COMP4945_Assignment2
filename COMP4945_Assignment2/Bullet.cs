using System.Windows.Forms;
using System.Drawing;

namespace COMP4945_Assignment2
{
    class Bullet : Projectile
    {
        public Bullet(Point location, int player) : base(0, location, player)
        {
            Speed = 6;
            image.Size = new Size(10, 18);
            image.Image = Properties.Resources.bullet_bob1;
        }
    }
}
