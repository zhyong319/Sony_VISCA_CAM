using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace VISCA_CAM
{
    public partial class Form1 : Form
    {
        //byte[] revBuff;
        public Form1()
        {
            InitializeComponent();
            this.comboBox1.Text = @"COM3";

        }

        private void button1_Click(object sender, EventArgs e) //打开串口
        {
           
            
            if (this.serialPort1.IsOpen == false) 
            {
                try
                {
                    this.serialPort1.StopBits = StopBits.One;
                    this.serialPort1.DataBits = 8;
                    this.serialPort1.BaudRate = 9600;
                    this.serialPort1.PortName = this.comboBox1.Text;
                    this.serialPort1.Open();
                    this.button1.Text = "关闭串口";
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("访问被拒绝的端口！");
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("指定的端口已被打开！");
                }
                catch (IOException)
                {
                    MessageBox.Show("该端口处于无效状态");
                }
            }
            else
            {
                this.serialPort1.Close();
                this.button1.Text = "打开串口";
            }
            
        }

        private void button2_Click(object sender, EventArgs e) // up
        {
            byte[] buff_up = { 0x81, 0x01, 0x06, 0x01, 0x00, 0x05,0x03,0x01,0xff };
            this.serialPort1.Write(buff_up, 0, buff_up.Length);
            if (this.checkBox1.Checked == true)
            {
                byte[] buff_stop = { 0x81, 0x01, 0x06, 0x01, 0x00, 0x05, 0x03, 0x03, 0xff };
                this.serialPort1.Write(buff_stop, 0, buff_stop.Length);
            }
            this.textBox1.Text = "数据发送成功";
        }

        private void button4_Click(object sender, EventArgs e) //down
        {
            byte[] buff_down = { 0x81, 0x01, 0x06, 0x01, 0x00, 0x05, 0x03, 0x02, 0xff };
            this.serialPort1.Write(buff_down, 0, buff_down.Length);
            if (this.checkBox1.Checked == true)
            {
                byte[] buff_stop = { 0x81, 0x01, 0x06, 0x01, 0x00, 0x05, 0x03, 0x03, 0xff };
                this.serialPort1.Write(buff_stop, 0, buff_stop.Length);
            }
            this.textBox1.Text = "数据发送成功";

        }

        private void button3_Click(object sender, EventArgs e) //left
        {
            byte[] buff_left = { 0x81,0x01,0x06,0x01,0x05,0x00,0x01,0x03,0xff};
            this.serialPort1.Write(buff_left, 0, buff_left.Length);
            if (this.checkBox1.Checked == true)
            {
                byte[] buff_stop = { 0x81, 0x01, 0x06, 0x01, 0x00, 0x05, 0x03, 0x03, 0xff };
                this.serialPort1.Write(buff_stop, 0, buff_stop.Length);
            }
            this.textBox1.Text = "数据发送成功";
        }

        private void button5_Click(object sender, EventArgs e) //right
        {
            byte[] buff_right = { 0x81, 0x01, 0x06, 0x01, 0x05, 0x00, 0x02, 0x03, 0xff };
            this.serialPort1.Write(buff_right, 0, buff_right.Length);
            if (this.checkBox1.Checked == true)
            {
                byte[] buff_stop = { 0x81, 0x01, 0x06, 0x01, 0x00, 0x05, 0x03, 0x03, 0xff };
                this.serialPort1.Write(buff_stop, 0, buff_stop.Length);
            }
            this.textBox1.Text = "数据发送成功";
        }

        private void button6_Click(object sender, EventArgs e) //stop
        {
            byte[] buff_stop = { 0x81, 0x01, 0x06, 0x01, 0x00, 0x05, 0x03, 0x03, 0xff };
            this.serialPort1.Write(buff_stop, 0, buff_stop.Length);
            this.textBox1.Text = "数据发送成功";
        }

        private void button7_Click(object sender, EventArgs e) //power on
        {
            byte[] buff_poweron = { 0x81,0x01,0x04,0x00,0x02,0xff};
            this.serialPort1.Write(buff_poweron,0,buff_poweron.Length);
            this.textBox1.Text = "数据发送成功";

        }

        private void button8_Click(object sender, EventArgs e) //pwoer off
        {
            byte[] buff_poweroff = { 0x81, 0x01, 0x04, 0x00, 0x03, 0xff };
            this.serialPort1.Write(buff_poweroff, 0, buff_poweroff.Length);
            this.textBox1.Text = "数据发送成功";
        }

        private void button11_Click(object sender, EventArgs e) //get local infomation
        {
            byte[] buff = { 0x81,0x09,0x06,0x12,0xff};
            this.serialPort1.Write(buff, 0, buff.Length);
            this.textBox1.Text = "数据发送成功";
           
                    
        }

        private void OnRevData(object sender, SerialDataReceivedEventArgs e) //rev serial data
        {
            byte[] inputBuff = new byte[20];
            try
            {
                for (int ii = 0; ii < 20; ii++)
                {

                    inputBuff[ii] = (byte)this.serialPort1.ReadByte();
                    if (inputBuff[ii] == 0xff)
                    {
                        this.serialPort1.DiscardInBuffer();
                        this.textBox1.Text = "数据接收成功！";
                        this.textBox1.Text = byteToHexStr(inputBuff);
                        return;
                    }
                }
            }
            catch (TimeoutException)
            {
                this.textBox1.Text = "数据接收超时!";
            }
        }

        public static string byteToHexStr(byte[] bytes) //将BYTE数组转为16进制的字符串
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                    if (bytes[i] == 0xff) return returnStr;
                    returnStr += " ";
                }
            }
            return returnStr;
        }

        private void button12_Click(object sender, EventArgs e) //镜头停止
        {
            byte[] buff = { 0x81,0x01,0x04,0x07,0x00,0xff };
            this.serialPort1.Write(buff, 0, buff.Length);
            this.textBox1.Text = "数据发送成功";
        }

        private void button9_Click(object sender, EventArgs e) //镜头拉近
        {
            byte[] buff = { 0x81, 0x01, 0x04, 0x07, 0x02, 0xff };
            this.serialPort1.Write(buff, 0, buff.Length);
            if (this.checkBox2.Checked == true)
            {
                byte[] buff1 = { 0x81, 0x01, 0x04, 0x07, 0x00, 0xff };
                this.serialPort1.Write(buff1, 0, buff1.Length);
            }
            this.textBox1.Text = "数据发送成功";
        }

        private void button10_Click(object sender, EventArgs e) //镜头推远
        {
            byte[] buff = { 0x81, 0x01, 0x04, 0x07, 0x03, 0xff };
            this.serialPort1.Write(buff, 0, buff.Length);
            if (this.checkBox2.Checked == true)
            {
                byte[] buff1 = { 0x81, 0x01, 0x04, 0x07, 0x00, 0xff };
                this.serialPort1.Write(buff1, 0, buff1.Length);
            }
            this.textBox1.Text = "数据发送成功";
        }
    }
    }

 