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
    public class map2Model : PageModel
    {
        public ActionResult OnGet()
        {
            Image Image = new Bitmap($"Resources/CarUp.png");

            var outputStream = new MemoryStream();
            Image.Save(outputStream, ImageFormat.Png);
            outputStream.Seek(0, SeekOrigin.Begin);

            return File(outputStream, "image/png");
        }
    }
}
