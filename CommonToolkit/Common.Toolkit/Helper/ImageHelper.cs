using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Toolkit.Helper
{
    public class ImageHelper
    {
        /// <summary>  
        /// 调整图片大小  
        /// </summary>  
        /// <param name="strOldPic">源图文件名(包括路径)</param>  
        /// <param name="strNewPic">缩小后保存为文件名(包括路径)</param>  
        /// <param name="intWidth">宽度</param>  
        /// <param name="intHeight">高度</param>  
        public static void ResizeImg(string strOldPic, string strNewPic, int intWidth, int intHeight)
        {

            System.Drawing.Bitmap objPic, objNewPic;
            try
            {
                objPic = new System.Drawing.Bitmap(strOldPic);
                //System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(objPic);

                objNewPic = new System.Drawing.Bitmap(objPic, intWidth, intHeight);

                objNewPic.SetResolution(20, 20);

                objNewPic.Save(strNewPic);
            }
            catch (Exception exp)
            {
                //throw exp;
            }
            finally
            {
                objPic = null;
                objNewPic = null;
            }
        }


        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
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
            ImageCodecInfo myImageCodecInfo;
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            // Get an ImageCodecInfo object that represents the JPEG codec.
            myImageCodecInfo = GetEncoderInfo("image/jpeg");

            myEncoder = System.Drawing.Imaging.Encoder.Quality;

            myEncoderParameters = new EncoderParameters(1);

            // 用给定的Level值压缩图片
            myEncoderParameter = new EncoderParameter(myEncoder, level);
            myEncoderParameters.Param[0] = myEncoderParameter;

            using (Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile(oldPic))
            {
                using (Stream newBitmap = File.OpenWrite(newPic))
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
        public static void ResizeImage(string oldPic, string newPic, int newW, int newH, InterpolationMode level = InterpolationMode.HighQualityBicubic)
        {
            using (Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile(oldPic))
            {
                Graphics g = Graphics.FromImage(bitmap);
                // 插值算法的质量
                g.InterpolationMode = level;
                using (Bitmap newBitmap = new Bitmap(bitmap, newW, newH))
                {
                    g.DrawImage(newBitmap, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
                    newBitmap.Save(newPic);
                }
                g.Dispose();
            }
        }
    }
}
