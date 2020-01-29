using System.Windows.Forms;
using System.Drawing;
using System;

namespace COMP4945_Assignment2
{
    class Bomb : Projectile
    {
        public static readonly Image IMAGE = Properties.Resources.Bomb;
        public static readonly Size SIZE = new Size(20, 28);
        public Bomb(Guid id, Point location) : base(id, 2, location, SIZE)
        {
            Speed = 6;
        }
    }
}
