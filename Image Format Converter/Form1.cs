using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using ImageMagick;

namespace Image_Format_Converter
{
    public enum ResizeImageType
    {
        None,
        BaseHeight,
        BaseWidth
    }

    public enum ImageFormat
    {
        None,
        JPG,
        PNG,
        BMP
    }

    public partial class Form1 : Form
    {
        Form2 fm2 = new Form2();
        Form3 fm3 = new Form3();
        ImageFormat ImgFormat = ImageFormat.None;
        string InputDir = "";
        string OutputDir = "";
        string ShowImagePath = "";
        int ProcessValue = -1;
        string Version = "1.1.2";
        public Form1()
        {
            InitializeComponent();
            this.Text += Version;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var _openFileDialog = new OpenFileDialog())
            {
                _openFileDialog.Title = "選取圖片(多選)";
                _openFileDialog.Filter = "Image files (*.JPG/*JPEG/*PNG/*BMP/*HEIC)|*JPG;*JPEG;*PNG;*BMP;*.HEIC";
                _openFileDialog.Multiselect = true;
                if (_openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    listBox1.Items.Clear();
                    foreach (var _file in _openFileDialog.FileNames)
                    {
                        InputDir = Path.GetDirectoryName(_file);
                        var _onlyFileName = Path.GetFileName(_file);
                        listBox1.Items.Add(_onlyFileName);
                    }
                    if (listBox1.Items.Count > 0)
                        listBox1.SelectedIndex = 0;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (fm2.ShowDialog() == DialogResult.Cancel)
            {
                switch (fm2.ImgFormat)
                {
                    case ImageFormat.None:
                        ImgFormat = ImageFormat.None;
                        break;
                    case ImageFormat.JPG:
                        ImgFormat = ImageFormat.JPG;
                        break;
                    case ImageFormat.PNG:
                        ImgFormat = ImageFormat.PNG;
                        break;
                    case ImageFormat.BMP:
                        ImgFormat = ImageFormat.BMP;
                        break;
                    default:
                        break;
                }
            }
            button5.Text = ImgFormat.ToString();
            button5.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (CommonOpenFileDialog _commonOpenFileDialog = new CommonOpenFileDialog())
            {
                _commonOpenFileDialog.Title = "選取輸出資料夾";
                _commonOpenFileDialog.IsFolderPicker = true;
                if (_commonOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    OutputDir = _commonOpenFileDialog.FileName;
                    string _onlyTheLastDirectoryName = Path.GetFileName(OutputDir);
                    if (_onlyTheLastDirectoryName == string.Empty)
                        _onlyTheLastDirectoryName = Path.GetPathRoot(OutputDir);
                    button6.Text = _onlyTheLastDirectoryName;
                    button6.Visible = true;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button6.Visible)
            {
                string _dir = OutputDir + Path.DirectorySeparatorChar + "converted";
                if (!Directory.Exists(_dir))
                {
                    try
                    {
                        Directory.CreateDirectory(_dir);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                System.Diagnostics.Process.Start(_dir);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string _resizeInfo = "";
            string _height = "";
            string _width = "";
            if (fm3.ShowDialog() == DialogResult.Cancel)
            {
                _height = string.Format("{0}", fm3.ResizeImageHeight);
                _width = string.Format("{0}", fm3.ResizeImageWidth);

                if (fm3.ResizeEnabled)
                {
                    if (!fm3.IgnoreAspectRatio)
                    {
                        switch (fm3.ResizeImageType)
                        {
                            case ResizeImageType.None:
                                break;
                            case ResizeImageType.BaseHeight:
                                _resizeInfo += "啟用" + Environment.NewLine + 'H' + _height;
                                break;
                            case ResizeImageType.BaseWidth:
                                _resizeInfo += "啟用" + Environment.NewLine + "W" +_width;
                                break;
                            default:
                                break;
                        }

                    }
                    else
                    {
                        _resizeInfo += "啟用" + Environment.NewLine + 'H' + _height + 'x' + "W" +_width;
                    }
                }
                else
                {
                    _resizeInfo += "未啟用";
                }

                button9.Text = _resizeInfo;
                button9.Visible = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = listBox1.Items.Count;
            progressBar1.Minimum = 0;
            ProcessValue = 0;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (var item in listBox1.Items)
            {
                string _selectedHEIC = item.ToString();
                if (_selectedHEIC != "")
                {
                    ProcessValue++;
                    string _inputImagePath = InputDir + Path.DirectorySeparatorChar + _selectedHEIC;
                    string _outputImagePath = OutputDir + Path.DirectorySeparatorChar + "converted" + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(_selectedHEIC);
                    using (var _img = new MagickImage(_inputImagePath))
                    {
                        // Sets the output format to jpg/png/bmp.
                        switch (ImgFormat)
                        {
                            case ImageFormat.None:
                                _img.Format = MagickFormat.Jpg;
                                _outputImagePath += ".jpg";
                                break;
                            case ImageFormat.JPG:
                                _img.Format = MagickFormat.Jpg;
                                _outputImagePath += ".jpg";
                                break;
                            case ImageFormat.PNG:
                                _img.Format = MagickFormat.Png;
                                _outputImagePath += ".png";
                                break;
                            case ImageFormat.BMP:
                                _img.Format = MagickFormat.Bmp;
                                _outputImagePath += ".bmp";
                                break;
                            default:
                                _img.Format = MagickFormat.Jpg;
                                _outputImagePath += ".jpg";
                                break;
                        }
                        //Create the directory with save converted file
                        string _dir = OutputDir + Path.DirectorySeparatorChar + "converted";
                        if (!Directory.Exists(_dir))
                        {
                            try
                            {
                                Directory.CreateDirectory(_dir);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }

                        // This will resize the image to a fixed size without maintaining the aspect ratio.
                        // Normally an image will be resized to fit inside the specified size.
                        if (fm3.ResizeEnabled)
                        {
                            MagickGeometry _size;
                            if (!fm3.IgnoreAspectRatio)
                            {
                                switch (fm3.ResizeImageType)
                                {
                                    case ResizeImageType.None:
                                        break;
                                    case ResizeImageType.BaseHeight:
                                        _size = new MagickGeometry(0, fm3.ResizeImageHeight);
                                        _size.IgnoreAspectRatio = false;
                                        _img.Resize(_size);
                                        break;
                                    case ResizeImageType.BaseWidth:
                                        _size = new MagickGeometry(fm3.ResizeImageWidth, 0);
                                        _size.IgnoreAspectRatio = false;
                                        _img.Resize(_size);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                _size = new MagickGeometry(fm3.ResizeImageWidth, fm3.ResizeImageHeight);
                                _size.IgnoreAspectRatio = true;
                                _img.Resize(_size);
                            }
                        }

                        // Write the image to the file
                        _img.Write(_outputImagePath);
                        //using (var _fs = new FileStream(_outputImagePath, FileMode.Create))
                        //{
                        //    _img.Write(_fs);
                        //}
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ProcessValue != -1)
                progressBar1.Value = ProcessValue;

            if (backgroundWorker1.IsBusy)
            {
                label1.Text = string.Format("處理進度: {0}/{1}", ProcessValue, progressBar1.Maximum);
            }
            else
            {
                if (ProcessValue != -1)
                    label1.Text = string.Format("處理進度: {0}/{1}", ProcessValue, progressBar1.Maximum);
                else
                    label1.Text = "";
            }

            button1.Enabled = !backgroundWorker1.IsBusy;
            button2.Enabled = !backgroundWorker1.IsBusy;
            button3.Enabled = !backgroundWorker1.IsBusy;
            button4.Enabled = !backgroundWorker1.IsBusy;
            listBox1.Enabled = !backgroundWorker1.IsBusy;
            button7.Enabled = !backgroundWorker1.IsBusy
                              && listBox1.Items.Count > 0
                              && button5.Visible
                              && button6.Visible;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowImagePath = InputDir + Path.DirectorySeparatorChar + listBox1.SelectedItem.ToString();
            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (ShowImagePath == "") { return; };
            using (var _ms = new MemoryStream())
            {
                // Write to stream
                using (var _img = new MagickImage(ShowImagePath))
                {
                    // Sets the output format to jpg
                    _img.Format = MagickFormat.Jpg;
                    // Write the image to the memorystream
                    _img.Write(_ms);
                }

                Image _showImg = Image.FromStream(_ms);
                Graphics _g = e.Graphics;
                _g.DrawImage(_showImg, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height), new Rectangle(0, 0, _showImg.Width, _showImg.Height), GraphicsUnit.Pixel);
            }
        }
    }
}
