using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WritingBoard
{
    public partial class WritingBoard : Form
    {
        private string saveName = "";
        public WritingBoard()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WritingBoard_Load(object sender, EventArgs e)
        {
            foreach (FontFamily family in FontFamily.Families)
                toolStripComboBox1.Items.Add(family.Name);
            for (int index = 5; index <= 70; ++index)
                toolStripComboBox2.Items.Add(index.ToString());
            toolStripComboBox1.SelectedIndex = toolStripComboBox1.FindString("Times New Roman");
            toolStripComboBox2.SelectedIndex = 5;
            toolStripButton6.Checked = richTextBox1.SelectionAlignment == HorizontalAlignment.Left;
            toolStripButton7.Checked = richTextBox1.SelectionAlignment == HorizontalAlignment.Right;
            toolStripButton5.Checked = richTextBox1.SelectionAlignment == HorizontalAlignment.Center;
        }
        /// <summary>
        /// 打开文档操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "C:\\";
            openFileDialog1.Filter = "rtf 文件|*.rtf|txt 文件|*.txt|doc files|*.doc";
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            saveName = openFileDialog1.FileName;
            if (openFileDialog1.FilterIndex == 1)
                richTextBox1.LoadFile(saveName);
            else if (openFileDialog1.FilterIndex == 2)
                richTextBox1.LoadFile(saveName, RichTextBoxStreamType.PlainText);
            else
                OpenWord(saveName);
            Text = saveName;
        }
        public void OpenWord(string fileName)
        {
            ApplicationClass applicationClass = new ApplicationClass();
            Document document = null;
            object obj = Missing.Value;
            object FileName = fileName;
            object ReadOnly = false;
            object Visible = true;
            try
            {
                document = applicationClass.Documents.Open(ref FileName, ref obj, ref ReadOnly, ref obj, ref obj, ref obj, ref obj, ref obj, ref obj, ref obj, ref obj, ref Visible, ref obj, ref obj, ref obj, ref obj);
                document.ActiveWindow.Selection.WholeStory();
                document.ActiveWindow.Selection.Copy();
                richTextBox1.Paste();
            }
            finally
            {
                document?.Close(ref obj, ref obj, ref obj);
                applicationClass?.Quit(ref obj, ref obj, ref obj);
            }
        }
        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveName != "")
            {
                richTextBox1.SaveFile(saveName, RichTextBoxStreamType.RichText);
            }
            else
            {
                saveFileDialog1.Filter = "rtf files|*.rtf";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    saveName = saveFileDialog1.FileName;
                    richTextBox1.SaveFile(saveName, RichTextBoxStreamType.RichText);
                }
            }
        }
        /// <summary>
        /// 另存为操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 另存为AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = saveName.Substring(saveName.LastIndexOf('\\') + 1);
            saveFileDialog1.Filter = "rtf files|*.rtf|txt files|*.txt|doc files|*.doc";
            saveFileDialog1.AddExtension = true;
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            saveName = saveFileDialog1.FileName;
            if (saveFileDialog1.FilterIndex == 1)
                richTextBox1.SaveFile(saveName, RichTextBoxStreamType.RichText);
            else if (saveFileDialog1.FilterIndex == 2)
                richTextBox1.SaveFile(saveName, RichTextBoxStreamType.PlainText);
            else
                SaveAsWord(saveName);
        }
        public void SaveAsWord(string fileName)
        {
            ApplicationClass applicationClass = new ApplicationClass();
            Document document = null;
            object obj = Missing.Value;
            object FileName = fileName;
            try
            {
                document = applicationClass.Documents.Add(ref obj, ref obj, ref obj, ref obj);
                document.ActiveWindow.Selection.WholeStory();
                richTextBox1.SelectAll();
                Clipboard.SetData(DataFormats.Rtf, richTextBox1.SelectedRtf);
                document.ActiveWindow.Selection.Paste();
                document.SaveAs(ref FileName, ref obj, ref obj, ref obj, ref obj, ref obj, ref obj, ref obj, ref obj, ref obj, ref obj, ref obj, ref obj, ref obj, ref obj, ref obj);
            }
            finally
            {
                document?.Close(ref obj, ref obj, ref obj);
                applicationClass?.Quit(ref obj, ref obj, ref obj);
            }
        }
        /// <summary>
        /// 新建操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 新建NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            saveName = "";
        }
        /// <summary>
        /// 字体左中右
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            toolStripButton6.Checked = richTextBox1.SelectionAlignment == HorizontalAlignment.Left;
            toolStripButton7.Checked = richTextBox1.SelectionAlignment == HorizontalAlignment.Right;
            toolStripButton5.Checked = richTextBox1.SelectionAlignment == HorizontalAlignment.Center;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
            toolStripButton6.Checked = richTextBox1.SelectionAlignment == HorizontalAlignment.Left;
            toolStripButton7.Checked = richTextBox1.SelectionAlignment == HorizontalAlignment.Right;
            toolStripButton5.Checked = richTextBox1.SelectionAlignment == HorizontalAlignment.Center;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
            toolStripButton6.Checked = richTextBox1.SelectionAlignment == HorizontalAlignment.Left;
            toolStripButton7.Checked = richTextBox1.SelectionAlignment == HorizontalAlignment.Right;
            toolStripButton5.Checked = richTextBox1.SelectionAlignment == HorizontalAlignment.Center;
        }
        /// <summary>
        /// 字体颜色调色板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.OK)
                return;
            richTextBox1.SelectionColor = colorDialog1.Color;
        }
        /// <summary>
        /// 字体种类和大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionFont = new System.Drawing.Font(toolStripComboBox1.SelectedItem.ToString(), richTextBox1.SelectionFont.Size);
        }

        private void toolStripComboBox2_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionFont = new System.Drawing.Font(richTextBox1.SelectionFont.Name, float.Parse(toolStripComboBox2.SelectedItem.ToString()));
        }
        /// <summary>
        /// 字体加粗，倾斜，下划线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!toolStripButton1.Checked)
            {
                richTextBox1.SelectionFont = new System.Drawing.Font(richTextBox1.SelectionFont, richTextBox1.SelectionFont.Style | FontStyle.Bold);
                boldBToolStripMenuItem.Checked = toolStripButton1.Checked = !toolStripButton1.Checked;
                toolStripStatusLabel1.Enabled = true;
            }
            else
            {
                richTextBox1.SelectionFont = new System.Drawing.Font(richTextBox1.SelectionFont, richTextBox1.SelectionFont.Style & ~FontStyle.Bold);
                boldBToolStripMenuItem.Checked = toolStripButton1.Checked = !toolStripButton1.Checked;
                toolStripStatusLabel1.Enabled = false;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (!toolStripButton2.Checked)
            {
                richTextBox1.SelectionFont = new System.Drawing.Font(richTextBox1.SelectionFont, richTextBox1.SelectionFont.Style | FontStyle.Italic);
                italicIToolStripMenuItem.Checked = toolStripButton2.Checked = !toolStripButton2.Checked;
                toolStripStatusLabel2.Enabled = true;
            }
            else
            {
                richTextBox1.SelectionFont = new System.Drawing.Font(richTextBox1.SelectionFont, richTextBox1.SelectionFont.Style & ~FontStyle.Italic);
                italicIToolStripMenuItem.Checked = toolStripButton2.Checked = !toolStripButton2.Checked;
                toolStripStatusLabel2.Enabled = false;
            }

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (!toolStripButton3.Checked)
            {
                richTextBox1.SelectionFont = new System.Drawing.Font(richTextBox1.SelectionFont, richTextBox1.SelectionFont.Style | FontStyle.Underline);
                underlineUToolStripMenuItem.Checked = toolStripButton3.Checked = !toolStripButton3.Checked;
                toolStripStatusLabel3.Enabled = true;
            }
            else
            {
                richTextBox1.SelectionFont = new System.Drawing.Font(richTextBox1.SelectionFont, richTextBox1.SelectionFont.Style & ~FontStyle.Underline);
                underlineUToolStripMenuItem.Checked = toolStripButton3.Checked = !toolStripButton3.Checked;
                toolStripStatusLabel3.Enabled = false;
            }

        }


        /// <summary>
        /// 撤销，重复，剪切，复制，粘贴，全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 撤消UToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        private void 重复RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
        }

        private void 剪切TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void 复制CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void 粘贴PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void 全选AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        /// <summary>
        /// 选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 选项OToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            标准ToolStripMenuItem.Checked = toolStrip1.Visible;
            字体ToolStripMenuItem.Checked = toolStrip2.Visible;
        }

        private void 标准ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            标准ToolStripMenuItem.Checked = !标准ToolStripMenuItem.Checked;
            if (标准ToolStripMenuItem.CheckState == CheckState.Checked)
                toolStrip1.Show();
            else
                toolStrip1.Hide();
        }

        private void 字体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            字体ToolStripMenuItem.Checked = !字体ToolStripMenuItem.Checked;
            if (字体ToolStripMenuItem.CheckState == CheckState.Checked)
                toolStrip2.Show();
            else
                toolStrip2.Hide();
        }

        /// <summary>
        /// 搜索功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 搜索SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindDialog findDialog = new FindDialog(this);
            findDialog.Show();
        }

        public int FindString(string strToFind, int start, RichTextBoxFinds opt)
        {
            int index = richTextBox1.Find(strToFind, start, opt);
            return index;
        }

        public int  TextLength
        {
            get
            {
                return richTextBox1.Text.Length;
            }
        }
    }
}
