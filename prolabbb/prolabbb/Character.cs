using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace prolabbb
{
    internal class Character
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Location> locations { get; set; }
        public int[,] visibleWays { get; set; }
        public int[,] passedWays { get; set; }
        public int[,] paintedChests { get; set; }
        public int[,] seenObstacles { get; set; }


        public Location currentLocation { get; set; }
        public List<Location> lastThreelocations { get; set; }
        public List<int> lastThreeCount { get; set; }
        public List<Location> chestLocations { get; set; }

        public int stepCount = 0;


        public Character(int ID, string Name)
        {
            this.ID = ID;
            this.Name = Name;
            this.locations = new List<Location>();
            this.visibleWays = new int[Form1.numberOfLines, Form1.numberOfLines];
            this.passedWays = new int[Form1.numberOfLines, Form1.numberOfLines];
            this.paintedChests = new int[Form1.numberOfLines, Form1.numberOfLines];
            this.seenObstacles = new int[Form1.numberOfLines, Form1.numberOfLines];
            this.lastThreelocations = new List<Location>();
            this.lastThreeCount = new List<int>();
            this.chestLocations = new List<Location>();


            locations.Add(setFirstLocation());
            this.currentLocation = new Location(locations[0].x, locations[0].y);
        }

        private Location setFirstLocation()
        {
            Random random = new Random();
            Location location;

            while (true)
            {
                int randomX = random.Next(0, Form1.numberOfLines);
                int randomY = random.Next(0, Form1.numberOfLines);

                if (Program.mapArray[randomY, randomX] == 0)
                {
                    location = new Location(randomX, randomY);
                    break;
                }
            }

            setVisibleWays(location.x, location.y);
            passedWays[location.y, location.x] = 1;
            return location;
        }
        public void setNextLocation()
        {
            int currentX = currentLocation.x;
            int currentY = currentLocation.y;
            Location nextLocation = new Location(0, 0);

            List<int> limits = findLimit(currentX, currentY);  // 1 yukarı, 2 sag, 3 asagi, 4 sola 

            if (!limits.Contains(1))
            {
                if (checkObstacle(currentX, currentY - 1)) { limits.Add(1); }
            }
            if (!limits.Contains(2))
            {
                if (checkObstacle(currentX + 1, currentY)) { limits.Add(2); }
            }
            if (!limits.Contains(3))
            {
                if (checkObstacle(currentX, currentY + 1)) { limits.Add(3); }
            }
            if (!limits.Contains(4))
            {
                if (checkObstacle(currentX - 1, currentY)) { limits.Add(4); }
            }

            if(true)
            {
                bool randomWay = false;

                Location temp = new Location(currentX, currentY);

                if (lastThreelocations.Count < 3)
                {
                    lastThreelocations.Add(temp);
                    lastThreeCount.Add(1);
                }
                else
                {
                    bool check = false;
                    int index = 0;
                    foreach (Location loc in lastThreelocations)
                    {
                        if (loc.x == temp.x && loc.y == temp.y)
                        {
                            check = true;
                            index = lastThreelocations.IndexOf(loc);
                            break;
                        }
                    }

                    if (check)
                    {
                        lastThreeCount[index]++;
                    }
                    else
                    {
                        lastThreelocations.RemoveAt(0);
                        lastThreeCount.RemoveAt(0);

                        lastThreelocations.Add(temp);
                        lastThreeCount.Add(1);
                    }

                    if (lastThreeCount[0] > 2 || lastThreeCount[1] > 2 || lastThreeCount[2] > 2)
                    {
                        randomWay = true;
                    }
                }

                if (randomWay)
                {
                    nextLocation = chooseRandomNextLocation(currentX, currentY, limits);
                }
                else
                {
                    switch (findEmptyestDirection(currentX, currentY, limits))
                    {
                        case 1:
                            nextLocation.x = currentX;
                            nextLocation.y = currentY - 1;
                            break;
                        case 2:
                            nextLocation.x = currentX + 1;
                            nextLocation.y = currentY;
                            break;
                        case 3:
                            nextLocation.x = currentX;
                            nextLocation.y = currentY + 1;
                            break;
                        case 4:
                            nextLocation.x = currentX - 1;
                            nextLocation.y = currentY;
                            break;
                    }
                }
            }

            if (passedWays[nextLocation.y, nextLocation.x] != 1) { locations.Add(nextLocation); }
            currentLocation = new Location(nextLocation.x, nextLocation.y);
            passedWays[nextLocation.y, nextLocation.x] = 1;
            setVisibleWays(nextLocation.x, nextLocation.y);
        }

        private int findEmptyestDirection(int currentX, int currentY, List<int> limits)
        {
            int L = 3, R = 3, U = 3, D = 3;
            Dictionary<int, int> area = new Dictionary<int, int>();   // 1 yukarı, 2 sag, 3 asagi, 4 sola 

            if (currentX - 3 < 0) { L = currentX; }
            if (currentX + 3 > Form1.numberOfLines - 1) { R = Form1.numberOfLines - 1 - currentX; }
            if (currentY - 3 < 0) { U = currentY; }
            if (currentY + 3 > Form1.numberOfLines - 1) { D = Form1.numberOfLines - 1 - currentY; }

            if (!limits.Contains(1))
            {
                area.Add(1, 0);
                for (int i = currentY - U; i < currentY; i++)
                {
                    for (int j = currentX - L; j <= currentX + R; j++)
                    {
                        if (passedWays[i, j] == 1) { area[1]++; }
                    }
                }
            }
            if (!limits.Contains(2))
            {
                area.Add(2, 0);
                for (int i = currentY - U; i <= currentY + D; i++)
                {
                    for (int j = currentX + 1; j <= currentX + R; j++)
                    {
                        if (passedWays[i, j] == 1) { area[2]++; }
                    }
                }
            }
            if (!limits.Contains(3))
            {
                area.Add(3, 0);
                for (int i = currentY + 1; i <= currentY + D; i++)
                {
                    for (int j = currentX - L; j <= currentX + R; j++)
                    {
                        if (passedWays[i, j] == 1) { area[3]++; }
                    }
                }
            }
            if (!limits.Contains(4))
            {
                area.Add(4, 0);
                for (int i = currentY - U; i <= currentY + D; i++)
                {
                    for (int j = currentX - L; j < currentX; j++)
                    {
                        if (passedWays[i, j] == 1) { area[4]++; }
                    }
                }
            }

            int smallestValue = area.Min(x => x.Value);
            var keysWithSmallestValue = area.Where(x => x.Value == smallestValue).Select(x => x.Key).ToList();

            Random random = new Random();
            int randomIndex = random.Next(0, keysWithSmallestValue.Count);

            int randomlySelectedKey = keysWithSmallestValue[randomIndex];

            return randomlySelectedKey;
        }

        public Location isChestOnSight(int currentX, int currentY) 
        {
            int L = 3, R = 3, U = 3, D = 3;

            if (currentX - 3 < 0) { L = currentX; }
            if (currentX + 3 > Form1.numberOfLines - 1) { R = Form1.numberOfLines - 1 - currentX; }
            if (currentY - 3 < 0) { U = currentY; }
            if (currentY + 3 > Form1.numberOfLines - 1) { D = Form1.numberOfLines - 1 - currentY; }

            for (int i = currentY - U; i <= currentY + D; i++)
            {
                for (int j = currentX - L; j <= currentX + R; j++)
                {
                    if (Program.mapArray[i ,j] <= 4 && Program.mapArray[i, j] >= 1) 
                    {
                        Console.WriteLine(j + ", " + i);
                        Console.ReadLine();

                        chestLocations.Add(new Location(j, i));
                        return new Location(j, i); 
                    }
                }
            }

            return null;
        }

        private Location chooseRandomNextLocation(int currentX, int currentY, List<int> limits)
        {
            List<int> notLimits = new List<int>() { 1, 2, 3, 4 };
            for (int i = 0; i < limits.Count; i++) { notLimits.Remove(limits[i]); }

            Random random = new Random();
            Location nextLocation = new Location(0, 0);
            int selectedIndex = random.Next(0, notLimits.Count);
            int direction = notLimits[selectedIndex]; // 1 yukarı, 2 sag, 3 asagi, 4 sola 

            switch (direction)
            {
                case 1:
                    nextLocation = new Location(currentX, currentY - 1);
                    break;
                case 2:
                    nextLocation = new Location(currentX + 1, currentY);
                    break;
                case 3:
                    nextLocation = new Location(currentX, currentY + 1);
                    break;
                case 4:
                    nextLocation = new Location(currentX - 1, currentY);
                    break;
            }

            return nextLocation;
        }

        private bool checkObstacle(int x, int y)
        {
            if (Program.mapArray[y, x] > 4) { return true; }
            return false;
        }

        private List<int> findLimit(int currentX, int currentY)
        {
            List<int> limits = new List<int>();// 1 yukarı, 2 sag, 3 asagi, 4 sola

            if (currentY == 0) { limits.Add(1); }
            if (currentX == Form1.numberOfLines - 1) { limits.Add(2); }
            if (currentY == Form1.numberOfLines - 1) { limits.Add(3); }
            if (currentX == 0) { limits.Add(4); }

            return limits;
        }

        public void setVisibleWays(int currentX, int currentY)
        {
            int L = 3, R = 3, U = 3, D = 3;

            if (currentX - 3 < 0) { L = currentX; }
            if (currentX + 3 > Form1.numberOfLines - 1) { R = Form1.numberOfLines - 1 - currentX; }
            if (currentY - 3 < 0) { U = currentY; }
            if (currentY + 3 > Form1.numberOfLines - 1) { D = Form1.numberOfLines - 1 - currentY; }


            for (int i = currentY - U; i <= currentY + D; i++)
            {
                for (int j = currentX - L; j <= currentX + R; j++)
                {
                    visibleWays[i, j] = 1;
                    chechkingObstacle(j, i);
                }
            }
        }

        private void chechkingObstacle(int currentX, int currentY)
        {
            if (Program.mapArray[currentY, currentX] > 4 && seenObstacles[currentY, currentX] != 1)
            {
                chooseLabel(Program.mapArray[currentY, currentX]);

                while (Program.mapArray[currentY - 1, currentX] != 0)
                {
                    currentY--;
                }
                while (Program.mapArray[currentY, currentX - 1] != 0)
                {
                    currentX--;
                }

                while (Program.mapArray[currentY, currentX] > 4)
                {
                    int tempX = currentX;
                    while (Program.mapArray[currentY, tempX] > 4)
                    {
                        seenObstacles[currentY, tempX] = 1;
                        tempX++;
                    }

                    currentY++;
                }
            }
        }

        private void chooseLabel(int type)
        {
            string str = "";

            switch (type)
            {
                case 5:
                    str = "Kuş bulundu!";
                    break;
                case 6:
                    str = "Arı bulundu!";
                    break;
                case 7:
                    str = "Dağ bulundu!";
                    break;
                case 8:
                    str = "Duvar bulundu!";
                    break;
                case 9:
                    str = "Kaya bulundu!";
                    break;
                case 10:
                    str = "Ağaç bulundu!";
                    break;
            }

            OverlayedForm.obstacleText = str + "\n";
            OverlayedForm.add = true;
        }


        int[] dx = { 1, 0, -1, 0 };
        int[] dy = { 0, 1, 0, -1 };

        private bool IsSafe(int x, int y, int n)
        {
            return x >= 0 && x < n && y >= 0 && y < n && Program.mapArray[x, y] <= 4;
        }

        public List<int[]> goForChest(int[] start, int[] end)
        {
            int n = Program.mapArray.GetLength(0);
            bool[,] visited = new bool[n, n];
            int[,] parentX = new int[n, n];
            int[,] parentY = new int[n, n];

            Queue<int[]> queue = new Queue<int[]>();
            queue.Enqueue(start);
            visited[start[0], start[1]] = true;

            while (queue.Count > 0)
            {
                int[] current = queue.Dequeue();
                int x = current[0];
                int y = current[1];

                if (x == end[0] && y == end[1])
                {
                    // Hedefe ulaşıldı, geriye giden yolu bul
                    List<int[]> path = new List<int[]>();
                    while (x != start[0] || y != start[1])
                    {
                        path.Add(new int[] { x, y });
                        int tempX = parentX[x, y];
                        y = parentY[x, y];
                        x = tempX;
                    }
                    path.Add(new int[] { start[0], start[1] });
                    path.Reverse();
                    return path;
                }

                for (int i = 0; i < 4; i++)
                {
                    int nextX = x + dx[i];
                    int nextY = y + dy[i];

                    if (IsSafe(nextX, nextY, n) && !visited[nextX, nextY])
                    {
                        queue.Enqueue(new int[] { nextX, nextY });
                        visited[nextX, nextY] = true;
                        parentX[nextX, nextY] = x;
                        parentY[nextX, nextY] = y;
                    }
                }
            }

            // Hedefe ulaşılamadı
            return new List<int[]>();
        }

        public void removeChest(int currentX, int currentY, Graphics g)
        {
            int[] dx = { 0, 1, 0, -1, 0, 1, -1, 1, -1 };
            int[] dy = { 0, 0, 1, 0, -1, 1, -1, -1, 1 };

            for (int i = 0; i < 9; i++)
            {
                int nextX = currentX + dx[i];
                int nextY = currentY + dy[i];

                if (Program.mapArray[nextY, nextX] >= 1 && Program.mapArray[nextY, nextX] <= 4)
                {
                    Program.mapArray[nextY, nextX] = 0;
                    paintedChests[nextY, nextX] = 1;

                    if (passedWays[nextY, nextX] == 0)
                    {
                        g.FillRectangle(Brushes.White, nextX * Form1.squareLength,
                        nextY * Form1.squareLength, Form1.squareLength, Form1.squareLength);
                    }
                }
            }

            Program.mapArray[currentY, currentX] = 0;
        }

        public void clearFog(int currentX, int currentY, Graphics g)
        {
            int L = 3, R = 3, U = 3, D = 3;

            if (currentX - 3 < 0) { L = currentX; }
            if (currentX + 3 > Form1.numberOfLines - 1) { R = Form1.numberOfLines - 1 - currentX; }
            if (currentY - 3 < 0) { U = currentY; }
            if (currentY + 3 > Form1.numberOfLines - 1) { D = Form1.numberOfLines - 1 - currentY; }


            for (int i = currentY - U; i <= currentY + D; i++)
            {
                for (int j = currentX - L; j <= currentX + R; j++)
                {
                    if (passedWays[i, j] != 1 && paintedChests[i, j] != 1)
                    {
                        g.FillRectangle(Brushes.Tomato, j * Form1.squareLength,
                            i * Form1.squareLength, Form1.squareLength, Form1.squareLength);
                    }
                }
            }
        }

        private bool isSafe(int x, int y, int n)
        {
            return x >= 0 && x < n && y >= 0 && y < n && passedWays[x, y] == 1;
        }

        private List<int[]> shortestWayToChests(int[] start, int[] end)
        {
            int n = passedWays.GetLength(0);
            bool[,] visited = new bool[n, n];
            int[,] parentX = new int[n, n];
            int[,] parentY = new int[n, n];

            Queue<int[]> queue = new Queue<int[]>();
            queue.Enqueue(start);
            visited[start[0], start[1]] = true;

            while (queue.Count > 0)
            {
                int[] current = queue.Dequeue();
                int x = current[0];
                int y = current[1];

                if (x == end[0] && y == end[1])
                {
                    // Hedefe ulaşıldı, geriye giden yolu bul
                    List<int[]> path = new List<int[]>();
                    while (x != start[0] || y != start[1])
                    {
                        path.Add(new int[] { x, y });
                        int tempX = parentX[x, y];
                        y = parentY[x, y];
                        x = tempX;
                    }
                    path.Add(new int[] { start[0], start[1] });
                    path.Reverse();
                    return path;
                }

                for (int i = 0; i < 4; i++)
                {
                    int nextX = x + dx[i];
                    int nextY = y + dy[i];

                    if (isSafe(nextX, nextY, n) && !visited[nextX, nextY])
                    {
                        queue.Enqueue(new int[] { nextX, nextY });
                        visited[nextX, nextY] = true;
                        parentX[nextX, nextY] = x;
                        parentY[nextX, nextY] = y;
                    }
                }
            }

            // Hedefe ulaşılamadı
            return new List<int[]>();
        }

        public void finalWay(Graphics g)
        {
            chestLocations.Insert(0, locations[0]);

            for (int i = 0; i < chestLocations.Count - 1; i++)
            {
                int[] start = { chestLocations[i].y, chestLocations[i].x };
                int[] end = { chestLocations[i + 1].y, chestLocations[i + 1].x };
                List<int[]> path = shortestWayToChests(start, end);

                for (int j = 0; j < path.Count; j++)
                {
                    int[] point = path[j];

                    g.FillRectangle(Brushes.LawnGreen, point[1] * Form1.squareLength,
                    point[0] * Form1.squareLength, Form1.squareLength, Form1.squareLength);

                    stepCount++;
                }

                stepCount--;
            }

            g.FillRectangle(Brushes.Aqua, chestLocations[0].x * Form1.squareLength,
            chestLocations[0].y * Form1.squareLength, Form1.squareLength, Form1.squareLength);
            g.FillRectangle(Brushes.RoyalBlue, chestLocations[chestLocations.Count - 1].x * Form1.squareLength,
            chestLocations[chestLocations.Count - 1].y * Form1.squareLength, Form1.squareLength, Form1.squareLength);
        }
    }
}
