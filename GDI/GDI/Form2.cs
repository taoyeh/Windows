using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDI
{
    public partial class Form2 : Form
    {
        Point start;
        //主画布
        Graphics gPic = null;
        //次画布
        Graphics gImage = null;
        Bitmap bmp = null;
        Pen pen = new Pen(Color.Red, 2);
        bool isDrawing = false;
        int w, h, x, y;
        List<Rectangle> rects = new List<Rectangle>();
        public Form2()
        {
            InitializeComponent();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            start = new Point(e.X, e.Y);
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gImage = Graphics.FromImage(bmp);
            gPic = pictureBox1.CreateGraphics();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing == false) return;
            w = e.X - start.X;
            h = e.Y - start.Y;
            x = start.X; y = start.Y;
            Positive(ref x, ref y, ref w, ref h);
            gImage.Clear(pictureBox1.BackColor);
            DrawmyRectangle(gImage,pen, x,y, w, h);
            foreach(Rectangle rectangle in rects)
            {
                gImage.DrawRectangle(pen, rectangle);
            }
            gPic.DrawImage(bmp,new Point(0,0));
        }

        //最小化程序之后也可以显示
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if(bmp!=null)
            {
                e.Graphics.DrawImage(bmp,new Point(0,0));
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
            rects.Add(new Rectangle(new Point(x, y), new Size(w, h)));
            gPic.Dispose();
            gImage.Dispose();
            gPic = null;
        }
        //矩形端点重绘
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
        //画矩形
        private void DrawmyRectangle(Graphics g, Pen pen, int x, int y, int w, int h)
        {
            Positive(ref x, ref y, ref w, ref h);
            g.DrawRectangle(pen, x, y, w, h);
        }
    }
}
