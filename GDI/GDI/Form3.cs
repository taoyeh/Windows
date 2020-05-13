using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDI
{
    public partial class Form3 : Form
    {
        Point start;
        //主画布
        Graphics gPic = null;
        //次画布
        Graphics gImage = null;
        Bitmap bmp = null;
        Pen pen = new Pen(System.Drawing.Color.Red, 2);
        bool isDrawing = false;
        int w, h, x, y,newx,newy;
        List<Shape> shapes = new List<Shape>();
        List<Point> points = new List<Point>();
        Shape MyShape = null;
        //枚举类型，各种绘图工具
        public  enum drawTools
        {
             Pen=0,Line, Ellipse, Rectangle, Rubber, None
        };
        //打开文件获得的类型
        public Image theImage;

        public  drawTools DrawTool = drawTools.None;

        public Form3()
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
            switch(DrawTool)
            {
                case drawTools.Rectangle:
                    gImage.Clear(pictureBox1.BackColor);
                    PositiveRectangle(ref x, ref y, ref w, ref h);
                    gImage.DrawRectangle(pen, x, y, w, h);
                    break;
                case drawTools.Ellipse:
                    gImage.Clear(pictureBox1.BackColor);
                    PositiveRectangle(ref x, ref y, ref w, ref h);
                    gImage.DrawEllipse(pen, x, y, w, h);
                    break;
                case drawTools.Line:
                    gImage.Clear(pictureBox1.BackColor);
                    gImage.DrawLine(pen,start,new Point(e.X, e.Y) );
                    break;
                case drawTools.Pen:
                    Point point = new Point(e.X, e.Y);
                    points.Add(point);
                    gImage.DrawLine(pen, start.X, start.Y, e.X, e.Y);
                    start.X = e.X;
                    start.Y = e.Y;
                    break;
                case drawTools.Rubber:
                    point = new Point(e.X, e.Y);
                    points.Add(point);
                    gImage.DrawLine(new Pen(pictureBox1.BackColor,pen.Width+5), start.X, start.Y, e.X, e.Y);
                    start.X = e.X;
                    start.Y = e.Y;
                    break;
            }
            foreach (Shape shape in shapes)
            {
                shape.Show(gImage);
            }
            gPic.DrawImage(bmp, new Point(0, 0));

            
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
            switch (DrawTool)
            {
                case drawTools.Rubber:
                    shapes.Add(new Shape(new Pen(pictureBox1.BackColor, 5), points, DrawTool));

                    break;
                case drawTools.Pen:
                    shapes.Add(new Shape(pen, points, DrawTool));
                    break;
                default:
                    shapes.Add(new Shape(pen, x, y, w, h, DrawTool, false, pen.Color));
                    break;
            }
            foreach (Shape shape in shapes)
            {
                shape.Show(gImage);
            }

            // 复制粘贴等操作先获取对象
            foreach (Shape shape in shapes)
            {
                if (shape.start.X <= e.X && shape.start.Y <= e.Y && shape.end.X >= e.X - shape.start.X && shape.end.Y >= e.Y - shape.start.Y)
                {
                    MyShape = new Shape(shape);
                    newx = e.X;
                    newy = e.Y;
                    break;
                }
            }
            gPic.DrawImage(bmp, new Point(0, 0));
            gPic.Dispose();
            gImage.Dispose();
            points = new List<Point>();

        }

        //最小化程序之后也可以显示
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (bmp != null)
            {
                e.Graphics.DrawImage(bmp, new Point(0, 0));
            }
        }
      
        /// <summary>
        /// 调色板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Color_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.OK)
                return;
            pen.Color = colorDialog1.Color;
        }
        /// <summary>
        /// 选择画笔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region

        private void MyLine_Click(object sender, EventArgs e)
        {
            DrawTool = drawTools.Line ;
            RemoveAll();
            MyLine.Checked = true;
        }


        private void MyRectangle_Click(object sender, EventArgs e)
        {
            DrawTool = drawTools.Rectangle;
            RemoveAll();
            MyRectangle.Checked = true;
        }

        private void MyEllipse_Click(object sender, EventArgs e)
        {
            DrawTool = drawTools.Ellipse;
            RemoveAll();
            MyEllipse.Checked = true;
        }

        private void MyRubber_Click(object sender, EventArgs e)
        {
            DrawTool = drawTools.Rubber;
            RemoveAll();
            MyRubber.Checked = true;
            
        }
        private void MyPen_Click(object sender, EventArgs e)
        {
            DrawTool = drawTools.Pen;
            RemoveAll();
            MyPen.Checked = true;
        }
        public void RemoveAll()
        {
            MyRubber.Checked = MyEllipse.Checked = MyRectangle.Checked = MyLine.Checked= MyPen.Checked = false;
        }

        #endregion

        /// <summary>
        /// 退出画布
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// 保存画布
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "图像(*.bmp;*.wmf;*.ico;*.cur;*.jpg)|*.bmp;*.wmf;*.ico;*.cur;*.jpg";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bmp.Save(saveFileDialog1.FileName, ImageFormat.Bmp);
            }
        }

        private void MyFill_Click(object sender, EventArgs e)
        {
            gImage = pictureBox1.CreateGraphics();
            SolidBrush b1 = new SolidBrush(pen.Color);
            switch (DrawTool)
            {                
                case drawTools.Rectangle:                  
                    gImage.FillRectangle(b1, x, y, w, h);
                    shapes.Add(new Shape(pen, x, y, w, h, DrawTool,true, pen.Color));
                    break;
                case drawTools.Ellipse:
                    gImage.FillEllipse(b1, x, y, w, h);           
                    shapes.Add(new Shape(pen, x, y, w, h, DrawTool, true, pen.Color));
                    break;
            }
            
        }
        //线宽调整
        private void 工具TToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string line = e.ClickedItem.Text;
            switch (line)
            {
                case "2.5": pen.Width=2.5F; break;
                case "3": pen.Width = 3; break;
                case "3.5": pen.Width = 3.5F; break;
                case "4": pen.Width = 4; break;
                case "5": pen.Width = 5; break;
                case "6": pen.Width = 6; break;
            }
        }

        private void 撤销_Click(object sender, EventArgs e)
        {

            if(shapes.Count>0)
            {
                shapes.RemoveAt(shapes.Count - 1);
                gPic = pictureBox1.CreateGraphics();
                bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                gImage = Graphics.FromImage(bmp);
                foreach (Shape shape in shapes)
                {
                    shape.Show(gImage);
                }
                gPic.DrawImage(bmp, new Point(0, 0));
                gPic.Dispose();
                gImage.Dispose();
                Refresh();
            }
        }

        private void 复制CToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 粘贴PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gImage = Graphics.FromImage(bmp);
            gPic = pictureBox1.CreateGraphics();
            shapes.Add(new Shape(pen, newx, newy, MyShape.end.X, MyShape.end.Y, DrawTool, false, Color.BackColor));
            foreach (Shape shape in shapes)
            {
                shape.Show(gImage);
            }
            gPic.DrawImage(bmp, new Point(0, 0));
            gPic.Dispose();
            gImage.Dispose();
        }

        private void 剪切TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gImage = Graphics.FromImage(bmp);
            gPic = pictureBox1.CreateGraphics();
            gImage.Clear(pictureBox1.BackColor);
            shapes.RemoveAt(shapes.Count()-1);
            foreach (Shape shape in shapes)
            {
                shape.Show(gImage);
            }
            gPic.DrawImage(bmp, new Point(0, 0));
            gPic.Dispose();
            gImage.Dispose();
        }




        /// <summary>
        /// 新建画布
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 新建NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gPic = pictureBox1.CreateGraphics();
            gPic.Clear(pictureBox1.BackColor);
            shapes = new List<Shape>();
            Text = "新建画布";
        }
        /// <summary>
        /// 通过图片打开画布
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files(*.bmp;*.wmf;*.ico;*.cur;*.jpg)|*.bmp;*.wmf;*.ico;*.cur;*.jpg";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                theImage = Image.FromFile(openFileDialog1.FileName);
                gPic = pictureBox1.CreateGraphics();
                gPic.DrawImage(theImage, new Point(0, 0));
                gImage = Graphics.FromImage(theImage);
            }
        }

        //矩形端点重绘
        private static void PositiveRectangle(ref int x, ref int y, ref int w, ref int h)
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
