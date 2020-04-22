# 前言
> c# 正编译生成debug,exe等文件，同样也可以通过exe[反编译](https://blog.csdn.net/kongwei521/article/details/54927689)生成相对应的文件，一劳永逸，任君选择(｀・ω・´) （~~我们老师为了给我们看要求效果给了我们一个半成品exe，cv工程师上线了，真为所欲为~~）
# 问题描述
### 验证简易相册程序的实现（不需要写入报告）
### 设计一个类似Windows写字板的程序，使其具有以下功能：
- 提供基本的写字板功能，如文件的打开，保存，支持txt、rtf；
-  实现多格式的文本的编辑、修饰等，均有相应的菜单来控制，常用的菜单项配备相应的工具栏按钮（可参照Windows自带的写字板）
按需添加其他的代码，如窗口的布局等；
- 实现自定义的查找对话框，有能力的同学可进一步实现替换/全文替换等功能
- 状态栏中显示学生学号、姓名等信息，在窗口的标题中显示当前正在编辑的文件名
- （可选）其他你觉得实用的功能：如支持word等
- （可选）将其改造成MDI程序：父窗口中包含新建、窗口、帮助等菜单；在运行时，子窗口的菜单会与其合并，合理设置各菜单项的MergeIndex/MergeAction属性，使其符合我们的使用习惯；编程允许用户调整各自窗口间的布局关系（平铺、层叠等）等
先看效果
![image.png](https://i.loli.net/2020/04/22/OVzHlgextuRaCfD.png)
# 解决方法 
### 用richtextbox是一个类似写字板的一个控件，能满足rtf，txt等格式，如果要满足doc文档，去百度Microsoft.Office.Interop.Word.dll，下载并引用，打开保存代码如下（其中saveName为文件名）
```

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
        public void 父窗体打开Open(WritingBoard writingBoard)
        {
            writingBoard.openFileDialog1.InitialDirectory = "C:\\";
            writingBoard.openFileDialog1.Filter = "rtf 文件|*.rtf|txt 文件|*.txt|doc files|*.doc";
            if (writingBoard.openFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            saveName = writingBoard.openFileDialog1.FileName;
            if (writingBoard.openFileDialog1.FilterIndex == 1)
                writingBoard.richTextBox1.LoadFile(saveName);
            else if (writingBoard.openFileDialog1.FilterIndex == 2)
                writingBoard.richTextBox1.LoadFile(saveName, RichTextBoxStreamType.PlainText);
            else
                OpenWord(saveName);
            writingBoard.Text = saveName;
            return;
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
```

### 其他一系列doc加粗，倾斜，下划线，左中右对齐，字体型号颜色，查找（大小写，全字，正逆），其实本质上来说都是对richtextbox的一些属性的修改，注意多用DropDownOpening，DropDownItemClicked等操作代替Click,使代码更加精简高效
&emsp;&emsp; ![image.png](https://i.loli.net/2020/04/22/kYU73eRCyHDGzol.png)

&emsp;&emsp;这是一个MDI调整子窗体使用DropDownItemClicked的例子，高效优雅 = v =
```
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
```

### MDI的使用，是为了让一个父窗体拥有许多个子窗体，进行更加简单有效的操作
![image.png](https://i.loli.net/2020/04/22/fYAncDsQNLPpgMz.png)
在父窗体new子窗体就行，记得一系列基本操作，你也可以通过生成函数和属性进行快速高效设置
```
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
```

### 源码附上 
 &emsp;&emsp;[传送门](https://github.com/taoyeh/Windows/tree/master)，我设置了分支，这样我就可以把不同内容相同属性的代码放在一起了，同时也可以拥有不同的md可以阅读，对于git 的整理也是不错的(｀・ω・´)
 ![image.png](https://i.loli.net/2020/04/22/bUwZKoH8m7rQ5xn.png)
