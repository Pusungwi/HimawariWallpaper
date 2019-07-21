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
            progress.Minimum = 0;
            progress.Maximum = 10;
        }

        private void OnProgressDownloadStatus(float value)
        {
            int realValue = (int)(progress.Maximum * value);
            progress.Increment(realValue);
        }

        private void downloadNowButton_Click(object sender, EventArgs e)
        {
            HimawariDownloader.Downloader downloader = new HimawariDownloader.Downloader();
            downloader.onProgressDownload = OnProgressDownloadStatus;
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
