using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Image_Format_Converter
{
    public partial class Form3 : Form
    {
        public ResizeImageType ResizeImageType = ResizeImageType.BaseHeight;
        public bool ResizeEnabled = false;
        public bool IgnoreAspectRatio = true;
        public int ResizeImageHeight = 100;
        public int ResizeImageWidth = 100;
        public Form3()
        {
            InitializeComponent();
            this.numericUpDown1.Value = ResizeImageHeight;
            this.numericUpDown2.Value = ResizeImageWidth;
            numericUpDown1.Enabled = ResizeEnabled;
            numericUpDown2.Enabled = ResizeEnabled;
            radioButton1.Visible = !IgnoreAspectRatio;
            radioButton2.Visible = !IgnoreAspectRatio;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            ResizeImageHeight = Convert.ToInt32(numericUpDown1.Value);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            ResizeImageWidth = Convert.ToInt32(numericUpDown2.Value);
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            ResizeImageType = ResizeImageType.BaseHeight;
            UpdateUI();
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            ResizeImageType = ResizeImageType.BaseWidth;
            UpdateUI();
        }

        private void UpdateUI()
        {
            ResizeEnabled = checkBox1.Checked;
            IgnoreAspectRatio = !checkBox2.Checked;
            numericUpDown1.Enabled = (ResizeEnabled && IgnoreAspectRatio) || ((ResizeEnabled && !IgnoreAspectRatio) && radioButton1.Checked);
            numericUpDown2.Enabled = (ResizeEnabled && IgnoreAspectRatio) || ((ResizeEnabled && !IgnoreAspectRatio) && radioButton2.Checked);
            radioButton1.Visible = ResizeEnabled && !IgnoreAspectRatio;
            radioButton2.Visible = ResizeEnabled && !IgnoreAspectRatio;
        }
    }
}
