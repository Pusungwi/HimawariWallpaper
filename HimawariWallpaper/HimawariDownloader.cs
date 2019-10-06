using System;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using Microsoft.Win32;

namespace HimawariDownloader
{
    public class SetWallpaper
    {
        public static readonly int CONST_INT_SET_DESKTOP_WALLPAPER = 20;
        public static readonly int CONST_INT_UPDATE_INIT_FILE = 0x0001;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        private static void SetParam(string path)
        {
            SystemParametersInfo(CONST_INT_SET_DESKTOP_WALLPAPER, 0, path, CONST_INT_UPDATE_INIT_FILE);
        }

        public static bool FromFilePath(string path)
        {
            bool result = false;

            if (File.Exists(path))
            {
                result = true;

                //Tile:
                //    key.SetValue(@"WallpaperStyle", "0");
                //    key.SetValue(@"TileWallpaper", "1");
                //    break;
                //Center:
                //    key.SetValue(@"WallpaperStyle", "0");
                //    key.SetValue(@"TileWallpaper", "0");
                //    break;
                //Stretch:
                //    key.SetValue(@"WallpaperStyle", "2");
                //    key.SetValue(@"TileWallpaper", "0");
                //    break;
                //Fill:
                //    key.SetValue(@"WallpaperStyle", "10");
                //    key.SetValue(@"TileWallpaper", "0");
                //    break;
                //Fit:
                //    key.SetValue(@"WallpaperStyle", "6");
                //    key.SetValue(@"TileWallpaper", "0");
                //    break;

                RegistryKey desktopKey = Registry.CurrentUser.OpenSubKey("Control Panel", true).OpenSubKey("Desktop", true);
                desktopKey.SetValue("Wallpaper", path);
                desktopKey.SetValue("WallpaperStyle", "6");
                desktopKey.SetValue("TileWallpaper", "0");
                desktopKey.Close();

                RegistryKey colorKey = Registry.CurrentUser.OpenSubKey("Control Panel", true).OpenSubKey("Colors", true);
                colorKey.SetValue("Background", "0 0 0");
                colorKey.Close();

                SetParam(path);
            }

            return result;
        }
    }

    public class Downloader
    {
        public enum eSAType
        {
            HIWAMARI8 = 0,
            EPIC,
            MAX
        }

        public enum eProgress
        {
            NONE = 0,
            START,
            CONNECT,
            WRITE_IMAGE,
            SAVE_IMAGE,
            COMPLETE,
            MAX,
        }

        public Action<eProgress, bool> onProgressDownload = null;

        private readonly int CONST_INT_WIDTH = 550;
        private readonly int CONST_INT_NUM_BLOCKS = 4; //this apparently corresponds directly with the level, keep this exactly the same as level without the 'd'
        private readonly string CONST_STR_LEVEL = "4d"; //Level can be 4d, 8d, 16d, 20d 
        private readonly string FORMAT_SAT_IMG_URL = "https://himawari8-dl.nict.go.jp/himawari8/img/D531106/{0}/{1}/{2}/{3}/{4}/{5}";

        private string GetDownloadURL(DateTime time, eSAType type)
        {
            string targetTimeStr = time.ToString("HHmmss");
            string targetYearStr = time.ToString("yyyy");
            string targetMonthStr = time.ToString("MM");
            string targetDayStr = time.ToString("dd");
            string resultURL = string.Format(FORMAT_SAT_IMG_URL, CONST_STR_LEVEL, CONST_INT_WIDTH, targetYearStr, targetMonthStr, targetDayStr, targetTimeStr);

            return resultURL;
        }

        /// <summary>
        /// 위성 배경사진 다운로드 후 저장까지 해주는 함수
        /// </summary>
        /// <param name="time">대상 시간 (UTC 필수)</param>
        /// <param name="type">위성 종류</param>
        /// <returns></returns>
        public string DownloadSATWallpaper(DateTime time, eSAType type)
        {
            onProgressDownload?.Invoke(eProgress.START, true);

            string resultFilePath = "";
            DateTime targetTime = time.AddSeconds(-time.Second).AddMinutes(-30).AddMinutes(-(time.Minute % 10));
            string targetURL = "";
            
            // 파일이 정확히 있나 없나 테스트
            do
            {
                targetURL = GetDownloadURL(targetTime, type);
                string testUrl = targetURL + "_0_0.png"; //string.Format(FORMAT_SAT_IMG_URL, CONST_STR_LEVEL, CONST_INT_WIDTH, targetYearStr, targetMonthStr, targetDayStr, targetTimeStr);

                bool testResult = false;
                try
                {
                    WebRequest request = WebRequest.Create(testUrl);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    HttpStatusCode httpStatus = response.StatusCode;

                    if (httpStatus == HttpStatusCode.OK)
                    {
                        testResult = true;
                        onProgressDownload?.Invoke(eProgress.CONNECT, true);
                    }

                    response.Close();
                }
                catch
                {
                    testResult = false;
                }

                if (testResult)
                {
                    break;
                }
                else
                {
                    targetTime = targetTime.AddHours(-1);
                }
            } while (true);

            string saveFileName = "latest.png";
            string saveFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\Himawari8\\";
            if (!Directory.Exists(saveFolderPath))
            {
                Directory.CreateDirectory(saveFolderPath);
            }

            //저장 경로
            resultFilePath = saveFolderPath + saveFileName;

            Bitmap image = new Bitmap(CONST_INT_WIDTH * CONST_INT_NUM_BLOCKS, CONST_INT_WIDTH * CONST_INT_NUM_BLOCKS);
            Graphics graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Black);

            for (int i = 0; i < CONST_INT_NUM_BLOCKS; ++i)
            {
                for (int j = 0; j < CONST_INT_NUM_BLOCKS; ++j)
                {
                    string partImageURL = string.Format("{0}_{1}_{2}.png", targetURL, i, j);
                    //Console.WriteLine("Downloading: " + partImageURL);

                    try
                    {
                        WebRequest request = WebRequest.Create(partImageURL);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        HttpStatusCode httpStatus = response.StatusCode;

                        if (httpStatus == HttpStatusCode.OK)
                        {
                            Image imgblock = Image.FromStream(response.GetResponseStream());
                            graphics.DrawImage(imgblock, i * CONST_INT_WIDTH, j * CONST_INT_WIDTH, CONST_INT_WIDTH, CONST_INT_WIDTH);
                            imgblock.Dispose();
                            response.Close();

                            onProgressDownload?.Invoke(eProgress.WRITE_IMAGE, true);
                        }
                    }
                    catch (Exception e)
                    {
                        string errorMsg = string.Format("Failed! {0}", e.Message);
                    }
                }
            }

            //이미지 코덱 지정
            ImageCodecInfo pngCodecInfo = null;
            ImageCodecInfo[] encodersArray = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < encodersArray.Length; i++)
            {
                if (encodersArray[i].MimeType == "image/png")
                {
                    pngCodecInfo = encodersArray[i];
                    break;
                }
            }

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 90L);

            if (pngCodecInfo != null)
            {
                image.Save(resultFilePath, pngCodecInfo, encoderParams);
                onProgressDownload?.Invoke(eProgress.SAVE_IMAGE, true);
            }
            else
            {
                //실패 했으니 저장 패스 빈 값으로 처리해서 문제가 있음을 알린다
                resultFilePath = "";
            }

            onProgressDownload?.Invoke(eProgress.COMPLETE, true);

            return resultFilePath;
        }
    }
}
