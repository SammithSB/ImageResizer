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
            TableLayoutPanel panel = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 4, // Adjusted number of rows for better distribution
                Dock = DockStyle.Fill,
                AutoSize = true
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); // Smaller button row
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); // Smaller input row
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); // Smaller button row
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Larger image view row

            btnOpenImage = new Button
            {
                Text = "Open Image",
                Dock = DockStyle.Fill
            };
            btnOpenImage.Click += BtnOpenImage_Click;

            txtWidth = new TextBox
            {
                PlaceholderText = "Width",
                Dock = DockStyle.Fill
            };

            txtHeight = new TextBox
            {
                PlaceholderText = "Height",
                Dock = DockStyle.Fill
            };

            btnResizeAndSave = new Button
            {
                Text = "Resize and Save",
                Dock = DockStyle.Fill
            };
            btnResizeAndSave.Click += BtnResizeAndSave_Click;

            pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.Fixed3D,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            // Adding controls to the TableLayoutPanel
            panel.Controls.Add(btnOpenImage, 0, 0);
            panel.SetColumnSpan(btnOpenImage, 2); // Span the button across two columns
            panel.Controls.Add(txtWidth, 0, 1);
            panel.Controls.Add(txtHeight, 1, 1);
            panel.Controls.Add(btnResizeAndSave, 0, 2);
            panel.SetColumnSpan(btnResizeAndSave, 2); // Span the button across two columns
            panel.Controls.Add(pictureBox, 0, 3);
            panel.SetRowSpan(pictureBox, 1); // Span the PictureBox across one large row

            this.Controls.Add(panel);
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
