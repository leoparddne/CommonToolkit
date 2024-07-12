using System;
using System.Collections.Generic;
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
                objNewPic = new System.Drawing.Bitmap(objPic, intWidth, intHeight);
                objNewPic.Save(strNewPic);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                objPic = null;
                objNewPic = null;
            }
        }
    }
}
