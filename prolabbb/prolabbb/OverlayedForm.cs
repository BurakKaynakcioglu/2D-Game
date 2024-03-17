using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace prolabbb
{
    public partial class OverlayedForm : Form
    {
        Character character;
        Graphics g;
        Label label;
        
        int numberOfChests = 0;

        static public string obstacleText = "";
        static public bool add = false;

        Panel panel;
        public OverlayedForm(Label label, Panel panel)
        {
            this.label = label;
            this.panel = panel;
            InitializeComponent();
        }

        private async void OverlayedForm_Load(object sender, EventArgs e)
        {
            await Task.Delay(500);

            g = panel1.CreateGraphics();
            for (int i = 0; i < Form1.numberOfLines; i++)
            {
                for (int j = 0; j < Form1.numberOfLines; j++)
                {
                    g.FillRectangle(Brushes.Gray, i * Form1.squareLength, j * Form1.squareLength, Form1.squareLength + 1, Form1.squareLength + 1);
                }
            }

            character = new Character(41, "erkek");
            g.FillRectangle(Brushes.Blue, character.locations[0].x * Form1.squareLength,
                character.locations[0].y * Form1.squareLength, Form1.squareLength, Form1.squareLength);

             timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Location loc = character.isChestOnSight(character.currentLocation.x, character.currentLocation.y);
            if (loc != null) {

                Console.WriteLine("girdik");
                Console.ReadLine();

                int[] start = { character.currentLocation.y, character.currentLocation.x };
                int[] end = { loc.y, loc.x };
                List<int[]> path = character.goForChest(start, end);

                timer1.Stop();

                numberOfChests++;
                drawPath(path);
            }
            else
            {
                g.FillRectangle(Brushes.Red, character.currentLocation.x * Form1.squareLength,
                    character.currentLocation.y * Form1.squareLength, Form1.squareLength, Form1.squareLength);
                character.setNextLocation();
                g.FillRectangle(Brushes.Blue, character.currentLocation.x * Form1.squareLength,
                    character.currentLocation.y * Form1.squareLength, Form1.squareLength, Form1.squareLength);
                character.clearFog(character.currentLocation.x, character.currentLocation.y, g);

                if (add) 
                {
                    string current = label.Text;
                    label.Text = current + obstacleText;
                    add = false;
                }
            }
        }

        private async void drawPath(List<int[]> path)
        {
            if (path.Count > 0)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    int[] point = path[i];

                    g.FillRectangle(Brushes.Red, character.currentLocation.x * Form1.squareLength,
                    character.currentLocation.y * Form1.squareLength, Form1.squareLength, Form1.squareLength);

                    character.currentLocation.x = point[1];
                    character.currentLocation.y = point[0];

                    if (character.passedWays[character.currentLocation.y, character.currentLocation.x] != 1)
                    {
                        character.locations.Add(character.currentLocation);
                    }

                    character.passedWays[character.currentLocation.y, character.currentLocation.x] = 1;
                    character.setVisibleWays(character.currentLocation.x, character.currentLocation.y);

                    if (add)
                    {
                        string current = label.Text;
                        label.Text = current + obstacleText;
                        add = false;
                    }

                    g.FillRectangle(Brushes.Blue, character.currentLocation.x * Form1.squareLength,
                     character.currentLocation.y * Form1.squareLength, Form1.squareLength, Form1.squareLength);
                    character.clearFog(character.currentLocation.x, character.currentLocation.y, g);

                    await Task.Delay(50);
                }

                if (Program.mapArray[character.currentLocation.y, character.currentLocation.x] <= 4 &&
                    Program.mapArray[character.currentLocation.y, character.currentLocation.x] >= 1)
                {
                    writeChestToLabel(character.currentLocation.x, character.currentLocation.y,
                        Program.mapArray[character.currentLocation.y, character.currentLocation.x]);

                    character.removeChest(character.currentLocation.x, character.currentLocation.y, g);

                    Console.WriteLine("bulduk");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Hedefe ulaşılamıyor.");
            }

            if (Form1.numberOfChests == numberOfChests)
            {
                finalLabelText();
                character.finalWay(g);
                MessageBox.Show("Bütün sandıklar " + character.stepCount  + " adımda bulundu!", "Oyun bitti!");
                this.Close();
            }
            else
            {
                timer1.Start();
            }
        }

        private void writeChestToLabel(int x, int y, int type)
        {
            String str = "";

            if (type == 1) 
            {
                str = "Altın sandık bulundu" + " (" + x + "-" + y + ")";
            }
            else if (type == 2) 
            {
                str = "Gümüş sandık bulundu" + " (" + x + "-" + y + ")";
            }
            else if (type == 3)
            {
                str = "Zümrüt sandık bulundu" + " (" + x + "-" + y + ")";
            }
            else if (type == 4)
            {
                str = "Bronz sandık bulundu" + " (" + x + "-" + y + ")";
            }

            string current = label.Text;
            label.Text = current + str + "\n";
        }

        int comparer(string line)
        {
            if (line.Contains("Altın")) return 1;
            if (line.Contains("Gümüş")) return 2;
            if (line.Contains("Zümrüt")) return 3;
            if (line.Contains("Bronz")) return 4;
            return 0;
        }

        private void finalLabelText()
        {
            string[] lines = label.Text.Split('\n');

            List<string> chestLines = new List<string>();
            foreach (string line in lines)
            {
                if (line.Contains("sandık"))
                {
                    chestLines.Add(line);
                }
            }

            Dictionary<string, int> sira = new Dictionary<string, int>
            {
                { "altın", 1 },
                { "gümüş", 2 },
                { "zümrüt", 3 },
                { "bronz", 4 }
            };

            chestLines = chestLines.OrderBy(comparer).ToList();

            label.Text = "";

            foreach (string line in chestLines)
            {
                label.Text += line + "\n";
            }

            foreach (string line in lines)
            {
                if (!line.Contains("sandık"))
                {
                    label.Text += line + "\n";
                }
            }
        }

        private void OverlayedForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Graphics graphics = panel.CreateGraphics();
            panel.Controls.Clear();
            graphics.Clear(Color.White);
            label.Text = "";
        }
    }
}
