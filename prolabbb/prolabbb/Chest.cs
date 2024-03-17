using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prolabbb
{
    internal class Chest
    {
        public Location location { get; set; }
        public Panel panel { get; set; }

        public Chest(Location location, Panel panel)
        {
            this.location = location;
            this.panel = panel;
        }

        public bool putChest(string type, ref int[,] mapArray) {
            if (location.x + 5 * Form1.squareLength > Form1.squareLength * Form1.numberOfLines ||
                location.y + 5 * Form1.squareLength > Form1.squareLength * Form1.numberOfLines)
            {
                return false;
            }

            for (int i = location.x / Form1.squareLength - 3; i < location.x / Form1.squareLength + 5; i++)
            {
                for (int j = location.y / Form1.squareLength - 3; j < location.y / Form1.squareLength + 5; j++)
                {
                    if (mapArray[j, i] != 0)
                    {
                        return false;
                    }
                }
            }

            PictureBox pb = new PictureBox();
            pb.Location = new Point(location.x + 1, location.y + 1);
            pb.Size = new Size(2 * Form1.squareLength - 1, 2 * Form1.squareLength - 1);
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            int chooseType = 0;
            switch (type)
            {
                case "gold":
                    pb.Image = Image.FromFile(Program.path + "gold_chest.png");
                    chooseType = 1;
                    break;
                case "silver":
                    pb.Image = Image.FromFile(Program.path + "silver_chest.png");
                    chooseType = 2;
                    break;
                case "emerald":
                    pb.Image = Image.FromFile(Program.path + "emerald_chest.png");
                    chooseType = 3;
                    break;
                case "bronze":
                    pb.Image = Image.FromFile(Program.path + "bronze_chest.png");
                    chooseType = 4;
                    break;
            }

            for (int i = location.x / Form1.squareLength; i < location.x / Form1.squareLength + 2; i++)
            {
                for (int j = location.y / Form1.squareLength; j < location.y / Form1.squareLength + 2; j++)
                {
                    mapArray[j, i] = chooseType;
                }
            }

            panel.Controls.Add(pb);
            pb.BringToFront();
            return true;
        }
    }
}
