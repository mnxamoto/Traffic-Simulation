using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrafficSimulation.Classes;

namespace WebApplication1.Classes
{
    public class SocketHelper
    {
        private static SocketHelper instance;

        private SocketHelper()
        { }

        public static SocketHelper GetInstance()
        {
            if (instance == null)
                instance = new SocketHelper();
            return instance;
        }

        private static Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public bool IsSend { get; set; }

        public bool Connect()
        {
            if (!IsSend)
            {
                //Соединяем сокет с удаленной точкой (сервером)
                server.Connect(IPAddress.Parse("127.0.0.1"), 1001);
                Console.Write($"Соединение с [{server.RemoteEndPoint}] установлено");

                IsSend = true;
            }

            return true;
        }

        public Crossroads[,] GetCrossroadses()
        {
            while (!IsSend)
            {

            }

            IsSend = false;
            Send(Command.GetCrossroadses);
            Packet packet = GetPacket();
            Crossroads[,] crossroadsArray = JsonConvert.DeserializeObject<Crossroads[,]>(packet.data);
            IsSend = true;
            return crossroadsArray;
        }

        public List<Car> GetCars()
        {
            List<Car> cars;

            if (IsSend)
            {
                IsSend = false;
                Send(Command.GetCars);
                Packet packet = GetPacket();
                cars = JsonConvert.DeserializeObject<List<Car>>(packet.data);
                IsSend = true;
                return cars;
            }
            else
            {
                return null;
            }
        }

        public void Send(Command command, object messageObject)
        {
            string messageString = JsonConvert.SerializeObject(messageObject);

            Packet packet = new Packet();
            packet.Command = command;
            packet.data = messageString;

            string packetString = JsonConvert.SerializeObject(packet);
            byte[] packetBytes = Encoding.GetEncoding("Unicode").GetBytes(packetString);

            try
            {
                server.Send(packetBytes);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Send(Command command)
        {
            Packet packet = new Packet();
            packet.Command = command;

            string packetString = JsonConvert.SerializeObject(packet);
            byte[] packetBytes = Encoding.GetEncoding("Unicode").GetBytes(packetString);

            try
            {
                server.Send(packetBytes);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ReceiveMesssage()
        {
            try
            {
                //Постоянно слушаем входящий трафик
                while (true)
                {
                    string message;

                    // Получаем пакет от сервера
                    Packet packet = GetPacket();
                    /*
                    //В зависимости от команды выполняется определённое действие
                    switch (packet.Command)
                    {
                        case Command.Start:
                            break;
                        default:
                            break;
                    }*/
                }
            }
            catch (Exception ex)
            {
                server.Close();
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
        }

        static Packet GetPacket()
        {
            byte[] bytes = new byte[65535];
            int bytesRec = server.Receive(bytes);
            string data = Encoding.GetEncoding("Unicode").GetString(bytes, 0, bytesRec);
            Packet packet = JsonConvert.DeserializeObject<Packet>(data);

            return packet;
        }
    }
}