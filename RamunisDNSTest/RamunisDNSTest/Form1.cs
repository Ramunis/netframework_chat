using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace RamunisDNSTest
{
    public partial class Form1 : Form
    {
        Socket socket;
        public Form1()
        {
            InitializeComponent();
        }

        private void BindButton_Click(object sender, EventArgs e)
        {
            BindButton.Enabled = false;
            ButtonSend.Enabled = true;
            try
            {
                int localPort = Int32.Parse(textBoxLocalPort.Text);

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                IPEndPoint localPoint = new IPEndPoint(IPAddress.Any, localPort);

                socket.Bind(localPoint);

                timer1.Enabled = true;

                Log.Items.Add("UDP port is open: " + textBoxLocalPort.Text + "\n");
            }
            catch (Exception exc)
            {
                Log.Items.Add(exc.Message + "\n");
            }
        }

        private void ButtonSend_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress[] addresses = Dns.GetHostAddresses(textBoxHost.Text);

                if (addresses.Length > 0)
                {
                    int port = Int32.Parse(textBoxPort.Text);

                    IPEndPoint reciever = new IPEndPoint(addresses[0], port);

                    byte[] data = Encoding.UTF8.GetBytes(textBoxMessage.Text);

                    socket.SendTo(data, reciever);

                    Log.Items.Add(textBoxHost.Text + " : " + textBoxMessage.Text + "\n");
                }
            }
            catch (Exception exc)
            {
                Log.Items.Add(exc.Message + "\n");
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            int count = 15;
            while (count < 10)
            {
                //
                if (socket.Available > 0)
                {
                    try
                    {
                        EndPoint dataSender = new IPEndPoint(IPAddress.Any, 0);

                        byte[] data = new byte[10000];

                        int bytesRead = socket.ReceiveFrom(data, ref dataSender);

                        if (bytesRead > 0)
                        {
                            string recievedText = Encoding.UTF8.GetString(data, 0, bytesRead);

                            Log.Items.Add("Me" + " : " + recievedText + "\n");
                        }
                    }
                    catch (Exception exc)
                    {
                        Log.Items.Add(exc.Message + "\n");
                    }
                }

                //

            }
        }
    }
}
