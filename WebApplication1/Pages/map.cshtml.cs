using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrafficSimulation.Classes;
using WebApplication1.Classes;

namespace TrafficSimulation.Pages
{
    public class mapModel : PageModel
    {
        public ActionResult OnGet()
        {
            List<Car> cars = SocketHelper.GetInstance().GetCars();

            if (cars == null)
            {
                return null;
            }
            else
            {
                Image image = DrawHelper.DrawCars(cars);

                var outputStream = new MemoryStream();
                image.Save(outputStream, ImageFormat.Png);
                outputStream.Seek(0, SeekOrigin.Begin);

                return File(outputStream, "image/png");
            }
        }
    }
}
