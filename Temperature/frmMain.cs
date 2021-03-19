using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;


namespace Temperature
{
    public partial class frmMain : Form
    {
        SerialPort UART = new SerialPort();

        private void _KhoiTao()
        {
            try
            {
                cbxcom.DataSource = SerialPort.GetPortNames();
                if (cbxcom.Items.Count > 0)
                {
                    cbxcom.SelectedIndex = 0;
                }
            }
            catch (Exception)
            {
            }
        }
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            _KhoiTao();
            this.UART.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.UART_DataReceived);

            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Interval =0 ;
            chart1.ChartAreas[0].AxisX.Maximum = 30;
            chart1.ChartAreas[0].AxisY.Maximum = 60;
            chart1.ChartAreas[0].AxisY.Minimum = 20;


        }   
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (UART.IsOpen)
            {
                UART.Close();
            }
        }



        string Tam = "";
        int Index = 0;
        int[] Data = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
        private void UART_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {

                Ibnhietdo.Text = UART.ReadTo("\r");
                Tam = UART.ReadTo("\r");
               Tam = Tam.Substring(0, 2);
                Data[0] = Convert.ToInt32(Tam);
                if (Index <= 28)
                {
                    Index = Index + 1;
                }
                for (int i = Index; i >= 1; i--)
                {
                    Data[i] = Data[i - 1];
                }
                chart1.Series["Nhiệt Độ"].Points.DataBindY(Data);
                chart1.Series["Nhiệt Độ"].Points.ResumeUpdates();
            }
            catch (Exception)
            {
            }
        }

        private void btnConnect_Click_1(object sender, EventArgs e)
        {
            {
                if (UART.IsOpen)//neu dang mo
                {
                    UART.Close();
                    btnConnect.Text = "KẾT NỐI";
                    cbxcom.Enabled = true;
                }
                else
                {
                    if (cbxcom.Text != "")
                    {
                        UART.PortName = cbxcom.Text;
                        try
                        {
                            UART.Open();
                            btnConnect.Text = "NGẮT KẾT NỐI";
                            cbxcom.Enabled = false;
                        }
                        catch
                        {
                            MessageBox.Show("KHÔNG THỂ MỞ CỔNG",
                         "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("XIN VUI LÒNG CHỌN CỔNG COM",
                            "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
