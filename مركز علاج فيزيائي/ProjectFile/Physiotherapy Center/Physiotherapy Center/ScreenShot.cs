using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Physiotherapy_Center
{
    public class ScreenShot
    {
        private string Name;

        public ScreenShot(string name)
        {
            this.Name = name;
        }
        public void CaptureScreen()
        {
            try
            {
                int screenWidth = 1920;
                int screenHeight = 1080;
                Bitmap screenshot = new Bitmap(screenWidth, screenHeight);

                using (Graphics graphics = Graphics.FromImage(screenshot))
                {
                    graphics.CopyFromScreen(0, 0, 0, 0, screenshot.Size);
                }

                string reportsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "التقارير");

                if (!Directory.Exists(reportsFolderPath))
                {
                    Directory.CreateDirectory(reportsFolderPath);
                }

                string savePath = Path.Combine(reportsFolderPath, Name + ".png");
                screenshot.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ: " + ex.Message);
            }
        }
    }
}
