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
    public partial class FindDialog : Form
    {
        private string findString;
        private bool wholeWord;
        private bool matchCase;
        int start = 0;
        WritingBoard main;
        public FindDialog(WritingBoard writingBoard)
        {
            InitializeComponent();
            main = writingBoard;
        }
        public string FindString
        {
            get
            {
                return findString;
            }
        }

        public bool WholeWord
        {
            get
            {
                return wholeWord;
            }
        }

        public bool MatchCase
        {
            get
            {
                return matchCase;
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            wholeWord = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            matchCase = checkBox2.Checked;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            RichTextBoxFinds opt = new RichTextBoxFinds();
            if (MatchCase) opt |= RichTextBoxFinds.MatchCase;
            if (WholeWord) opt |= RichTextBoxFinds.WholeWord;
            start = main.FindString(findString, start, opt);
            if (!checkBox3.Checked) start++;
            else
            {
                start--;
                if(start==-1) start = main.TextLength - 1;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            findString = textBox1.Text;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true) start = main.TextLength - 1;
            else start = 0;
        }
    }
}
