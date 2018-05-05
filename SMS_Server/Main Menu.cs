using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMS_Server
{
    public partial class Form1 : Form
    {
        List<device> activeDevices;
        public Form1()
        {
            InitializeComponent();
            activeDevices = new List<device>();
            initDevices(getAllPorts());

        }

        private void initDevices(string[] devices)
        {
            foreach(String d in devices)
            {
                SerialPort port = new SerialPort(d, Int32.Parse(device_config.baud_rate));
                device gsmDevice = new device(port, this);
                activeDevices.Add(gsmDevice);
            }
        }

        public void updatePending(String m)
        {
            this.Invoke((MethodInvoker)delegate
            {
                lblPending.Text = m;
            });
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private String[] getAllPorts()
        {
            return System.IO.Ports.SerialPort.GetPortNames();
        }
        
    }
}
