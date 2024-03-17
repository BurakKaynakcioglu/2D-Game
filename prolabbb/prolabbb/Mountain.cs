using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prolabbb
{
    internal class Mountain : Obstacle
    {
        public Mountain(Location location, Panel panel) : base(location, panel) { }

        public bool putMountain(ref int[,] mapArray)
        {
            if (location.x + 17 * Form1.squareLength > Form1.squareLength * Form1.numberOfLines ||
               location.y + 17 * Form1.squareLength > Form1.squareLength * Form1.numberOfLines)
            {
                return false;
            }

            for (int i = location.x / Form1.squareLength - 2; i < location.x / Form1.squareLength + 17; i++)
            {
                for (int j = location.y / Form1.squareLength - 2; j < location.y / Form1.squareLength + 17; j++)
                {
                    if (mapArray[j, i] != 0)
                    {
                        return false;
                    }
                }
            }

            PictureBox pb = new PictureBox();
            pb.Location = new Point(location.x + 1, location.y + 1);
            pb.Size = new Size(15 * Form1.squareLength - 1, 15 * Form1.squareLength - 1);
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            if (location.x >= (Form1.squareLength * Form1.numberOfLines) / 2)
            {
                pb.Image = Image.FromFile(Program.path + "mountain_summer.png");
            }
            else
            {
                pb.Image = Image.FromFile(Program.path + "mountain_winter.png");
            }

            for (int i = location.x / Form1.squareLength; i < location.x / Form1.squareLength + 15; i++)
            {
                for (int j = location.y / Form1.squareLength; j < location.y / Form1.squareLength + 15; j++)
                {
                    mapArray[j, i] = 7;
                }
            }

            panel.Controls.Add(pb);
            pb.BringToFront();

            return true;
        }
    }
}
