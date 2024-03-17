using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prolabbb
{
    internal class Obstacle
    {
        public Location location { get; set; }
        public Panel panel { get; set; }

        public Obstacle(Location location, Panel panel)
        {
            this.location = location;
            this.panel = panel;
        }
    }
}
