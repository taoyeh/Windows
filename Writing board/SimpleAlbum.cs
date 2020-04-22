using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Writing_board
{
    public partial class SimpleAlbum : Form
    {

        List<string> paths = new List<string>();
        int curIndex = 0;
        public SimpleAlbum()
        {
            InitializeComponent();
        }

        private void 新增ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            dlg.Filter = "Image files|*.bmp;*.jpg;*jpeg";
            if(dlg.ShowDialog()==DialogResult.OK)
            {
                foreach (string path in dlg.FileNames)
                    paths.Add(path);
                curIndex = paths.Count - 1;
                UpdateControls();
            }
        }

        /// <summary>
        /// 图片前一张后一张操作
        /// </summary>
        private void UpdateControls()
        {
            pictureBox1.Load(paths[curIndex]);
            toolStripStatusLabel1.Text = paths[curIndex];
            toolStripStatusLabel2.Text = string.Format("{0}/{1}", curIndex + 1, paths.Count);
            tsmlNext.Enabled = tsbNext.Enabled = curIndex < paths.Count - 1;
            tsmlPrev.Enabled = tsbPre.Enabled = curIndex>0;
        }    

        private void tsmlPrev_Click(object sender, EventArgs e)
        {
            curIndex--;
            if (curIndex < 0) curIndex = 0;
            UpdateControls();
        }

        private void tsmlNext_Click(object sender, EventArgs e)
        {
            curIndex++;
            if (curIndex >=paths.Count) curIndex = paths.Count-1;
            UpdateControls();
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            curIndex--;
            if (curIndex < 0) curIndex = 0;
            UpdateControls();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            curIndex++;
            if (curIndex >= paths.Count) curIndex = paths.Count - 1; 
            UpdateControls();
        }
        /// <summary>
        /// 图片缩放模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void 缩放模式ToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            PictureBoxSizeMode mode = this.pictureBox1.SizeMode;
            switch(mode)
            {
                case PictureBoxSizeMode.Normal:
                    zoomNormal.Checked = true;
                    zoomProportion.Checked = false;
                    zoomStretching.Checked = false;
                    break;
                case PictureBoxSizeMode.StretchImage:
                    zoomNormal.Checked = false;
                    zoomProportion.Checked = false;
                    zoomStretching.Checked = true;
                    break;
                case PictureBoxSizeMode.Zoom:
                    zoomNormal.Checked = false;
                    zoomProportion.Checked = true;
                    zoomStretching.Checked = false;
                    break;
            }
        }

        private void 缩放模式ToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string item = e.ClickedItem.Text;
            switch(item)
            {
                case "原始大小": pictureBox1.SizeMode = PictureBoxSizeMode.Normal;break;
                case "拉伸": pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; break;
                case "按比例缩放": pictureBox1.SizeMode = PictureBoxSizeMode.Zoom; break;
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            paths.RemoveAt(curIndex);
            if (curIndex > paths.Count - 1) curIndex = paths.Count - 1;
            UpdateControls();
        }

        private void host_Click(object sender, EventArgs e)
        {
            ToolStripControlHost host = sender as ToolStripControlHost;
            if (host == null) return;
            curIndex =(int)( host.Tag);
            UpdateControls();
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            ToolStripDropDown drop = new ToolStripDropDown();
            toolStripDropDownButton1.DropDown = drop;
            for(int i=0;i<paths.Count;i++)
            {
                PictureBox image = new PictureBox();
                image.SizeMode = PictureBoxSizeMode.Zoom;
                image.Image = Image.FromFile(paths[i]);
                ToolStripControlHost host = new ToolStripControlHost(image);
                host.AutoSize = false;
                host.Size = new Size(toolStripDropDownButton1.Width, 100);
                host.Tag = i;
                host.Click += host_Click;
                drop.Items.Add(host);
            }
        }
    }
}
