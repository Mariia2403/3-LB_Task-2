using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private string filePath = "";
        private string fileNameWithoutExt = "";
        private string folderPath = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                
                openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff";
                openFileDialog.Title = "Виберіть зображення";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    
                    filePath = openFileDialog.FileName;
                    fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
                    folderPath = Path.GetDirectoryName(filePath);

                    string extension = Path.GetExtension(filePath).TrimStart('.').ToLower();

                    
                    Regex regexExtForImage = new Regex(@"^((bmp)|(gif)|(tiff?)|(jpe?g)|(png))$", RegexOptions.IgnoreCase);
                    if (regexExtForImage.IsMatch(extension))
                    {
                       
                        pictureBox1.Image = Image.FromFile(filePath);
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom; 
                    }
                    else
                    {
                        MessageBox.Show("Оберіть файл з правильним розширенням (bmp, gif, tiff, jpeg, jpg, png)", "Неправильний формат", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {


            if (pictureBox1.Image != null)
            {
                Bitmap input = new Bitmap(pictureBox1.Image);
              
                input.RotateFlip(RotateFlipType.RotateNoneFlipX);

                pictureBox2.Image = input;
                string newFileName = Path.Combine(folderPath, $"{fileNameWithoutExt}-mirrored.gif");

                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                input.Save(newFileName, System.Drawing.Imaging.ImageFormat.Gif);
                MessageBox.Show("Файл збережено: " + newFileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Виберіть папку з зображеннями";

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    folderPath = folderBrowserDialog.SelectedPath;

                    
                    Regex regexExtForImage = new Regex(@"^((bmp)|(gif)|(tiff?)|(jpe?g)|(png))$", RegexOptions.IgnoreCase);

                  
                    foreach (string filePath in Directory.GetFiles(folderPath))
                    {
                        string extension = Path.GetExtension(filePath).TrimStart('.').ToLower();

                        
                        if (regexExtForImage.IsMatch(extension))
                        {
                            try
                            {
                                using (Bitmap input = new Bitmap(filePath))
                                {
                                    
                                    input.RotateFlip(RotateFlipType.RotateNoneFlipX);

                                    
                                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
                                    string newFileName = Path.Combine(folderPath, $"{fileNameWithoutExt}-mirrored.gif");

                                    input.Save(newFileName, System.Drawing.Imaging.ImageFormat.Gif);
                                }
                            }
                            catch (Exception ex)
                            {
                               // MessageBox.Show($"Помилка при обробці файлу {filePath}: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }

                    MessageBox.Show("Всі зображення в папці були перевернуті та збережені у форматі GIF з новими іменами.", "Завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }
    }
}

