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
using WebApplication1.Classes;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string Message { get; set; }
        public bool IsWorked { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            IsWorked = false;
        }

        public void OnPost(int countRow, int countColumm, int countCar, int minSpeed, int maxSpeed)
        {
            if (SocketHelper.GetInstance().Connect())
            {
                Message = "Соединение установлено";

                StartInfo info = new StartInfo();
                info.countRow = countRow;
                info.countColumm = countColumm;
                info.countCar = countCar;
                info.minSpeed = minSpeed;
                info.maxSpeed = maxSpeed;
                info.useTrafficLight = false;

                SocketHelper.GetInstance().Send(Command.Start, info);

                Thread.Sleep(10000);

                IsWorked = true;
            }
            else
            {
                Message = "Увы :(";
            }
        }
    }
}
