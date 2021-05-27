using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        private Thread Thread1;

        public bool Connect()
        {
            //Соединяем сокет с удаленной точкой (сервером)
            server.Connect(IPAddress.Parse("127.0.0.1"), 1001);
            Console.Write($"Соединение с [{server.RemoteEndPoint}] установлено");

            //Отдельный поток для приёма входящих пакетов и ответа на них
            Thread1 = new Thread(delegate ()
            {
                //ReceiveMesssage();
            });

            //Запускаем этот поток
            //Thread1.Start();

            //Отправляем информацию о клиенте на сервер
            //Send(Command.Connect, "");

            return true;
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

                    //В зависимости от команды выполняется определённое действие
                    switch (packet.Command)
                    {
                        case Command.Start:
                            break;
                        default:
                            break;
                    }
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
            string data = Encoding.GetEncoding(866).GetString(bytes, 0, bytesRec);
            Packet packet = JsonConvert.DeserializeObject<Packet>(data);

            return packet;
        }
    }
}
