using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GDI.Form3;

namespace GDI
{
    public class Shape
    {
        public drawTools DrawTool;

        public Pen pen;

        public Point start, end;

        public bool Fill;

        public Color FillColor;

        public List<Point> Points;


        //三个构造函数
        public Shape(Pen pen,int x,int y ,int w,int h, drawTools DrawTool,bool fill,Color FillColor)
        {
            this.DrawTool = DrawTool;
            this.pen =new Pen(System.Drawing.Color.Red, 2);
            this.pen.Color = pen.Color;
            start = new Point(x, y);
            end = new Point(w, h);
            if (fill == true)
            {
                Fill = fill;
                this.FillColor = FillColor;
            }
            else Fill = fill;
        }

        public Shape(Pen pen, List<Point> points,drawTools DrawTool)
        {
            this.DrawTool = DrawTool;
            this.pen = new Pen(System.Drawing.Color.Red, 2);
            this.pen.Color = pen.Color;
            this.pen.Width = pen.Width;
            Points = points;
        }

        public Shape(Shape shape)
        {
            DrawTool = shape.DrawTool;
            pen = new Pen(System.Drawing.Color.Red, 2);
            pen.Color = shape.pen.Color;
            pen.Width = shape.pen.Width;
            Fill = shape.Fill;
            FillColor = shape.FillColor;
            end = shape.end;
            start = shape.start;
        }

        //画图函数
        public void Show(Graphics g)
        {
            switch(DrawTool)
            {
                case drawTools.Rectangle:
                    if(!Fill) g.DrawRectangle(pen, start.X,start.Y,end.X,end.Y);
                    else
                    {
                        SolidBrush b1 = new SolidBrush(FillColor);//
                        g.FillRectangle(b1, start.X, start.Y, end.X, end.Y);
                    }
                    break;
                case drawTools.Ellipse:
                    if (!Fill)
                        g.DrawEllipse(pen, start.X, start.Y, end.X, end.Y);
                    else
                    {
                        SolidBrush b1 = new SolidBrush(FillColor);//
                        g.FillEllipse(b1, start.X, start.Y, end.X, end.Y);
                    }
                    break;
                case drawTools.Line:
                    g.DrawLine(pen, start, new Point(end.X+ start.X, end.Y+start.Y));
                    break;
                case drawTools.Pen:
                    Point firstp = Points.First();
                    foreach (var point in Points)
                    {
                        g.DrawLine(pen, firstp.X, firstp.Y, point.X, point.Y);
                        firstp = point;
                    }
                    break;
                case drawTools.Rubber:
                    firstp = Points.First();
                    foreach (var point in Points)
                    {
                        g.DrawLine(pen, firstp.X, firstp.Y, point.X, point.Y);
                        firstp = point;
                    }
                    break;
            }
        }




    }
}
