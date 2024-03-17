using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prolabbb
{
    internal class Bird : Obstacle
    {
        static public int birdTag = 0;

        public Bird(Location location, Panel panel) : base(location, panel) { }

        public bool putBird(ref int[,] mapArray)
        {
            if (location.x + 4 * Form1.squareLength > Form1.squareLength * Form1.numberOfLines ||
               location.y + 14 * Form1.squareLength > Form1.squareLength * Form1.numberOfLines)
            {
                return false;
            }

            for (int i = location.x / Form1.squareLength - 2; i < location.x / Form1.squareLength + 4; i++)
            {
                for (int j = location.y / Form1.squareLength - 2; j < location.y / Form1.squareLength + 14; j++)
                {
                    if (mapArray[j, i] != 0)
                    {
                        return false;
                    }
                }
            }

            PictureBox pb = new PictureBox();
            pb.Location = new Point(location.x + 1, location.y + 1);
            pb.Size = new Size(2 * Form1.squareLength - 1, 12 * Form1.squareLength - 1);
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            pb.BackColor = Color.IndianRed;
            pb.Image = Image.FromFile(Program.path + "Empty.png");

            PictureBox pb1 = new PictureBox();
            pb1.Location = new Point(location.x + 1, location.y + 1 + (5 * Form1.squareLength));
            pb1.Size = new Size(2 * Form1.squareLength - 1, 2 * Form1.squareLength - 1);
            pb1.SizeMode = PictureBoxSizeMode.StretchImage;
            pb1.BackColor = Color.IndianRed;
            pb1.Image = Image.FromFile(Program.path + "flying-bird.gif");
            pb1.Tag = "bird" + birdTag++;

            for (int i = location.x / Form1.squareLength; i < location.x / Form1.squareLength + 2; i++)
            {
                for (int j = location.y / Form1.squareLength; j < location.y / Form1.squareLength + 12; j++)
                {
                    mapArray[j, i] = 5;
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
