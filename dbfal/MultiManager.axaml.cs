using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;

namespace dbfal;

public partial class MultiManager : Window
{
    private static string[]? addr;
    //private static bool endt;
    TcpListener? serverSocket = null;
    TcpClient[]? clientSocket = null;
    private bool abort = false;
    private string? ipaddr;
    IPAddress? ip;
    Thread? th;
    private int maxplayers = -1;
    private int pointer = -1;
    private MainWindow form1;
    public static bool HasValidIP = false;

    public MultiManager()
    {
        InitializeComponent();
        this.form1 = new MainWindow();
    }

    public MultiManager(MainWindow mainWindow)
    {
        InitializeComponent();
        this.form1 = mainWindow;

    }

    private void MultiManager_Load(object? sender, RoutedEventArgs e)
    {
        GameSelection.SelectedIndex = 0;
        addr = new string[4];
        MPLaunch.IsEnabled = true;
        clientSocket = new TcpClient[3];
        NumOfPlayers.SelectedIndex = 0;
        TimeLimit.IsEnabled = false;
        FragLimit.IsEnabled = false;
        FragLimitGroup.IsEnabled = false;
        TimeLimitGroup.IsEnabled = false;
        HostToogle.IsEnabled = false;
        IPHostLabel.IsEnabled = false;
        IPHostText.IsEnabled = false;
        PlayerFoundLabel.IsEnabled = false;
        PlayerLookupButton.IsEnabled = false;
    }

    private void LaunchWithMP(object sender, RoutedEventArgs e)
    {
        if (NewDM.IsChecked!.Value || OldDM.IsChecked!.Value)
        {
            if (NewDM.IsChecked!.Value)
            {
                form1.adcoms[GameSelection.SelectedIndex] += "-deathmatch ";
            }
            else
            {
                form1.adcoms[GameSelection.SelectedIndex] += "-altdeath ";
            }
            if (TimeLimit.Value > 0)
            {
                form1.adcoms[GameSelection.SelectedIndex] += "-timer " + TimeLimit.Value!.ToString() + " ";
            }
            if (FragLimit.Value > 0)
            {
                form1.adcoms[GameSelection.SelectedIndex] += "-fraglimit " + FragLimit.Value!.ToString() + " ";
            }
        }

        if (NumOfPlayers.SelectedIndex == 0)
        {
            form1.adcoms[GameSelection.SelectedIndex] += "-net 0 127.0.0.1 ";
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

    private void PlayerLookupButton_Click(object sender, RoutedEventArgs e)
    {
        if (NumOfPlayers.SelectedValue == null) { return; }
        ComboBoxItem playersItem = (ComboBoxItem)NumOfPlayers.SelectedValue;
        maxplayers = Convert.ToInt32(playersItem.Content!.ToString());
        if (!HasValidIP && (HostToogle.IsChecked != null && HostToogle.IsChecked == false))
        {
            return;
        }

        ipaddr = string.Empty;
        if (IPHostText.Text != null)
        {
            for (int i = 0; i < IPHostText.Text!.Length; i++)
            {
                if (IPHostText.Text[i] != ' ')
                {
                    ipaddr += IPHostText.Text[i];
                }
            }
        }
        if (!HostToogle.IsChecked!.Value && !IPAddress.TryParse(ipaddr, out ip))
            return;
        NumOfPlayers.IsEnabled = false;
        HostToogle.IsEnabled = false;
        GameSelection.IsEnabled = false;
        IPHostText.IsEnabled = false;
        //endt = false;
        PlayerLookupButton.IsEnabled = false;
        pointer = GameSelection.SelectedIndex;
        form1.adcoms[pointer] = "-net ";
        if (HostToogle.IsChecked!.Value)
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

    private void ResetUIFromPlayerLookup()
    {
        PlayerFoundLabel.Content = "Found Players";
        HostToogle.IsEnabled = true;
        GameSelection.IsEnabled = true;
        IPHostText.IsEnabled = true;
        PlayerLookupButton.IsEnabled = true;
        MPLaunch.IsEnabled = true;
        NumOfPlayers.IsEnabled = true;
    }

    private void startServer()
    {
        serverSocket = new TcpListener(IPAddress.Any, 6666);
        int requestCount = 0;
        if (clientSocket == null)
        {
            return;
        }
        if (addr == null) { return; }
        /*for (int i = 0; i < 3; i++)
        {
            clientSocket[i] = default;
        }*/
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
                    networkStream.ReadExactly(bytesFrom);
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
                    networkStream.ReadExactly(bytesFrom);
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
            this.Dispatcher.Invoke(new Action(ResetUIFromPlayerLookup));
            //endt = true;
        }

    }

    private void startClient()
    {
        if (clientSocket == null) {
            return;
        }
        if (ip == null) {
            return;
        }
        clientSocket[0] = new TcpClient();
        clientSocket[0].Connect(ip, 6666);
        NetworkStream serverStream = clientSocket[0].GetStream();
        byte[] outStream = System.Text.Encoding.ASCII.GetBytes(GetLocalIPAddress() + "IP");
        serverStream.Write(outStream, 0, outStream.Length);
        serverStream.Flush();
        byte[] inStream = new byte[10025];
        serverStream.ReadExactly(inStream);
        string returndata = System.Text.Encoding.ASCII.GetString(inStream);
        int player = Convert.ToInt32(returndata);
        form1.adcoms[pointer] += player + " " + ip.ToString() + " ";
        Array.Clear(inStream, 0, inStream.Length);
        serverStream.ReadExactly(inStream);
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
                serverStream.ReadExactly(inStream);
                returndata = System.Text.Encoding.ASCII.GetString(inStream);
                form1.adcoms[pointer] += returndata + " ";
            }
        }
        this.Dispatcher.Invoke(new Action(ResetUIFromPlayerLookup));
    }

