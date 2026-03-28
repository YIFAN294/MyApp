using System.IO.Ports;
using System.Text;

namespace ModBus01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SerialPort _serialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);

        //定义一个取消异步/长时间运行的任务”的一个工具类  创建“信号源”
        private CancellationTokenSource _cts;

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;//检测线程冲突设置成false
            _serialPort.DataReceived += _serialPort_DataReceived;
        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            ////接受数据
            //int readBufferLength = _serialPort.ReadBufferSize;
            //byte[] readbuffer = new byte[readBufferLength];

            //_serialPort.Read(readbuffer, 0, readBufferLength);

            //int datalength = readbuffer[2];

            ////要解码的字节数，即从 readbuffer[3]起，连续取 datalength个字节，用 UTF-8 转成字符串
            //listBox1.Items.Add(Encoding.UTF8.GetString(readbuffer, 3, datalength));


            int len = _serialPort.BytesToRead;
            byte[] readbuffer = new byte[len];
            _serialPort.Read(readbuffer,0,len);

            if (len < 5) return;

            int slaveId = readbuffer[0];
            int byteCount = readbuffer[2];  // 数据区字节数
            if (len >= 3 + byteCount + 2)  // 3 字节头 + 数据 + 2 字节 CRC   //3 字节头：从站地址（1）+ 功能码（1）+ 字节数（1）byteCount字节数据区 2 字节 CRC 校验
            {
                // 提取数据区
                byte[] dataArea = new byte[byteCount];
                Array.Copy(readbuffer, 3, dataArea, 0, byteCount); //Array.Copy从 readbuffer的索引 3 开始（跳过 3 字节头），复制 byteCount个字节到 dataArea。这样 dataArea里就只剩下纯数据区，不含地址、功能码、CRC。

                // 按“每两个字节一个寄存器”解析
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < byteCount; i += 2)
                {
                    ushort regValue = (ushort)((dataArea[i] << 8) | dataArea[i + 1]);
                    if (i > 0) sb.Append(", ");
                    sb.Append(regValue);
                }

                // 显示到 ListBox
                this.Invoke((MethodInvoker)(() =>
                {
                    listBox1.Items.Add($"从站{slaveId} 数据: {sb}");
                }));
            }



            //throw new NotImplementedException();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }

            //老的方式弃用
            //Task.Factory.StartNew(() =>
            //{
            //    while (_serialPort.IsOpen)
            //    {
            //        //发送数据
            //        byte[] sendbuffer = new byte[] { 0x01, 0x03, 0x00, 0x63, 0x00, 0x06, 0x35, 0xd6 };
            //        _serialPort.Write(sendbuffer, 0, sendbuffer.Length);

            //        Thread.Sleep(1000);
            //    }

            //});


            _cts = new CancellationTokenSource();
            Task.Run(async () =>
            {

                while (!_cts.Token.IsCancellationRequested && _serialPort.IsOpen)
                {
                    byte[] sendbuffer = new byte[]
                    {
                        0x01, 0x03, 0x00, 0x63, 0x00, 0x06, 0x35, 0xd6
                    };

                    // 用异步写入，不阻塞线程
                    await _serialPort.BaseStream.WriteAsync(sendbuffer, 0, sendbuffer.Length, _cts.Token);

                    // 用异步延迟，不占线程
                    await Task.Delay(1000, _cts.Token);
                }

            });

            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _cts?.Cancel();


            //test
            //test02
        }
    }
}
