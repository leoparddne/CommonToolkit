using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Common.Toolkit.Helper
{
    public class ImageHelper
    {
        /// <summary>  
        /// 调整图片大小  - 仅支持windows平台
        /// </summary>  
        /// <param name="strOldPic">源图文件名(包括路径)</param>  
        /// <param name="strNewPic">缩小后保存为文件名(包括路径)</param>  
        /// <param name="intWidth">宽度</param>  
        /// <param name="intHeight">高度</param>  
        public static void ResizeImg(string strOldPic, string strNewPic, int intWidth, int intHeight)
        {
            if (!OperatingSystem.IsWindows())
            {
                return;
            }
            Bitmap? objPic = null, objNewPic = null;
            try
            {
                objPic = new Bitmap(strOldPic);
                objNewPic = new Bitmap(objPic, intWidth, intHeight);

                objNewPic.SetResolution(20, 20);
                objNewPic.Save(strNewPic);
            }
            catch (Exception)
            {
                //throw exp;
            }
            finally
            {
                objPic?.Dispose();
                objNewPic?.Dispose();
            }
        }


        private static ImageCodecInfo? GetEncoderInfo(String mimeType)
        {
            if (!OperatingSystem.IsWindows())
            {
                return null;
            }

            int j;

            ImageCodecInfo[] encoders;

            encoders = ImageCodecInfo.GetImageEncoders();

            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                {
                    return encoders[j];
                }
            }

            return null;
        }

        /// <summary>
        /// 压缩图片文件储存容量
        /// </summary>
        /// <param name="oldPic"></param>
        /// <param name="newPic"></param>
        /// <param name="level">0最低 100最高</param>
        public static void Compress(string oldPic, string newPic, long level)
        {
            if (!OperatingSystem.IsWindows())
            {
                return;
            }

            ImageCodecInfo? myImageCodecInfo;
            Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            // Get an ImageCodecInfo object that represents the JPEG codec.
            myImageCodecInfo = GetEncoderInfo("image/jpeg");

            if (myImageCodecInfo == null)
            {
                return;
            }

            myEncoder = Encoder.Quality;

            myEncoderParameters = new EncoderParameters(1);

            // 用给定的Level值压缩图片
            myEncoderParameter = new EncoderParameter(myEncoder, level);
            myEncoderParameters.Param[0] = myEncoderParameter;

            using (var bitmap = (Bitmap)Image.FromFile(oldPic))
            {
                using (var newBitmap = File.OpenWrite(newPic))
                {
                    bitmap.Save(newBitmap, myImageCodecInfo, myEncoderParameters);
                }
            }
        }

        /// <summary>
        /// 调整图片大小
        /// </summary>
        /// <param name="oldPic"></param>
        /// <param name="newPic"></param>
        /// <param name="newW"></param>
        /// <param name="newH"></param>
        /// <param name="level"></param>
        public static void ResizeImage(string oldPic, string newPic, int newW, int newH, int qualityLevel = 7)
        {
            if (!OperatingSystem.IsWindows())
            {
                return;
            }

            InterpolationMode level = (InterpolationMode)qualityLevel;

            using (var bitmap = (Bitmap)Image.FromFile(oldPic))
            {
                var g = Graphics.FromImage(bitmap);
                // 插值算法的质量
                g.InterpolationMode = level;
                using (var newBitmap = new Bitmap(bitmap, newW, newH))
                {
                    g.DrawImage(newBitmap, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
                    newBitmap.Save(newPic);
                }
                g.Dispose();
            }
        }
    }
}