    private void IsHostCheckChanged(object sender, RoutedEventArgs e)
    {
        IPHostLabel.IsEnabled = !HostToogle.IsChecked!.Value;
        IPHostText.IsEnabled = !HostToogle.IsChecked!.Value;
    }

    private void ClosingMultiManager(object sender, WindowClosingEventArgs e)
    {
        abort = true;
        if (clientSocket != null)
        {
            for (int i = 0; i < 3; i++)
            {
                if (clientSocket[i] != null)
                {
                    if (clientSocket[i].Connected)
                    {
                        clientSocket[i].Close();
                    }
                }
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

    private void NewDMCheckChanged(object sender, RoutedEventArgs e)
    {
        TimeLimit.IsEnabled = NewDM.IsChecked!.Value;
        FragLimit.IsEnabled = NewDM.IsChecked!.Value;
        FragLimitGroup.IsEnabled = NewDM.IsChecked!.Value;
        TimeLimitGroup.IsEnabled = NewDM.IsChecked!.Value;
        OldDM.IsEnabled = !NewDM.IsChecked!.Value;
    }

    private void OldDMCheckChanged(object sender, RoutedEventArgs e)
    {
        TimeLimit.IsEnabled = OldDM.IsChecked!.Value;
        FragLimit.IsEnabled = OldDM.IsChecked!.Value;
        FragLimitGroup.IsEnabled = OldDM.IsChecked!.Value;
        TimeLimitGroup.IsEnabled = OldDM.IsChecked!.Value;
        NewDM.IsEnabled = !OldDM.IsChecked!.Value;
    }

    private void PlayersNumberChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (NumOfPlayers == null)
        {
            return;
        }
        if (NumOfPlayers.SelectedIndex == 0)
        {
            HostToogle.IsEnabled = false;
            IPHostLabel.IsEnabled = false;
            IPHostText.IsEnabled = false;
            PlayerFoundLabel.IsEnabled = false;
            PlayerLookupButton.IsEnabled = false;
            MPLaunch.IsEnabled = true;
        } else
        {
            HostToogle.IsEnabled = true;
            IPHostLabel.IsEnabled = true;
            IPHostText.IsEnabled = true;
            PlayerFoundLabel.IsEnabled = true;
            PlayerLookupButton.IsEnabled = true;
            MPLaunch.IsEnabled = false;
        }
    }
}