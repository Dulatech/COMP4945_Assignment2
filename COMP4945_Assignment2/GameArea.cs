using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COMP4945_Assignment2
{
    public partial class GameArea : Form
        
    {

        private Random rnd = new Random();

        public GameArea()
        {
            InitializeComponent();

        }

        private void Form1_KeyEvent(object sender, KeyEventArgs e)
        {


            int offset = 10;
            if (e.KeyCode.ToString() == "A" || e.KeyCode.ToString() == "Left")
            {
                if (ProtoTank.Left > 0)
                {
                    ProtoTank.Location = new Point(ProtoTank.Location.X - offset, ProtoTank.Location.Y);

                    ProtoTank.Image = Properties.Resources.Tank_Left;
                }

            }
            else if (e.KeyCode.ToString() == "W" || e.KeyCode.ToString() == "Up")
            {
                if (ProtoTank.Top > 0)
                {
                    ProtoTank.Image = Properties.Resources.Tank_Up;
                    ProtoTank.Location = new Point(ProtoTank.Location.X, ProtoTank.Location.Y - offset);
                }

            }
            else if (e.KeyCode.ToString() == "S" || e.KeyCode.ToString() == "Down")
            {
                if (ProtoTank.Top + ProtoTank.Height < this.ClientRectangle.Height)
                {
                    ProtoTank.Image = Properties.Resources.Tank_Down;
                    ProtoTank.Location = new Point(ProtoTank.Location.X, ProtoTank.Location.Y + offset);
                }
            }
            else if (e.KeyCode.ToString() == "D" || e.KeyCode.ToString() == "Right")
            {

                if (ProtoTank.Left + ProtoTank.Width < this.ClientRectangle.Width)
                {
                    ProtoTank.Image = Properties.Resources.Tank_Right;
                    ProtoTank.Location = new Point(ProtoTank.Location.X + offset, ProtoTank.Location.Y);
                }
            }
            else if (e.KeyCode.ToString() == "Space")
            {

                Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                
                this.BackColor = randomColor;

                //var picture = new PictureBox
                //{
                //    Name = "pictureBox",
                //    Size = new Size(5, 5),
                //    Location = new Point(ProtoTank.Location.X+22, ProtoTank.Location.Y),
                //    BackColor = Color.FromArgb(255, 50, 50)
                //};

                //this.Controls.Add(picture);
                



            }
        }
    }
}
