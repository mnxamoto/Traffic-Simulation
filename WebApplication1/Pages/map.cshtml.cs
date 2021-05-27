using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TrafficSimulation.Pages
{
    public class mapModel : PageModel
    {
        public ActionResult OnGet()
        {
            Random random = new Random(5);

            Image image2 = new Bitmap(1000, 500);
            Graphics graphics = Graphics.FromImage(image2);
            Point point = new Point(random.Next(0, 100), random.Next(0, 100));
            Size size = new Size(DateTime.Now.Second * 20, random.Next(10, 1000));
            Rectangle rectangle = new Rectangle(point, size);
            SolidBrush brush = new SolidBrush(Color.Red);
            graphics.FillRectangle(brush, rectangle);

            Image Image = new Bitmap($"Resources/CarLeft.png"); //TODO: Сделать вызов функции формирования Image

            var outputStream = new MemoryStream();
            image2.Save(outputStream, ImageFormat.Png);
            outputStream.Seek(0, SeekOrigin.Begin);

            return File(outputStream, "image/png");
        }
    }
}
