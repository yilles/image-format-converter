using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HEIC_Converter
{
    public partial class Form2 : Form
    {
        public ImageFormat ImgFormat = ImageFormat.JPG;
        public Form2()
        {
            InitializeComponent();
        }

        private void radioButton1_MouseClick(object sender, MouseEventArgs e)
        {
            ImgFormat = ImageFormat.JPG;
            DialogResult = DialogResult.Cancel;
        }

        private void radioButton2_MouseClick(object sender, MouseEventArgs e)
        {
            ImgFormat = ImageFormat.PNG;
            DialogResult = DialogResult.Cancel;
        }

        private void radioButton3_MouseClick(object sender, MouseEventArgs e)
        {
            ImgFormat = ImageFormat.BMP;
            DialogResult = DialogResult.Cancel;
        }
    }
}
