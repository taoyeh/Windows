using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GDI
{
    public partial class Form1 : Form
    {
        Point start ;
        Graphics g = null;
        Pen pen = new Pen(Color.Red, 2);
        Pen backpen;
        bool isDrawing = false;
        int w, h,x,y;
        List<Rectangle> rects = new List<Rectangle>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            start = new Point(e.X, e.Y);
            g = CreateGraphics();
            isDrawing = true;
            backpen = new Pen(BackColor,2);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing == false) return;
            int rw = e.X - start.X;
            int rh = e.Y - start.Y;
            DrawmyRectangle(g,backpen, start.X, start.Y, w, h);
            DrawmyRectangle(g,pen, start.X, start.Y, rw, rh);
            w = rw;h = rh;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
            x = start.X; y = start.Y;
            Positive(ref x, ref y, ref w, ref h);
            rects.Add(new Rectangle(new Point(x,y), new Size(w, h)));
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            if (rects != null && rects.Count > 0)
                e.Graphics.DrawRectangles(pen, rects.ToArray());
        }

        private void DrawmyRectangle(Graphics g,Pen pen,int x,int y,int w,int h)
        {
            Positive(ref x, ref y, ref w, ref h);
            g.DrawRectangle(pen, x, y, w, h);
        }

        private static void Positive(ref int x, ref int y, ref int w, ref int h)
        {
            if (w < 0)
            {
                x = x + w; w = -w;
            }
            if (h < 0)
            {
                y = y + h; h = -h;
            }
        }
    }
}
