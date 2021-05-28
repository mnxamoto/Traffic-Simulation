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
        public static Image DrawCars(List<Car> cars)
        {
            Image image = new Bitmap(500, 1000);
            Graphics graphics = Graphics.FromImage(image);

            foreach (var car in cars)
            {
                DrawCarGrid(car, graphics);
            }
            
            /*
            if (false)
            {
                foreach (var crossroads in crossroadsArray)
                {
                    crossroads.DrawTrafficLight(graphics);
                }
            }*/

            return image;
        }

        public static void DrawCarGrid(Car car, Graphics graphics)
        {
            Point Current = car.Current;

            Rectangle rectangle = new Rectangle(Current, new Size(11, 11));

            int offset = 5;
            int offsetRoad = 5;
            DirectionMotion dr = car.DirectionMotion;
            DirectionMotion drn = car.DirectionMotionNext;

            if (((dr == DirectionMotion.Up) && (drn == DirectionMotion.Right)) ||
                ((dr == DirectionMotion.Right) && (drn == DirectionMotion.Down)) ||
                ((dr == DirectionMotion.Down) && (drn == DirectionMotion.Left)) ||
                ((dr == DirectionMotion.Left) && (drn == DirectionMotion.Up)))
            {
                offsetRoad = 15;
            }

            Image carImage = new Bitmap(1, 1);

            switch (car.DirectionMotion)
            {
                case DirectionMotion.Up:
                    rectangle.X += -offset + offsetRoad;
                    rectangle.Y += -offset;
                    carImage = new Bitmap("Resources/CarUp.png");
                    break;
                case DirectionMotion.Right:
                    rectangle.X += -offset;
                    rectangle.Y += -offset + offsetRoad;
                    carImage = new Bitmap("Resources/CarRight.png");
                    break;
                case DirectionMotion.Down:
                    rectangle.X += -offset - offsetRoad;
                    rectangle.Y += -offset;
                    carImage = new Bitmap("Resources/CarDown.png");
                    break;
                case DirectionMotion.Left:
                    rectangle.X += -offset;
                    rectangle.Y += -offset - offsetRoad;
                    carImage = new Bitmap("Resources/CarLeft.png");
                    break;
                default:
                    break;
            }

            //graphics.FillRectangle(new SolidBrush(Color.Red), rectangle);

            graphics.DrawImage(carImage, rectangle);
        }

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

            //Рисование дорог
            int numberStrips = 2;

            for (int i = 0; i < countColumn; i++)
            {
                for (int k = 0; k < countRow - 1; k++)
                {
                    //Асфальт
                    start = crossroadsArray[i, k].Point;
                    start.X -= widthRoad;
                    start.Y -= widthRoad;

                    size = new Size(widthRoad * numberStrips, 100 + (widthRoad * 2));
                    rectangle = new Rectangle(start, size);
                    g.FillRectangle(brushGray, rectangle);

                    //Разметка
                    //Сплошная
                    penWhite.DashStyle = DashStyle.Solid;

                    start = crossroadsArray[i, k].Point;
                    end = crossroadsArray[i, k + 1].Point;
                    start.X -= 0;
                    start.Y += widthRoad;
                    end.X += 0;
                    end.Y -= widthRoad;

                    g.DrawLine(penWhite, start, end);

                    //Пунктирная
                    penWhite.DashStyle = DashStyle.Dash;

                    start.X -= widthRoad / 2;
                    end.X -= widthRoad / 2;

                    g.DrawLine(penWhite, start, end);

                    start.X += widthRoad;
                    end.X += widthRoad;

                    g.DrawLine(penWhite, start, end);
                }
            }

            for (int k = 0; k < countRow; k++)
            {
                for (int i = 0; i < countColumn - 1; i++)
                {
                    //Асфальт
                    start = crossroadsArray[i, k].Point;
                    start.X -= widthRoad;
                    start.Y -= widthRoad;

                    size = new Size(100 + (widthRoad * 2), widthRoad * numberStrips);
                    rectangle = new Rectangle(start, size);
                    g.FillRectangle(brushGray, rectangle);

                    //Разметка
                    //Сплошная
                    penWhite.DashStyle = DashStyle.Solid;

                    start = crossroadsArray[i, k].Point;
                    end = crossroadsArray[i + 1, k].Point;
                    start.X += widthRoad;
                    start.Y += 0;
                    end.X -= widthRoad;
                    end.Y -= 0;

                    g.DrawLine(penWhite, start, end);

                    //Пунктирная
                    penWhite.DashStyle = DashStyle.Dash;

                    start.Y -= (widthRoad / 2);
                    end.Y -= (widthRoad / 2);

                    g.DrawLine(penWhite, start, end);

                    start.Y += widthRoad;
                    end.Y += widthRoad;

                    g.DrawLine(penWhite, start, end);
                }
            }

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
