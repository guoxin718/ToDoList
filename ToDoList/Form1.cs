using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToDoList
{
	public partial class Form1 : Form
	{
        private StringBuilder wallPaperPath = new StringBuilder(200);
        public Form1()
		{
			InitializeComponent();
            //textBox1.Text = File.ReadAllText(@"D:\\dev_vs\\ToDoList\\1.txt", Encoding.UTF8);
            textBox1.Text = File.ReadAllText(@"1.txt", Encoding.UTF8);
            textBox2.Text = File.ReadAllText(@"2.txt", Encoding.UTF8);
			textBox3.Text = File.ReadAllText(@"3.txt", Encoding.UTF8);
			textBox4.Text = File.ReadAllText(@"4.txt", Encoding.UTF8);



            const int SPI_GETDESKWALLPAPER = 0x0073;
            //StringBuilder wallPaperPath = new StringBuilder(200);
            if (!SystemParametersInfo(SPI_GETDESKWALLPAPER, 200, wallPaperPath, 0))
            {
                MessageBox.Show("无法获取桌面背景的图片，请重试！");
            }

            //程序启动的时候，加载桌面背景
            string currentImg = System.Environment.CurrentDirectory + "\\new.png";
            SystemParametersInfo(20, 0, currentImg, 0x2);
        }

		public void WriteToFile()
		{
            //如果文件不存在，则创建；存在则覆盖
            //System.IO.File.WriteAllText(@"D:\\dev_vs\\ToDoList\\1.txt", textBox1.Text, Encoding.UTF8);
            System.IO.File.WriteAllText(@"1.txt", textBox1.Text, Encoding.UTF8);
            System.IO.File.WriteAllText(@"2.txt", textBox2.Text, Encoding.UTF8);
			System.IO.File.WriteAllText(@"3.txt", textBox3.Text, Encoding.UTF8);
			System.IO.File.WriteAllText(@"4.txt", textBox4.Text, Encoding.UTF8);
		}

		[DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
		public static extern int SystemParametersInfo(
		int uAction,
		int uParam,
		string lpvParam,
		int fuWinIni
		);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SystemParametersInfo(uint uAction, uint uParam, StringBuilder lpvParam, uint init);

        private void BtnSave_Click(object sender, EventArgs e)
        {
            WriteToFile();

            //获取原背景图片的路径
            //System.Drawing.Image imgSrc = System.Drawing.Image.FromFile("D:\\dev_vs\\ToDoList\\template1.jpg");
            //SystemParametersInfo(20, 0, "D:\\dev_vs\\ToDoList\\new.jpg", 0x2);

            System.Drawing.Image imgSrc = System.Drawing.Image.FromFile("template1.jpg");
            //System.Drawing.Image imgSrc = System.Drawing.Image.FromFile(wallPaperPath.ToString());

            using (Graphics g = Graphics.FromImage(imgSrc))
			{
				g.DrawImage(imgSrc, 0, 0, imgSrc.Width, imgSrc.Height);
				using (Font f = new Font("微软雅黑", 30))
				{
					using (Brush b = new SolidBrush(Color.Black))
					{
						int x = 720, y = 250, h = 1368, w = 768;
						g.DrawString(textBox1.Text, f, b, new Rectangle(x + 30, y + 10, w, h));
						g.DrawString(textBox2.Text, f, b, new Rectangle(x + 990, y + 10, w, h));
						g.DrawString(textBox3.Text, f, b, new Rectangle(x + 30, y + 550, w, h));
						g.DrawString(textBox4.Text, f, b, new Rectangle(x + 990, y + 550, w, h));
					}
				}
			}

            //imgSrc.Save("D:\\dev_vs\\ToDoList\\new.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            imgSrc.Save("new.png", System.Drawing.Imaging.ImageFormat.Png);

            string currentImg = System.Environment.CurrentDirectory + "\\new.png";
            SystemParametersInfo(20, 0, currentImg, 0x2);

        }

        private void NotifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            ////还原窗体显示    
            //WindowState = FormWindowState.Normal;
            ////激活窗体并给予它焦点
            //this.Activate();
            ////任务栏区显示图标
            //this.ShowInTaskbar = true;
            ////托盘区图标隐藏
            //notifyIcon1.Visible = false;

            if (e.Button == MouseButtons.Left)//判断鼠标的按键
            {
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.WindowState = FormWindowState.Minimized;
                    this.Hide();
                }
                else if (this.WindowState == FormWindowState.Minimized)
                {
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    this.Activate();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                
                //if (MessageBox.Show("是否需要关闭程序？", "提示:", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)//出错提示
                //{
                //    //关闭窗口
                //    DialogResult = DialogResult.No;
                //    Dispose();
                //    Close();
                //}
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;//取消窗体的关闭事件
            this.WindowState = FormWindowState.Minimized;//使当前窗体最小化
            notifyIcon1.Visible = true;//使最下滑的图标可见
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //判断是否选择的是最小化按钮
            if (WindowState == FormWindowState.Minimized)
            {
                //隐藏任务栏区图标
                this.ShowInTaskbar = false;
                //图标显示在托盘区
                notifyIcon1.Visible = true;
            }
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ////程序退出的时候，还原桌面背景
            SystemParametersInfo(20, 0, wallPaperPath.ToString(), 0x2);
            Dispose();
            Close();
        }
    }
}
