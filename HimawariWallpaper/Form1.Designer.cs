namespace HimawariWallpaper
{
    partial class SettingWindow
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.downloadNowButton = new System.Windows.Forms.Button();
            this.setWallpaperCheckbox = new System.Windows.Forms.CheckBox();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // downloadNowButton
            // 
            this.downloadNowButton.Location = new System.Drawing.Point(63, 38);
            this.downloadNowButton.Name = "downloadNowButton";
            this.downloadNowButton.Size = new System.Drawing.Size(136, 32);
            this.downloadNowButton.TabIndex = 0;
            this.downloadNowButton.Text = "지금 다운로드";
            this.downloadNowButton.UseVisualStyleBackColor = true;
            this.downloadNowButton.Click += new System.EventHandler(this.downloadNowButton_Click);
            // 
            // setWallpaperCheckbox
            // 
            this.setWallpaperCheckbox.AutoSize = true;
            this.setWallpaperCheckbox.Checked = true;
            this.setWallpaperCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.setWallpaperCheckbox.Location = new System.Drawing.Point(46, 16);
            this.setWallpaperCheckbox.Name = "setWallpaperCheckbox";
            this.setWallpaperCheckbox.Size = new System.Drawing.Size(168, 16);
            this.setWallpaperCheckbox.TabIndex = 1;
            this.setWallpaperCheckbox.Text = "다운로드 완료시 바로 적용";
            this.setWallpaperCheckbox.UseVisualStyleBackColor = true;
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(12, 212);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(237, 23);
            this.progress.TabIndex = 2;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(13, 83);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(236, 112);
            this.listBox1.TabIndex = 3;
            // 
            // SettingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 247);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.setWallpaperCheckbox);
            this.Controls.Add(this.downloadNowButton);
            this.Name = "SettingWindow";
            this.Text = "HimawariDownloader";
            this.Load += new System.EventHandler(this.SettingWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button downloadNowButton;
        private System.Windows.Forms.CheckBox setWallpaperCheckbox;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.ListBox listBox1;
    }
}

