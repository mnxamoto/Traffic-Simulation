using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TrafficSimulation.Classes;
using WebApplication1.Classes;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string Message { get; set; }
        public bool IsWorked { get; set; }
        [BindProperty]
        public bool UseTrafficLight { get; set; }
        [BindProperty]
        public string Topology { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            IsWorked = false;
        }

        public void OnPost(int countRow, int countColumm, int countCar, int minSpeed, int maxSpeed, string btn1)
        {
            switch (btn1)
            {
                case "Подключиться":


                    if (Topology != null)
                    {
                        if (SocketHelper.GetInstance().Connect())
                        {
                            DrawHelper.Topology = Topology;
                            DrawHelper.UseTrafficLight = UseTrafficLight;
                            Message = "Соединение установлено";

                            StartInfo info = new StartInfo();
                            info.countRow = countRow;
                            info.countColumm = countColumm;
                            info.countCar = countCar;
                            info.minSpeed = minSpeed;
                            info.maxSpeed = maxSpeed;
                            info.useTrafficLight = UseTrafficLight;

                            switch (Topology)
                            {
                                case "Сетка":
                                    SocketHelper.GetInstance().Send(Command.StartGrid, info);
                                    break;
                                case "Круги":
                                    SocketHelper.GetInstance().Send(Command.StartCircle, info);
                                    break;
                                default:
                                    break;
                            }

                            Thread.Sleep(10000);

                            IsWorked = true;
                        }
                        else
                        {
                            Message = "Увы :(";
                        }
                    }
                    else
                    {
                        Message = "Выберите топологию";
                    }
                    break;
                case "Остановить":
                    IsWorked = false;
                    SocketHelper.GetInstance().Send(Command.Stop);
                    break;
                default:
                    break;
            }
        }
    }
}
