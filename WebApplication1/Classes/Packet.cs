using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Classes
{
    public class Packet
    {
        public Command Command;
        public string text;
    }

    public enum Command
    {
        Connect
    }
}
