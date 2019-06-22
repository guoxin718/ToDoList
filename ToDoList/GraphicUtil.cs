using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    class GraphicUtil
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr ptr);
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(
        IntPtr hdc, // handle to DC
        int nIndex // index of capability
        );
        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);
        const int HORZRES = 8;
        const int VERTRES = 10;
        const int LOGPIXELSX = 88;
        const int LOGPIXELSY = 90;
        const int DESKTOPVERTRES = 117;
        const int DESKTOPHORZRES = 118;

        /// <summary>
        /// 获取屏幕分辨率当前物理大小
        /// </summary>
        public static Size WorkingArea
        {
            get
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                Size size = new Size();
                size.Width = GetDeviceCaps(hdc, HORZRES);
                size.Height = GetDeviceCaps(hdc, VERTRES);
                ReleaseDC(IntPtr.Zero, hdc);
                return size;
            }
        }
        /// <summary>
        /// 当前系统DPI_X 大小 一般为96
        /// </summary>
        public static int DpiX
        {
            get
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                int DpiX = GetDeviceCaps(hdc, LOGPIXELSX);
                ReleaseDC(IntPtr.Zero, hdc);
                return DpiX;
            }
        }
        /// <summary>
        /// 当前系统DPI_Y 大小 一般为96
        /// </summary>
        public static int DpiY
        {
            get
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                int DpiX = GetDeviceCaps(hdc, LOGPIXELSY);
                ReleaseDC(IntPtr.Zero, hdc);
                return DpiX;
            }
        }
        /// <summary>
        /// 获取真实设置的桌面分辨率大小
        /// </summary>
        public static Size DESKTOP
        {
            get
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                Size size = new Size();
                size.Width = GetDeviceCaps(hdc, DESKTOPHORZRES);
                size.Height = GetDeviceCaps(hdc, DESKTOPVERTRES);
                ReleaseDC(IntPtr.Zero, hdc);
                return size;
            }
        }

        /// <summary>
        /// 获取宽度缩放百分比
        /// </summary>
        public static float ScaleX
        {
            get
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                int t = GetDeviceCaps(hdc, DESKTOPHORZRES);
                int d = GetDeviceCaps(hdc, HORZRES);
                float ScaleX = (float)GetDeviceCaps(hdc, DESKTOPHORZRES) / (float)GetDeviceCaps(hdc, HORZRES);
                ReleaseDC(IntPtr.Zero, hdc);
                return ScaleX;
            }
        }
        /// <summary>
        /// 获取高度缩放百分比
        /// </summary>
        public static float ScaleY
        {
            get
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                float ScaleY = (float)(float)GetDeviceCaps(hdc, DESKTOPVERTRES) / (float)GetDeviceCaps(hdc, VERTRES);
                ReleaseDC(IntPtr.Zero, hdc);
                return ScaleY;
            }
        }


        /// 根据屏幕分辨率生成生成新的图片
        /// </summary>
        /// <param name="sourceImagePath">原桌面背景图片地址</param>
        /// <param name="attachImagePath">附加背景图片地址</param>
        /// <param name="newImagePath">新生成图片的地址</param>
        /// <param name="width">桌面分辨率的宽度</param>
        /// <param name="height">桌面分辨率的高度</param>
        public static void NewImage(string sourceImagePath, string attachImagePath, string newImagePath, int width, int height,string t1,string t2,string t3,string t4)
        {
            System.Drawing.Image sourceImage = System.Drawing.Image.FromFile(sourceImagePath);
            System.Drawing.Image attachImage = System.Drawing.Image.FromFile(attachImagePath);

            //新生成图片的宽和高，设定为桌面分辨率的宽和高
            int towidth = width;
            int toheight = height;

            //原桌面背景图片的宽和高
            int ow = sourceImage.Width;
            int oh = sourceImage.Height;

            //原桌面图片分辨率高，不修改直接生成一个新的图片即可
            if ( towidth > ow || toheight> oh)
            {

            }

            //if (ow > oh)
            //{                
            //    toheight = sourceImage.Height * width / sourceImage.Width;
            //}
            //else
            //{
            //    towidth = sourceImage.Width * height / sourceImage.Height;
            //}

            //新建一个bmp图片
            System.Drawing.Image bm = new System.Drawing.Bitmap(width, height);
            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bm);
            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.White);
            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(sourceImage, new System.Drawing.Rectangle((width - towidth) / 2, (height - toheight) / 2, towidth, toheight),
                0, 0, ow, oh,
                System.Drawing.GraphicsUnit.Pixel);

            //g.DrawImage(attachImage, new System.Drawing.Rectangle((width - ow) / 2, (height - oh) / 2, towidth, toheight),
            //    0, 0, attachImage.Width, attachImage.Height,
            //    System.Drawing.GraphicsUnit.Pixel);

            g.DrawImage(attachImage, new System.Drawing.Rectangle((width - attachImage.Width) , (height - attachImage.Height)/2, attachImage.Width, attachImage.Height),
            0, 0, attachImage.Width, attachImage.Height,
            System.Drawing.GraphicsUnit.Pixel);

            using (Font f = new Font("微软雅黑", 30))
            {
                using (Brush b = new SolidBrush(Color.Green))
                {
                    int x = 720, y = 250, h = 1368, w = 768;
                    g.DrawString(t1, f, b, new Rectangle(x + 30, y + 10, w, h));
                    g.DrawString(t2, f, b, new Rectangle(x + 990, y + 10, w, h));
                    g.DrawString(t3, f, b, new Rectangle(x + 30, y + 550, w, h));
                    g.DrawString(t4, f, b, new Rectangle(x + 990, y + 550, w, h));
                }
            }


            try
            {
                //以jpg格式保存缩略图
                bm.Save(newImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                sourceImage.Dispose();
                attachImage.Dispose();
                bm.Dispose();
                g.Dispose();
            }
        }
    }
}
