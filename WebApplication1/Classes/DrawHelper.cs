using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Classes;

namespace TrafficSimulation.Classes
{
    public static  class DrawHelper
    {
        public static string Topology { get; set; }
        public static bool UseTrafficLight { get; set; }

        public static Image DrawCars(List<Car> cars)
        {
            Image image = new Bitmap(500, 1000);
            Graphics graphics = Graphics.FromImage(image);


            switch (Topology)
            {
                case "Сетка":
                    foreach (var car in cars)
                    {
                        DrawCarGrid(car, graphics);
                    }

                    if (UseTrafficLight)
                    {
                        Crossroads[,] crossroadsArray = SocketHelper.GetInstance().GetCrossroadses();

                        foreach (var crossroads in crossroadsArray)
                        {
                            DrawTrafficLight(graphics, crossroads);
                        }
                    }
                    break;
                case "Круги":
                    foreach (var car in cars)
                    {
                        DrawCarCircle(car, graphics);
                    }
                    break;
                default:
                    image = null;
                    break;
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

        public static void DrawTrafficLight(Graphics g,Crossroads crossroads)
        {
            Point centre = crossroads.Point;
            centre.X -= 2;
            centre.Y -= 2;

            Point trafficLight;
            Size size = new Size(5, 5);
            Rectangle rectangle;
            SolidBrush brushFirst = null;
            SolidBrush brushSecond = null;
            int offset = 20;

            switch (crossroads.Phase)
            {
                case Phase.First:
                    brushFirst = new SolidBrush(Color.Red);
                    brushSecond = new SolidBrush(Color.Lime);
                    break;
                case Phase.Second:
                    brushFirst = new SolidBrush(Color.Lime);
                    brushSecond = new SolidBrush(Color.Red);
                    break;
                case Phase.Green:
                    return;
                default:
                    break;
            }

            //Слева вверху
            trafficLight = centre;
            trafficLight.X -= offset;
            trafficLight.Y -= offset;

            rectangle = new Rectangle(trafficLight, size);
            g.FillRectangle(brushFirst, rectangle);

            //Справа вверху
            trafficLight = centre;
            trafficLight.X += offset;
            trafficLight.Y -= offset;

            rectangle = new Rectangle(trafficLight, size);
            g.FillRectangle(brushSecond, rectangle);

            //Слева внизу
            trafficLight = centre;
            trafficLight.X -= offset;
            trafficLight.Y += offset;

            rectangle = new Rectangle(trafficLight, size);
            g.FillRectangle(brushSecond, rectangle);

            //Справа внизу
            trafficLight = centre;
            trafficLight.X += offset;
            trafficLight.Y += offset;

            rectangle = new Rectangle(trafficLight, size);
            g.FillRectangle(brushFirst, rectangle);
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

        public static void DrawCarCircle(Car car, Graphics graphics)
        {
            int Angle = car.Angle;
            Point Current = car.Current;
            Size size = new Size(11, 11);
            Rectangle rectangle = new Rectangle(Current, size);
            rectangle.X -= 5;
            rectangle.Y -= 5;

            //graphics.FillEllipse(new SolidBrush(Color.Red), rectangle);
            Image carImage = new Bitmap("Resources/CarUp.png");
            carImage = DrawHelper.RotateImage(carImage, -Angle);
            graphics.DrawImage(carImage, rectangle);
        }

        public static Image RotateImage(Image img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new Point(0, 0));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }

        public static Image DrawCrossroads(Crossroads[,] crossroadsArray, bool useTrafficLight)
        {
            Image image;

            switch (Topology)
            {
                case "Сетка":
                    image = DrawHelper.DrawGrid(crossroadsArray, false);
                    break;
                case "Круги":
                    image = DrawHelper.DrawCircles(crossroadsArray);
                    break;
                default:
                    image = null;
                    break;
            }

            return image;
        }

        public static Image DrawGrid(Crossroads[,] crossroadsArray, bool useTrafficLight)
        {
            int countRow = crossroadsArray.GetLength(1);
            int countColumn = crossroadsArray.GetLength(0);

            Image background = new Bitmap(1000, 1000);
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

        public static Image DrawCircles(Crossroads[,] crossroadsArray)
        {
            int countRow = crossroadsArray.GetLength(1);
            int countColumn = crossroadsArray.GetLength(0);

            Image background = new Bitmap(1000, 1000);
            Graphics g = Graphics.FromImage(background);

            //Смещение относительно начала координат
            int xOffset = 50;
            int yOffset = 50;

            int[] radius = new int[3] { 45, 35, 25 };

            SolidBrush brushGray = new SolidBrush(Color.Gray);

            Pen penBlack = new Pen(Color.Black);
            int widthRoad = 10;
            Point start;
            Point end;
            Size size;
            Rectangle rectangle;

            for (int i = 0; i < countColumn; i++)
            {
                for (int k = 0; k < countRow; k++)
                {
                    foreach (var r in radius)
                    {
                        //Рисование кругов
                        start = crossroadsArray[i, k].Point;
                        start.X -= r;
                        start.Y -= r;

                        size = new Size(r * 2, r * 2);
                        rectangle = new Rectangle(start, size);

                        if (r == 25)
                        {
                            SolidBrush brushGreen = new SolidBrush(Color.Green);
                            g.FillEllipse(brushGreen, rectangle);
                        }
                        else
                        {
                            g.FillEllipse(brushGray, rectangle);
                        }


                        if (r == 35)
                        {
                            Pen penWhite = new Pen(Color.White);
                            penWhite.Color = Color.White;
                            penWhite.DashStyle = DashStyle.Dash;
                            g.DrawEllipse(penWhite, rectangle);
                        }
                        else
                        {
                            g.DrawEllipse(penBlack, rectangle);
                        }

                        //Image house = new Bitmap(Properties.Resources.House);
                        //g.DrawImage(house, rectangle);
                    }

                    DrawRoad(crossroadsArray[i, k].Point, radius[0], g);
                }
            }

            return background;
        }

        private static void DrawRoad(Point circle, int r, Graphics g)
        {

            Pen pen = new Pen(Color.Black);
            SolidBrush brushGrey = new SolidBrush(Color.Gray);
            Point start;
            Point end;

            Size size = new Size(13, 20);
            Rectangle rectangle;

            //Слева
            start = circle;
            start.X -= r + 11;
            start.Y -= 10;

            rectangle = new Rectangle(start, size);
            g.FillRectangle(brushGrey, rectangle);

            //Справа
            start = circle;
            start.X += r - 1;
            start.Y -= 10;

            rectangle = new Rectangle(start, size);
            g.FillRectangle(brushGrey, rectangle);

            size = new Size(20, 13);

            //Сверху
            start = circle;
            start.X -= 10;
            start.Y -= r + 11;

            rectangle = new Rectangle(start, size);
            g.FillRectangle(brushGrey, rectangle);

            //Снизу
            start = circle;
            start.X -= 10;
            start.Y += r - 1;

            rectangle = new Rectangle(start, size);
            g.FillRectangle(brushGrey, rectangle);

            for (int i = -10; i < 20; i += 10)
            {
                if (i == 0)
                {
                    pen.Color = Color.White;
                    pen.DashStyle = DashStyle.Dash;
                }
                else
                {
                    pen.Color = Color.Black;
                    pen.DashStyle = DashStyle.Solid;
                }

                //Слева
                start = circle;
                start.X -= r;
                start.Y -= i;

                end = start;
                end.X -= 10;

                g.DrawLine(pen, start, end);

                //Справа
                start = circle;
                start.X += r;
                start.Y -= i;

                end = start;
                end.X += 10;

                g.DrawLine(pen, start, end);

                //Сверху
                start = circle;
                start.X -= i;
                start.Y -= r;

                end = start;
                end.Y -= 10;

                g.DrawLine(pen, start, end);

                //Снизу
                start = circle;
                start.X -= i;
                start.Y += r;

                end = start;
                end.Y += 10;

                g.DrawLine(pen, start, end);
            }
        }
    }
}
