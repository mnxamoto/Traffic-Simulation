using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrafficSimulation.Classes;
using WebApplication1.Classes;

namespace TrafficSimulation.Pages
{
    public class backgroundModel : PageModel
    {
        public ActionResult OnGet()
        {
            Thread.Sleep(1000);

            Crossroads[,] crossroadsArray = SocketHelper.GetInstance().GetCrossroadses();
            Image image = DrawHelper.DrawCrossroads(crossroadsArray, false);
            var outputStream = new MemoryStream();
            image.Save(outputStream, ImageFormat.Png);
            outputStream.Seek(0, SeekOrigin.Begin);

            return File(outputStream, "image/png");
        }
    }
}