using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prolabbb
{
    internal class Bee : Obstacle
    {
        static public int beeTag = 0;

        public Bee(Location location, Panel panel) : base(location, panel) { }

        public bool putBee(ref int[,] mapArray)
        {
            if (location.x + 10 * Form1.squareLength > Form1.squareLength * Form1.numberOfLines ||
               location.y + 4 * Form1.squareLength > Form1.squareLength * Form1.numberOfLines)
            {
                return false;
            }

            for (int i = location.x / Form1.squareLength - 2; i < location.x / Form1.squareLength + 10; i++)
            {
                for (int j = location.y / Form1.squareLength - 2; j < location.y / Form1.squareLength + 4; j++)
                {
                    if (mapArray[j, i] != 0)
                    {
                        return false;
                    }
                }
            }

            PictureBox pb = new PictureBox();
            pb.Location = new Point(location.x + 1, location.y + 1);
            pb.Size = new Size(8 * Form1.squareLength - 1, 2 * Form1.squareLength - 1);
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            pb.BackColor = Color.IndianRed;
            pb.Image = Image.FromFile(Program.path + "Empty.png");

            PictureBox pb1 = new PictureBox();
            pb1.Location = new Point(location.x + 1 + (3 * Form1.squareLength), location.y + 1);
            pb1.Size = new Size(2 * Form1.squareLength - 1, 2 * Form1.squareLength - 1);
            pb1.SizeMode = PictureBoxSizeMode.StretchImage;
            pb1.BackColor = Color.IndianRed;
            pb1.Image = Image.FromFile(Program.path + "bee.gif");
            pb1.Tag = "bee" + beeTag++;

            for (int i = location.x / Form1.squareLength; i < location.x / Form1.squareLength + 8; i++)
            {
                for (int j = location.y / Form1.squareLength; j < location.y / Form1.squareLength + 2; j++)
                {
                    mapArray[j, i] = 6;
                }
            }

            panel.Controls.Add(pb);
            panel.Controls.Add(pb1);
            pb.BringToFront();
            pb1.BringToFront();
            return true;
        }
    }
}
