using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImageResizer
{
    public partial class Form1 : Form
    {
        private Button? btnOpenImage;
        private Button? btnResizeAndSave;
        private TextBox? txtWidth;
        private TextBox? txtHeight;
        private PictureBox? pictureBox;
        private Image? originalImage;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            btnOpenImage = new Button
            {
                Text = "Open Image",
                Location = new Point(10, 10)
            };
            btnOpenImage.Click += BtnOpenImage_Click;

            txtWidth = new TextBox
            {
                PlaceholderText = "Width",
                Location = new Point(10, 50)
            };

            txtHeight = new TextBox
            {
                PlaceholderText = "Height",
                Location = new Point(150, 50)
            };

            btnResizeAndSave = new Button
            {
                Text = "Resize and Save",
                Location = new Point(10, 90)
            };
            btnResizeAndSave.Click += BtnResizeAndSave_Click;

            pictureBox = new PictureBox
            {
                Location = new Point(10, 130),
                Size = new Size(360, 200),
                BorderStyle = BorderStyle.Fixed3D
            };

            this.Controls.AddRange(new Control[] { btnOpenImage, txtWidth, txtHeight, btnResizeAndSave, pictureBox });
        }

        private void BtnOpenImage_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if(pictureBox!=null){
                    originalImage = Image.FromFile(openFileDialog.FileName);
                    pictureBox.Image = originalImage;
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                }
            }
        }

        private void BtnResizeAndSave_Click(object? sender, EventArgs e)
        {
            if (originalImage == null)
            {
                MessageBox.Show("Please open an image first.");
                return;
            }
            if (!int.TryParse(txtWidth.Text, out int width) || !int.TryParse(txtHeight.Text, out int height))
            {
                MessageBox.Show("Please enter valid numbers for width and height.");
                return;
            }

            Image resizedImage = ResizeImage(originalImage, width, height);

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JPEG Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileExtension = System.IO.Path.GetExtension(saveFileDialog.FileName).ToLower();
                    switch (fileExtension)
                    {
                        case ".jpg":
                            resizedImage.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;
                        case ".png":
                            resizedImage.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                            break;
                        case ".bmp":
                            resizedImage.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                    }

                    MessageBox.Show("Image saved successfully!");
                }
            }
        }

        private Image ResizeImage(Image image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return resizedImage;
        }
    }
}
