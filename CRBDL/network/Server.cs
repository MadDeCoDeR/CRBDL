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

//TODO
using CRBDL;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CDL.network
{
    class Server
    {
        TcpListener serverSocket = null;
        private bool abort = false;
        private int maxplayers = -1;
        List<TcpClient> clientSocket = new List<TcpClient>();
        private Form1 form1;
        private int pointer = -1;
        private static List<string> addr = new List<string>();
        //private Form2 form2;

        public Server(Form1 form1)
        {
            this.form1 = form1;
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
            //if (!abort)
            //{
            //    //clientSocket.Close();
            //    serverSocket.Stop();
            //    Invoke((MethodInvoker)(() => form2.GetLabel2().Text = "Found Players"));
            //    Invoke((MethodInvoker)(() => form2.GetCheckBox1().Enabled = true));
            //    Invoke((MethodInvoker)(() => form2.GetComboBox1().Enabled = true));
            //    Invoke((MethodInvoker)(() => form2.GetMaskedTextBox1().Enabled = true));
            //    endt = true;
            //    Invoke((MethodInvoker)(() => button2.Enabled = true));
            //    Invoke((MethodInvoker)(() => button1.Enabled = true));
            //    Invoke((MethodInvoker)(() => comboBox2.Enabled = true));
            //}

        }

    }
}
