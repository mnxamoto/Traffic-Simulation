using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficSimulation.Classes
{
    public class Car
    {
        public Point Current { get; set; }
        public DirectionMotion DirectionMotion { get; set; }
        public DirectionMotion DirectionMotionNext { get; set; }
        public int Angle { get; set; }
    }

    public enum DirectionMotion
    {
        Up,
        Right,
        Down,
        Left
    }
}
