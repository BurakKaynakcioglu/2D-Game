using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prolabbb
{
    internal static class Program
    {
        // 0 - engel veya sandık olmayan kareler
        // 1 - gold chest
        // 2 - silver chest
        // 3 - emerald chest
        // 4 - bronze chest
        // 5 - bird
        // 6 - bee
        // 7 - mountain
        // 8 - wall
        // 9 - rock
        // 10 - tree
        
        public static string path = "C:\\Users\\kkayn\\OneDrive\\Masaüstü\\BillabProje\\";
        public static int[,] mapArray; // harita oluştuğu zaman static olarak oyuntahtası matrisi buraya gelecek. referansı gönderrdikten sonra 

        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
