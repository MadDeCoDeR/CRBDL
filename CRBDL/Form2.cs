/*
Classic RBDoom 3 BFG Edition Launcher

Copyright(C) 2019 George Kalmpokis

Permission is hereby granted, free of charge, to any person obtaining a copy of this software
and associated documentation files(the "Software"), to deal in the Software without
restriction, including without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following conditions :

The above copyright notice and this permission notice shall be included in all copies or substantial
portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using CRBDL;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CDL
{
    public partial class Form2 : Form
    {
        private static string[] addr;
        //private static bool endt;
        TcpListener serverSocket = null;
        TcpClient[] clientSocket;
        private bool abort = false;
        private string ipaddr;
        IPAddress ip;
        Thread th;
        private int maxplayers = -1;
        private int pointer = -1;
        private Form1 form1;
        public Form2(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            addr = new string[4];
            maskedTextBox1.ValidatingType = typeof(System.Net.IPAddress);
            button1.Enabled = false;
            clientSocket = new TcpClient[3];
            comboBox2.SelectedIndex = 0;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            label4.Enabled = false;
            label6.Enabled = false;
            label8.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked || checkBox3.Checked)
            {
                if (checkBox2.Checked)
                {
                    form1.adcoms[comboBox1.SelectedIndex] += "-deathmatch ";
                }
                else
                {
                    form1.adcoms[comboBox1.SelectedIndex] += "-altdeath ";
                }
                if (textBox1.Text.Length > 0)
                {
                    form1.adcoms[comboBox1.SelectedIndex] += "-timer " + textBox1.Text + " ";
                }
                if (textBox2.Text.Length > 0)
                {
                    form1.adcoms[comboBox1.SelectedIndex] += "-fraglimit " + textBox2.Text + " ";
                }
            }
            form1.Launchgame();
        }

        private string GetLocalIPAddress()
        {
            string Addresses = string.Empty;
            NetworkInterface[] allNICs = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var nic in allNICs)
            {
                var ipProp = nic.GetIPProperties();
                var gwAddresses = ipProp.GatewayAddresses;
                if (nic.OperationalStatus == OperationalStatus.Up && nic.Speed > 0 && gwAddresses.Count > 0)
                {
                    IPAddress localIP = ipProp.UnicastAddresses.First(d => d.Address.AddressFamily == AddressFamily.InterNetwork).Address;
                    return localIP.ToString();
                }
            }
            // throw new Exception("No network adapters with an IPv4 address in the system!");
            return Addresses;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            maxplayers = Convert.ToInt32(comboBox2.GetItemText(comboBox2.Items[comboBox2.SelectedIndex]));

            ipaddr = string.Empty;
            for (int i = 0; i < maskedTextBox1.Text.Length; i++)
            {
                if (maskedTextBox1.Text[i] != ' ')
                {
                    ipaddr += maskedTextBox1.Text[i];
                }
            }
            if (!checkBox1.Checked && !IPAddress.TryParse(ipaddr, out ip))
                return;
            comboBox2.Enabled = false;
            checkBox1.Enabled = false;
            comboBox1.Enabled = false;
            maskedTextBox1.Enabled = false;
            //endt = false;
            button2.Enabled = false;
            pointer = comboBox1.SelectedIndex;
            form1.adcoms[pointer] = "-net ";
            if (checkBox1.Checked)
            {
                form1.adcoms[pointer] += "0 " + GetLocalIPAddress() + " ";
                th = new Thread(startServer);
                th.IsBackground = true;
                th.Start();
                // Thread.Sleep(1000);
                //th.Abort();
                //startServer();
            }
            else
            {
                th = new Thread(startClient);
                th.IsBackground = true;
                th.Start();

                //Thread.Sleep(1000);
                //startClient();
            }
        }

        private void startServer()
        {
            serverSocket = new TcpListener(IPAddress.Any, 6666);
            int requestCount = 0;
            for (int i = 0; i < 3; i++)
            {
                clientSocket[i] = default(TcpClient);
            }
            serverSocket.Start();
            int startcount = 1;
            int sendcount = 1;

            while ((true))
            {
                if (abort)
                    break;
                try
                {
                    byte[] bytesFrom = new byte[10025];
                    string dataFromClient;
                    string serverResponse;
                    if (startcount < maxplayers)
                    {
                        requestCount = requestCount + 1;
                        clientSocket[startcount - 1] = serverSocket.AcceptTcpClient();
                        NetworkStream networkStream = clientSocket[startcount - 1].GetStream();
                        networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                        dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                        dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("IP"));
                        addr[requestCount] = dataFromClient;
                        form1.adcoms[pointer] += dataFromClient + " ";
                        serverResponse = Convert.ToString(startcount);
                        Byte[] sendBytes = System.Text.Encoding.ASCII.GetBytes(serverResponse);
                        networkStream.Write(sendBytes, 0, sendBytes.Length);
                        networkStream.Flush();
                        startcount++;
                    }
                    else
                    {
                        NetworkStream networkStream = clientSocket[sendcount - 1].GetStream();
                        serverResponse = Convert.ToString(true);
                        Byte[] sendBytes = System.Text.Encoding.ASCII.GetBytes(serverResponse);
                        networkStream.Write(sendBytes, 0, sendBytes.Length);
                        networkStream.Flush();
                        Array.Clear(bytesFrom, 0, bytesFrom.Length);
                        networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                        dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                        int client = Convert.ToInt32(dataFromClient);
                        for (int j = 1; j < maxplayers; j++)
                        {
                            if (j != client)
                            {
                                string sip = addr[j];
                                Array.Clear(sendBytes, 0, sendBytes.Length);
                                sendBytes = Encoding.ASCII.GetBytes(sip);
                                networkStream.Write(sendBytes, 0, sendBytes.Length);
                                networkStream.Flush();
                            }
                        }
                        clientSocket[sendcount - 1].Close();
                        sendcount++;
                    }

                    if (sendcount == maxplayers)
                        break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            if (!abort)
            {
                //clientSocket.Close();
                serverSocket.Stop();
                Invoke((MethodInvoker)(() => label2.Text = "Found Players"));
                Invoke((MethodInvoker)(() => checkBox1.Enabled = true));
                Invoke((MethodInvoker)(() => comboBox1.Enabled = true));
                Invoke((MethodInvoker)(() => maskedTextBox1.Enabled = true));
                //endt = true;
                Invoke((MethodInvoker)(() => button2.Enabled = true));
                Invoke((MethodInvoker)(() => button1.Enabled = true));
                Invoke((MethodInvoker)(() => comboBox2.Enabled = true));
            }

        }

        private void startClient()
        {
            clientSocket[0] = new TcpClient();
            clientSocket[0].Connect(ip, 6666);
            NetworkStream serverStream = clientSocket[0].GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(GetLocalIPAddress() + "IP");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
            byte[] inStream = new byte[10025];
            serverStream.Read(inStream, 0, inStream.Length);
            string returndata = System.Text.Encoding.ASCII.GetString(inStream);
            int player = Convert.ToInt32(returndata);
            form1.adcoms[pointer] += player + " " + ip.ToString() + " ";
            Array.Clear(inStream, 0, inStream.Length);
            serverStream.Read(inStream, 0, inStream.Length);
            returndata = System.Text.Encoding.ASCII.GetString(inStream);
            bool cont = Convert.ToBoolean(returndata);
            string outbuf = Convert.ToString(player);
            Array.Clear(outStream, 0, outStream.Length);
            outStream = System.Text.Encoding.ASCII.GetBytes(outbuf);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
            for (int i = 1; i < maxplayers; i++)
            {
                if (i == player)
                {
                    form1.adcoms[pointer] += GetLocalIPAddress() + " ";
                }
                else
                {
                    Array.Clear(inStream, 0, inStream.Length);
                    serverStream.Read(inStream, 0, inStream.Length);
                    returndata = System.Text.Encoding.ASCII.GetString(inStream);
                    form1.adcoms[pointer] += returndata + " ";
                }
            }
            Invoke((MethodInvoker)(() => label2.Text = "Found Players"));
            Invoke((MethodInvoker)(() => checkBox1.Enabled = true));
            Invoke((MethodInvoker)(() => comboBox1.Enabled = true));
            Invoke((MethodInvoker)(() => maskedTextBox1.Enabled = true));
            //endt = true;
            Invoke((MethodInvoker)(() => button2.Enabled = true));
            Invoke((MethodInvoker)(() => button1.Enabled = true));
            Invoke((MethodInvoker)(() => comboBox2.Enabled = true));
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            label5.Enabled = !checkBox1.Checked;
            maskedTextBox1.Enabled = !checkBox1.Checked;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            abort = true;
            for (int i = 0; i < 3; i++)
            {
                if (clientSocket[i] != null)
                {
                    if (clientSocket[i].Connected)
                    {
                        clientSocket[i].Close();
                    }
                    else
                    {
                        th.Abort();
                    }

                    clientSocket[i] = null;
                }
            }
            if (serverSocket != null)
            {
                serverSocket.Stop();
                serverSocket = null;
            }
            for (int i = 0; i < 3; i++)
            {
                form1.adcoms[i] = "";
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = checkBox2.Checked;
            textBox2.Enabled = checkBox2.Checked;
            label4.Enabled = checkBox2.Checked;
            label6.Enabled = checkBox2.Checked;
            label8.Enabled = checkBox2.Checked;
            checkBox3.Enabled = !checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = checkBox3.Checked;
            textBox2.Enabled = checkBox3.Checked;
            label4.Enabled = checkBox3.Checked;
            label6.Enabled = checkBox3.Checked;
            label8.Enabled = checkBox3.Checked;
            checkBox2.Enabled = !checkBox3.Checked;
        }
    }
}
