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
            progTextBox.Text = "";

            progress.Minimum = 0;
            progress.Maximum = 10;
        }

        private void OnProgressDownloadStatus(HimawariDownloader.Downloader.eProgress status, bool result)
        {
            System.Text.StringBuilder SB = new System.Text.StringBuilder();
            SB.Append(progTextBox.Text);

            switch (status)
            {
                case HimawariDownloader.Downloader.eProgress.CONNECT:
                    SB.AppendLine("접속 완료");
                    break;
                case HimawariDownloader.Downloader.eProgress.SAVE_IMAGE:
                    SB.AppendLine("이미지 저장 완료");
                    break;
                case HimawariDownloader.Downloader.eProgress.WRITE_IMAGE:
                    SB.AppendLine("이미지 쓰기 완료");
                    break;
                case HimawariDownloader.Downloader.eProgress.COMPLETE:
                    SB.AppendLine("완료!");
                    break;
                default:
                    break;
            }

            progTextBox.Text = SB.ToString();
                
            //int realValue = (int)(progress.Maximum * value);
            //progress.Increment(realValue);
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
