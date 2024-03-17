using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prolabbb
{
    internal class Rock : Obstacle
    {
        public Rock(Location location, Panel panel) : base(location, panel) { }

        public bool putRock(int size, ref int[,] mapArray)
        {
            if (location.x + (size + 2) * Form1.squareLength > Form1.squareLength * Form1.numberOfLines ||
                location.y + (size + 2) * Form1.squareLength > Form1.squareLength * Form1.numberOfLines)
            {
                return false;
            }

            for (int i = location.x / Form1.squareLength - 2; i < location.x / Form1.squareLength + (size + 2); i++)
            {
                for (int j = location.y / Form1.squareLength - 2; j < location.y / Form1.squareLength + (size + 2); j++)
                {
                    if (mapArray[j, i] != 0)
                    {
                        return false;
                    }
                }
            }

            PictureBox pb = new PictureBox();
            pb.Location = new Point(location.x + 1, location.y + 1);

            switch (size)
            {
                case 2:
                    pb.Size = new Size(2 * Form1.squareLength - 1, 2 * Form1.squareLength - 1);
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    if (location.x >= (Form1.squareLength * Form1.numberOfLines) / 2)
                    {
                        pb.Image = Image.FromFile(Program.path + "stone_summer.png");
                    }
                    else
                    {
                        pb.Image = Image.FromFile(Program.path + "stone_winter.png");
                    }
                    break;
                case 3:
                    pb.Size = new Size(3 * Form1.squareLength - 1, 3 * Form1.squareLength - 1);
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    if (location.x >= (Form1.squareLength * Form1.numberOfLines) / 2)
                    {
                        pb.Image = Image.FromFile(Program.path + "stones_summer.png");
                    }
                    else
                    {
                        pb.Image = Image.FromFile(Program.path + "stones_winter.png");
                    }
                    break;
            }

            panel.Controls.Add(pb);
            pb.BringToFront();

            for (int i = location.x / Form1.squareLength; i < location.x / Form1.squareLength + size; i++)
            {
                for (int j = location.y / Form1.squareLength; j < location.y / Form1.squareLength + size; j++)
                {
                    mapArray[j, i] = 9;
                }
            }

            return true;
        }
    }
}
