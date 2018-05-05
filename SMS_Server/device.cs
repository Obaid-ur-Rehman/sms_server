using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;

namespace SMS_Server
{
    class device
    {
        Thread reader, writer;
        SerialPort devicePort;
        Form1 f;

        public device(SerialPort port, Form1 frm)
        {
            devicePort = port;
            devicePort.Open();
            f = frm;

            writer = new Thread(backgroundWriter);
            writer.IsBackground = true;
            writer.Start();



            reader = new Thread(backgroundReader);
            reader.IsBackground = true;
            reader.Start();

            //initDevice();

            
        }

        private void initDevice()
        {
            devicePort.Write(device_config.textMode + "\r\n");
            //Thread.Sleep(200);
        }

        private void readBalance()
        {
            devicePort.Write(device_config.balance_inq + "\r\n");
        }

        private void backgroundReader()
        {
            while(true)
            {
                //String s = devicePort.ReadTo("\n");
                readResult(devicePort.ReadLine());
            }
        }

        private void readResult(String feed)
        {
            if(feed.StartsWith(">"))
            {
                //writeSMS();
            }
            if (feed.StartsWith("+CUSD"))
            {
                //Get balance from feed
            }
            if(feed.StartsWith("+CMGS"))
            {
                
                sms_queue.pending--;
                f.updatePending(sms_queue.pending.ToString());

                
            }
        }

        private void writeSMS()
        {
            devicePort.Write("Test\r");
            devicePort.Write((char)26 + "\r\n");
        }

        private void backgroundWriter()
        {
            while (true)
            {
                sendSMS("03038170020");
                
                Thread.Sleep(Int32.Parse(device_config.sms_delay));
                
            }
        }

        private void sendSMS(string number)
        {
            sms_queue.pending++;
            f.updatePending(sms_queue.pending.ToString());
            devicePort.Write("AT+CMGS=" + number + "\r");
            Thread.Sleep(100);
            devicePort.Write("Test\r");
            devicePort.Write((char)26 + "\r");
        }
    }
}
