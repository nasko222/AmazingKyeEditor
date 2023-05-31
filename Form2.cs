using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AmazingKyeEditor
{
    public partial class Form2 : Form
    {
        public static bool ActiveSelf;
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Form1.instance.MyReadKye(rawTextBox.Text.Split('\n'));
            }
            catch
            {
                return;
            }
            ActiveSelf = false;
            this.Close();
            
        }

        private void Form2_Close(object sender, EventArgs e)
        {
            CloseMe();
        }

        public void CloseMe()
        {
            ActiveSelf = false;
            Form1.instance.ReactivateMain();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try 
            {
                Form1.instance.MyReadKye(rawTextBox.Text.Split('\n'));
                Form1.instance.MySaveFile(true);
            }
            catch
            {
                return;
            }
            rawTextBox.Clear();
        }
    }
}
