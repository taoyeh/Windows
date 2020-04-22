using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WritingBoard
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WritingBoard writingBoard = new WritingBoard();
            writingBoard.TopLevel = false;
            writingBoard.MdiParent = this;
            writingBoard.Show();
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WritingBoard writingBoard = new WritingBoard();
            writingBoard.TopLevel = false;
            writingBoard.MdiParent = this;
            writingBoard.父窗体打开Open(writingBoard);
            writingBoard.Show();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (WritingBoard writingBoard in MdiChildren) writingBoard.Close();
            Application.Exit();
        }

        private void 窗口WToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string layout=e.ClickedItem.Text;
            switch(layout)
            {
                case "水平平铺": LayoutMdi(MdiLayout.TileHorizontal);break;
                case "垂直平铺": LayoutMdi(MdiLayout.TileVertical); break;
                case "层叠": LayoutMdi(MdiLayout.Cascade); break;
            }
        }
    }
}
