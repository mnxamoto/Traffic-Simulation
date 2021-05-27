using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficSimulation.Classes
{
    public static  class DrawHelper
    {
        public static Image DrawGrid(Crossroads[,] crossroadsArray, bool useTrafficLight)
        {
            int countRow = crossroadsArray.GetLength(1);
            int countColumn = crossroadsArray.GetLength(0);

            Image background = new Bitmap(1000, 500);
            Graphics g = Graphics.FromImage(background);
            Random random = new Random(3);

            SolidBrush brushGray = new SolidBrush(Color.Gray);

            Pen penBlack = new Pen(Color.Black);
            Pen penWhite = new Pen(Color.White);
            penWhite.DashStyle = DashStyle.Dash;
            int widthRoad = 10 * 2;
            Point start;
            Point end;
            Size size;
            Rectangle rectangle;

            //TODO: Нарисовать сами дороги

            for (int i = 0; i < countColumn - 1; i++)
            {
                for (int k = 0; k < countRow - 1; k++)
                {
                    //Внутренние границы и домики
                    start = crossroadsArray[i, k].Point;
                    start.X += widthRoad;
                    start.Y += widthRoad;

                    size = new Size(60, 60);
                    rectangle = new Rectangle(start, size);

                    Image house = new Bitmap("Resources/House.jpg");
                    g.DrawImage(house, rectangle);

                    g.DrawRectangle(penBlack, rectangle);
                }
            }

            //Внешние границы
            start = crossroadsArray[0, 0].Point;
            end = crossroadsArray[0, countRow - 1].Point;

            start.X -= widthRoad;
            start.Y -= widthRoad;
            end.X -= widthRoad;
            end.Y += widthRoad;

            g.DrawLine(penBlack, start, end);

            start = crossroadsArray[0, 0].Point;
            end = crossroadsArray[countColumn - 1, 0].Point;

            start.X -= widthRoad;
            start.Y -= widthRoad;
            end.X += widthRoad;
            end.Y -= widthRoad;

            g.DrawLine(penBlack, start, end);

            start = crossroadsArray[0, countRow - 1].Point;
            end = crossroadsArray[countColumn - 1, countRow - 1].Point;

            start.X -= widthRoad;
            start.Y += widthRoad;
            end.X += widthRoad;
            end.Y += widthRoad;

            g.DrawLine(penBlack, start, end);

            start = crossroadsArray[countColumn - 1, 0].Point;
            end = crossroadsArray[countColumn - 1, countRow - 1].Point;

            start.X += widthRoad;
            start.Y -= widthRoad;
            end.X += widthRoad;
            end.Y += widthRoad;

            g.DrawLine(penBlack, start, end);

            return background;
        }
    }
}
