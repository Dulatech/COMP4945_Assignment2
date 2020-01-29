using System.Windows.Forms;
using System.Drawing;
using System;

namespace COMP4945_Assignment2
{
    class Bullet : Projectile
    {
        public static readonly Image IMAGE = Properties.Resources.bullet_bob1;
        public static readonly Size SIZE = new Size(10, 18);
        public Bullet(Guid id, Point location) : base(id, 0, location, SIZE)
        {
            Speed = 6;
        }
    }
}
