using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using ThoughtWorks.QRCode.Codec;
using System.IO;
using Evt.Framework.Common;
using ThoughtWorks.QRCode.Codec.Data;


using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using Yme.Util.Extension;

namespace Yme.Util
{
    /// <summary>
    /// 二维码助手
    /// </summary>
    public class QRCodeUtil
    {
        #region 生成二维码

        /// <summary>
        /// 生成【带图片】二维码
        /// </summary>
        /// <param name="data">二维码数据</param>
        /// <param name="logoImgPath">嵌入的图片路径</param>
        /// <param name="savePath">二维码图片保存路径</param>
        /// <returns>二维码图片</returns>
        public static Image GetQRCode(string data, string logoImgPath = "", string savePath = "")
        {
            return GetQRCode(data, 5, 7, QRCodeEncoder.ENCODE_MODE.BYTE, QRCodeEncoder.ERROR_CORRECTION.M, logoImgPath, savePath,false);
        }

        /// <summary>
        /// 生成【带图片】二维码
        /// </summary>
        /// <param name="data">二维码数据</param>
        /// <param name="scale">尺寸</param>
        /// <param name="version">版本</param>
        /// <param name="encoding">加密方式</param>
        /// <param name="correctionLevel">容错级别</param>
        /// <param name="logoImgPath">嵌入的图片路径</param>
        /// <param name="savePath">二维码图片保存路径</param>
        /// /// <param name="savePath">是否要删除</param>
        /// <returns>二维码图片</returns>
        public static Image GetQRCode(string data, int scale, int version, QRCodeEncoder.ENCODE_MODE encoding, QRCodeEncoder.ERROR_CORRECTION correctionLevel, string logoImgPath = "", string savePath = "",bool existDel=true)
        {
            //校验
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new MessageException("数据不能为空!");
            }

            //参数初始化
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeScale = scale;
            qrCodeEncoder.QRCodeVersion = version;
            qrCodeEncoder.QRCodeEncodeMode = encoding;
            qrCodeEncoder.QRCodeErrorCorrect = correctionLevel;

            //返回二维码图片
            Image image;
            try
            {
                //Encoding.UTF8
                var tempImage = qrCodeEncoder.Encode(data, Encoding.Default);
                int width = 0;//tempImage.Width / 10;
                int dwidth = 0;//width * 2;
                image = new Bitmap(tempImage.Width + dwidth, tempImage.Height + dwidth);
                Graphics g = Graphics.FromImage(image);
                var c = System.Drawing.Color.White;
                g.FillRectangle(new SolidBrush(c), 0, 0, tempImage.Width + dwidth, tempImage.Height + dwidth);
                g.DrawImage(tempImage, width, width);
                g.Dispose();

                //二维码中嵌入图片
                if (!string.IsNullOrWhiteSpace(logoImgPath))
                {
                    var logoImg = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logoImgPath.Replace(AppDomain.CurrentDomain.BaseDirectory, string.Empty));
                    if (!File.Exists(logoImg))
                    {
                        throw new MessageException("嵌入二位码中的图片文件不存在!");
                    }

                    //合并图片
                    image = CombinImage(image, logoImg);
                }

                //保存二维码
                if (!string.IsNullOrWhiteSpace(savePath))
                {
                    var qrCodeImgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, savePath.Replace(AppDomain.CurrentDomain.BaseDirectory,string.Empty));
                    if(existDel)
                    {
                        if (File.Exists(qrCodeImgPath))
                        {
                            File.Delete(qrCodeImgPath);
                        }
                        image.Save(qrCodeImgPath, ImageFormat.Png);
                    }
                    else
                    {
                        if (!File.Exists(qrCodeImgPath))
                        {
                            image.Save(qrCodeImgPath, ImageFormat.Png);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new MessageException(ex.Message);
            }

            return image;
        }

        #endregion

        #region 解密二维码

        /// <summary>
        /// 解密二维码
        /// </summary>
        /// <param name="qrCodeImg">二维码图片</param>
        /// <returns>二维码数据</returns>
        public string DeQRCode(Image qrCodeImg)
        {
            QRCodeDecoder decoder = new QRCodeDecoder();
            return decoder.decode(new QRCodeBitmapImage(new Bitmap(qrCodeImg)), Encoding.UTF8);
        }

        #endregion

        #region 私有方法

        /// <summary>  
        /// 调用此函数后使此两种图片合并，类似相册，有个背景图，中间贴自己的目标图片  
        /// </summary>  
        /// <param name="backImg">相框背景图片(二维码)</param>  
        /// <param name="logoImg">嵌入的Logo图片</param>  
        private static Image CombinImage(Image backImg, string logoImg)
        {
            Image img = Image.FromFile(logoImg);        //照片图片    
            if (img.Height != 50 || img.Width != 50)
            {
                img = KiResizeImage(img, 50, 50, 0);
            }
            Graphics g = Graphics.FromImage(backImg);

            //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);   
            g.DrawImage(backImg, 0, 0, backImg.Width, backImg.Height);

            //相片四周刷一层黑色边框  
            g.FillRectangle(System.Drawing.Brushes.White, backImg.Width / 2 - img.Width / 2 - 3, backImg.Width / 2 - img.Width / 2 - 3, img.Width + 6, img.Height + 6);
            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);  
            g.DrawImage(img, backImg.Width / 2 - img.Width / 2, backImg.Height / 2 - img.Height / 2, img.Width, img.Height);
            GC.Collect();
            return backImg;
        }

        /// <summary>  
        /// Resize图片  
        /// </summary>  
        /// <param name="bmp">原始Bitmap</param>  
        /// <param name="newW">新的宽度</param>  
        /// <param name="newH">新的高度</param>  
        /// <param name="mode">保留着，暂时未用</param>  
        /// <returns>处理以后的图片</returns>  
        private static Image KiResizeImage(Image bmp, int newW, int newH, int mode)
        {
            try
            {
                System.Drawing.Image b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);

                // 插值算法的质量  
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();

                return b;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
