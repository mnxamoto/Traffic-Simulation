using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnPost(int sum1, int sum2)
        {
            StartInfo info = new StartInfo();
            info.countRow = sum1;
            info.countColumm = sum2;
            info.countCar = 3;
            info.minSpeed = 4;
            info.maxSpeed = 5;

            if (SocketHelper.GetInstance().Connect())
            {
                Message = "Соединение установлено";



                SocketHelper.GetInstance().Send(Command.Start, info);
            }
            else
            {
                Message = "Увы :(";
            }
        }
    }
}
