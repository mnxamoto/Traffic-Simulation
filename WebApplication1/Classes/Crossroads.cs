using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficSimulation.Classes
{
    public class Crossroads
    {
        public Point Point { get; set; }
        public Phase Phase { get; set; }
    }

    public enum Phase
    {
        First = 0,
        Second = 1,
        Green = 2
    }
}
