using System;
using System.Windows.Forms;

namespace HimawariWallpaper
{
    public partial class SettingWindow : Form
    {
        public SettingWindow()
        {
            InitializeComponent();
        }

        private void SettingWindow_Load(object sender, EventArgs e)
        {
            ;
        }

        private void downloadNowButton_Click(object sender, EventArgs e)
        {
            HimawariDownloader.Downloader downloader = new HimawariDownloader.Downloader();
            string imagePath = downloader.DownloadSATWallpaper(DateTime.UtcNow, HimawariDownloader.Downloader.eSAType.HIWAMARI8);
            
            if (!string.IsNullOrEmpty(imagePath))
            {
                MessageBox.Show(string.Format("다운로드 성공. 위치 : {0}", imagePath));
                if (setWallpaperCheckbox.Checked)
                {
                    HimawariDownloader.SetWallpaper.FromFilePath(imagePath);
                }
            }
        }
    }
}
