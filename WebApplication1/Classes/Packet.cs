using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Classes
{
    public class Packet
    {
        public Command Command;
        public string data;
    }

    public class StartInfo
    {
        public int countRow;
        public int countColumm;
        public int countCar;
        public int minSpeed;
        public int maxSpeed;
        public bool useTrafficLight;
    }

    public enum Command
    {
        StartGrid,
        StartCircle,
        GetCrossroadses,
        SendCrossroadses,
        GetCars,
        SendCars
    }
}
