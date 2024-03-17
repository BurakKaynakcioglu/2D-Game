using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prolabbb
{
    public partial class Form1 : Form
    {
        static public readonly int squareLength = 10;
        static public int numberOfLines = 0;
        static public int numberOfChests = 0;

        MyArrayList<int> beeRotateList = new MyArrayList<int>();
        MyArrayList<string> beeWayList = new MyArrayList<string>();

        MyArrayList<int> birdRotateList = new MyArrayList<int>();
        MyArrayList<string> birdWayList = new MyArrayList<string>();


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form form = Application.OpenForms["OverlayedForm"];
            if (form != null)
            {
                form.Close();
            }

            Graphics graphics = gamePanel.CreateGraphics();
            gamePanel.Controls.Clear();
            graphics.Clear(Color.White);
            label1.Text = "";

            numberOfLines = int.Parse(textBox1.Text);

            setBackground(graphics);
            InitializeLines(graphics);
            putObjects();

            for (int i = 0; i < Bird.birdTag; i++)
            {
                birdRotateList.Add(squareLength * 5);
                birdWayList.Add("U");
            }
            for (int i = 0; i < Bee.beeTag; i++)
            {
                beeRotateList.Add(squareLength * 3);
                beeWayList.Add("L");
            }

            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OverlayedForm overlayedForm = new OverlayedForm(label1, gamePanel);
            overlayedForm.TopMost = true;
            overlayedForm.Show();
        }


        //arkaplandaki birim kareleri oluştur
        private void InitializeLines(Graphics g)
        {
            for (int i = 0; i <= numberOfLines; i++)
            {
                g.DrawLine(Pens.Gray, 0, squareLength * i, squareLength * numberOfLines, squareLength * i);
                g.DrawLine(Pens.Gray, squareLength * i, 0, i * squareLength, squareLength * numberOfLines);
            }
        }

        //arkaplan ayarla
        private void setBackground(Graphics g)
        {            
            Image image1 = Image.FromFile(Program.path + "winter_background.jpg");
            g.DrawImage(image1, 0, 0, numberOfLines * squareLength / 2, numberOfLines * squareLength);

            Image image2 = Image.FromFile(Program.path + "summer_background.jpg");
            g.DrawImage(image2, numberOfLines * squareLength / 2 , 0, numberOfLines * squareLength / 2, numberOfLines * squareLength);
        }

        //resim ekle
        private void putObjects()
        {    
            //Hangi nesneden kaç tane olacağını belirleyin (Birimkare boyutuna göre sığacak mantıklı sayılar ver)
            int tree = 7;
            int rock = 7;
            int mountain = 3;
            int wall = 5;
            int bird = 2;
            int bee = 2;
            int chest = 20; //4 ün katları girilmeli

            numberOfChests = chest;

            int[,] mapArray = new int[numberOfLines, numberOfLines];
            Program.mapArray = mapArray;

            int mountainCount = 0;
            while (mountain != 0) 
            {
                Random rnd = new Random();
                int randomX = rnd.Next(2, numberOfLines) * squareLength;
                int randomY = rnd.Next(2, numberOfLines) * squareLength;

                Mountain m = new Mountain(new Location(randomX, randomY), this.gamePanel);
                
                bool check = m.putMountain(ref mapArray);
                
                if (check) 
                {
                    mountainCount++;
                    if(mountainCount == mountain) { break; } 
                }
            }

            int treeCount = 0;
            while (tree != 0)
            {
                Random rnd = new Random();
                int randomX = rnd.Next(2, numberOfLines) * squareLength;
                int randomY = rnd.Next(2, numberOfLines) * squareLength;
                int randomTreeSize = rnd.Next(2, 6);

                Tree t = new Tree(new Location(randomX, randomY), this.gamePanel);

                bool check = t.putTree(randomTreeSize, ref mapArray);

                if (check)
                {
                    treeCount++;
                    if (treeCount == tree) { break; }
                }
            }

            int rockCount = 0;
            while (rock != 0)
            {
                Random rnd = new Random();
                int randomX = rnd.Next(2, numberOfLines) * squareLength;
                int randomY = rnd.Next(2, numberOfLines) * squareLength;
                int randomTreeSize = rnd.Next(2, 4);

                Rock r = new Rock(new Location(randomX, randomY), this.gamePanel);

                bool check = r.putRock(randomTreeSize, ref mapArray);

                if (check)
                {
                    rockCount++;
                    if (rockCount == rock) { break; }
                }
            }

            int wallCount = 0;
            while (wall != 0)
            {
                Random rnd = new Random();
                int randomX = rnd.Next(2, numberOfLines) * squareLength;
                int randomY = rnd.Next(2, numberOfLines) * squareLength;

                Wall w = new Wall(new Location(randomX, randomY), this.gamePanel);

                bool check = w.putWall(ref mapArray);

                if (check)
                {
                    wallCount++;
                    if (wallCount == wall) { break; }
                }
            }

            int chestCount = 0;
            while (chest != 0)
            {
                Random rnd = new Random();
                int randomX = rnd.Next(3, numberOfLines) * squareLength;
                int randomY = rnd.Next(3, numberOfLines) * squareLength;

                Chest c = new Chest(new Location(randomX, randomY), this.gamePanel);

                int gap = chest / 4;
                string type = "";
                if (chestCount >= 0 && chestCount < gap) { type = "gold"; }
                else if (chestCount >= gap && chestCount < gap * 2) { type = "silver"; }
                else if (chestCount >= gap * 2 && chestCount < gap * 3) { type = "emerald"; }
                else if (chestCount >= gap * 3 && chestCount < gap * 4) { type = "bronze"; }

                bool check = c.putChest(type,ref mapArray);

                if (check)
                {
                    chestCount++;
                    if (chestCount == chest) { break; }
                }
            }

            int birdCount = 0;
            while (bird != 0)
            {
                Random rnd = new Random();
                int randomX = rnd.Next(2, numberOfLines) * squareLength;
                int randomY = rnd.Next(2, numberOfLines) * squareLength;

                Bird b = new Bird(new Location(randomX, randomY), this.gamePanel);

                bool check = b.putBird(ref mapArray);

                if (check)
                {
                    birdCount++;
                    if (birdCount == bird) { break; }
                }
            }

            int beeCount = 0;
            while (bee != 0)
            {
                Random rnd = new Random();
                int randomX = rnd.Next(2, numberOfLines) * squareLength;
                int randomY = rnd.Next(2, numberOfLines) * squareLength;

                Bee b = new Bee(new Location(randomX, randomY), this.gamePanel);

                bool check = b.putBee(ref mapArray);

                if (check)
                {
                    beeCount++;
                    if (beeCount == bee) { break; }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (Control c in gamePanel.Controls)
            {
                if (c is PictureBox)
                {
                    PictureBox pb = (PictureBox)c;
                    string imagePath = pb.Tag as string;
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        if (imagePath.Contains("bee"))
                        {
                            string text = imagePath;
                            string numberPart = text.Replace("bee", "");
                            int index = int.Parse(numberPart);

                            if (beeWayList.get(index).Equals("L"))
                            {
                                pb.Location = new Point(pb.Location.X - 1, pb.Location.Y);
                                int newValue = beeRotateList.get(index) - 1;
                                beeRotateList.set(index, newValue);
                            }
                            else if (beeWayList.get(index).Equals("R"))
                            {
                                pb.Location = new Point(pb.Location.X + 1, pb.Location.Y);
                                int newValue = beeRotateList.get(index) + 1;
                                beeRotateList.set(index, newValue);
                            }

                            if (beeRotateList.get(index) == 0)
                            {
                                beeWayList.set(index, "R");
                                pb.Image = Image.FromFile(Program.path + "bee_R.gif");
                            }
                            else if (beeRotateList.get(index) == squareLength * 6) 
                            {
                                beeWayList.set(index, "L");
                                pb.Image = Image.FromFile(Program.path + "bee.gif");
                            }
                        }
                        else if (imagePath.Contains("bird"))
                        {
                            string text = imagePath;
                            string numberPart = text.Replace("bird", "");
                            int index = int.Parse(numberPart);

                            if (birdWayList.get(index).Equals("U"))
                            {
                                pb.Location = new Point(pb.Location.X, pb.Location.Y - 1);
                                int newValue = birdRotateList.get(index) - 1;
                                birdRotateList.set(index, newValue);
                            }
                            else if (birdWayList.get(index).Equals("D"))
                            {
                                pb.Location = new Point(pb.Location.X, pb.Location.Y + 1);
                                int newValue = birdRotateList.get(index) + 1;
                                birdRotateList.set(index, newValue);
                            }

                            if (birdRotateList.get(index) == 0)
                            {
                                birdWayList.set(index, "D");
                                pb.Image = Image.FromFile(Program.path + "flying-bird_R.gif");
                            }
                            else if (birdRotateList.get(index) == squareLength * 10)
                            {
                                birdWayList.set(index, "U");
                                pb.Image = Image.FromFile(Program.path + "flying-bird.gif");
                            }
                        }
                    }
                }
            }
        }
    }
}
