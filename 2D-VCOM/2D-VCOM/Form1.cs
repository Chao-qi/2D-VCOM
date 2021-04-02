using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2D_VCOM
{
    public partial class Form1 : Form
    {
        SerialPort serialPort = new SerialPort();  //实例化串口对象
        public Form1()
        {
            InitializeComponent();
        }

        //初始化串口界面参数设置
        private void Init_Port_Confs()
        {
            /*串口界面参数设置*/
            string[] str = SerialPort.GetPortNames();  //检查是否含有串口
            if (str == null)
            {
                MessageBox.Show("本机没有串口！", "Error");
                return;
            }
            //添加串口
            foreach (string s in str)
            {
                comboBox1.Items.Add(s);
            }
            //设置默认串口选项
            comboBox1.SelectedIndex = 0;
            ASCII.Checked = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init_Port_Confs();

            Control.CheckForIllegalCrossThreadCalls = false;
            serialPort.DataReceived += new SerialDataReceivedEventHandler(dataReceived);
            //准备就绪
            serialPort.DtrEnable = true;
            serialPort.RtsEnable = true;
            //设置数据读取超时为1秒
            serialPort.ReadTimeout = 1000;

            serialPort.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!serialPort.IsOpen) //串口处于关闭状态
            {
                try
                {
                    if (comboBox1.SelectedIndex == -1)
                    {
                        MessageBox.Show("Error: 无效的端口,请重新选择", "Error");
                        return;
                    }
                    string strSerialName = comboBox1.SelectedItem.ToString();

                    serialPort.PortName = strSerialName;    //串口号
                    serialPort.BaudRate = 38400;            //波特率
                    serialPort.DataBits = 8;                //数据位
                    serialPort.StopBits = StopBits.One;     //停止位
                    serialPort.Parity = Parity.None;        //校验位

                    //打开串口
                    serialPort.Open();

                    //打开串口后设置将不再有效
                    button1.Text = "关闭串口";
                    button1.BackColor = Color.SpringGreen;

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message, "Error");
                    return;
                }
            }
            else //串口处于打开状态
            {

                serialPort.Close();//关闭串口
                //串口关闭时设置有效
                comboBox1.Enabled = true;

                button1.Text = "打开串口";
                button1.BackColor = Color.Transparent;
            }
        }
        private void dataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                DateTime dataTimeNow = DateTime.Now;
                textBox1.Text += string.Format("{0}\r\n", dataTimeNow);
                textBox1.ForeColor = Color.Blue;
                System.Threading.Thread.Sleep(5);
                Byte[] receivedData = new Byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);
                serialPort.DiscardInBuffer();
               // string strRcvh = null;
                string strRcva = null;
                for (int i = 0; i < receivedData.Length; i++)
                {
                    strRcva += receivedData[i].ToString("X2");
                    strRcva += " ";                   
                }
                if (Hex.Checked == true)
                {
                    textBox1.Text += strRcva + "\r\n";
                }
                else
                {
                   /* byte[] buff = new byte[strRcva.Length / 2];
                    int index = 0;
                    for (int i = 0; i < strRcva.Length; i += 2)
                    {
                        buff[index] = Convert.ToByte(strRcva.Substring(i, 2), 16);
                        ++index;
                    }*/
                    string result = Encoding.Default.GetString(receivedData);
                    textBox1.Text += result + "\r\n";
                }
            }
            else
            {
                MessageBox.Show("请打开某个串口", "错误提示");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           // serialPort.DiscardInBuffer(); //清空SerialPort控件的Buffer 
            if (!serialPort.IsOpen)
            {
                MessageBox.Show("请先打开串口", "Error");
                return;
            }
            String strsend = "get.ver";  //发送数据
            serialPort.WriteLine(strsend);  //发送一行数据
        }

        private void button3_Click(object sender, EventArgs e)
        {
          //  serialPort.DiscardInBuffer(); //清空SerialPort控件的Buffer 
            if (!serialPort.IsOpen)
            {
                MessageBox.Show("请先打开串口", "Error");
                return;
            }
            String strsend = "reset";  //发送数据
            serialPort.WriteLine(strsend);  //发送一行数据
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //serialPort.DiscardInBuffer(); //清空SerialPort控件的Buffer 
            if (!serialPort.IsOpen)
            {
                MessageBox.Show("请先打开串口", "Error");
                return;
            }
            String strsend = "load";  //发送数据
            serialPort.WriteLine(strsend);  //发送一行数据
        }

        private void button5_Click(object sender, EventArgs e)
        {
           // serialPort.DiscardInBuffer(); //清空SerialPort控件的Buffer 
            if (!serialPort.IsOpen)
            {
                MessageBox.Show("请先打开串口", "Error");
                return;
            }
            String strsend = "test";  //发送数据
            serialPort.WriteLine(strsend);  //发送一行数据
        }

        private void button6_Click(object sender, EventArgs e)
        {
           // serialPort.DiscardInBuffer(); //清空SerialPort控件的Buffer 
                                          // textBox1.Text = "";
            if (!serialPort.IsOpen)
            {
                MessageBox.Show("请先打开串口", "Error");
                return;
            }
            if (button6.BackColor == Color.Transparent)
            {
                String strSend = "lock";//发送框数据
                serialPort.WriteLine(strSend);//发送一行数据
                button6.BackColor = Color.SpringGreen;
                button6.Text = "Door unlock";
            }
            else if (button6.BackColor == Color.SpringGreen)
            {
                String strSend = "unlock";//发送框数据
                serialPort.WriteLine(strSend);//发送一行数据
                button6.BackColor = Color.Transparent;
                button6.Text = "Door lock";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
           // serialPort.DiscardInBuffer(); //清空SerialPort控件的Buffer 
                                          // textBox1.Text = "";
            if (!serialPort.IsOpen)
            {
                MessageBox.Show("请先打开串口", "Error");
                return;
            }
            if (button7.BackColor == Color.Transparent)
            {
                String strSend = "light-up";//发送框数据
                serialPort.WriteLine(strSend);//发送一行数据
                button7.BackColor = Color.SpringGreen;
                button7.Text = "Turn off";
            }
            else if (button7.BackColor == Color.SpringGreen)
            {
                String strSend = "Turn-off";//发送框数据
                serialPort.WriteLine(strSend);//发送一行数据
                button7.BackColor = Color.Transparent;
                button7.Text = "light up";
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
           // serialPort.DiscardInBuffer(); //清空SerialPort控件的Buffer 
            if (!serialPort.IsOpen)
            {
                MessageBox.Show("请先打开串口", "Error");
                return;
            }
            String strsend = "LEDstate";  //发送数据
            serialPort.WriteLine(strsend);  //发送一行数据
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (!serialPort.IsOpen)
            {
                //serialPort.DiscardInBuffer(); //清空SerialPort控件的Buffer 
               
                      MessageBox.Show("请先打开串口", "Error");
                      return;
             } 
               String strsend = "state";  //发送数据
               serialPort.WriteLine(strsend);  //发送一行数据
             }      

        private void button9_Click(object sender, EventArgs e)
        {
           // serialPort.DiscardInBuffer(); //清空SerialPort控件的Buffer 
            if (!serialPort.IsOpen)
            {
                MessageBox.Show("请先打开串口", "Error");
                return;
            }
            String strsend = "input";  //发送数据
            serialPort.WriteLine(strsend);  //发送一行数据
        }
        //保存文件
        private void button11_Click(object sender, EventArgs e)
        {
            StreamWriter myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                myStream = new StreamWriter(saveFileDialog1.FileName);

                myStream.Write(textBox1.Text); //写入

                myStream.Close();//关闭流
            }
        }
    }
        
}
