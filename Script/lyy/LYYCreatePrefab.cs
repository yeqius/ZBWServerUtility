
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using System.Linq;
using System.Net.NetworkInformation;

public class LYYCreatePrefab : MonoBehaviour
{
    private int port;
    private string ip;
    public int num = 1000;
    private int acturyNum = 0;
    private List<int> remotePortList = new List<int>();
    private List<int> portAvailableList = new List<int>();
    public GameObject gameobject;
    private int remotePort = 6000;
    private int portLenthValue = 0;
    private List<Process> processList = new List<Process>();
    //public Text client;
    public Text server;
    private int connectNum = 0;
    private int addNum = 0;
    private int backNum = 0;
    private int connectServerNum = 0;
   /* public Text russPaths;
    public Text copyPaths;
    public Text portPaths;
    public Text numString;
    public Text panString;
    public Text startNumString;
    public Text remoatPortStartString;*/
    private int startNum;
    // Start is called before the first frame update
    void Start()
    {
        startCmd();

    }
    public void Update()
    {
       // client.text = "Connect ToServer " + connectServerNum + "   " + " backNum " + backNum;
        server.text = "Connect " + connectNum + "   AddNum" + addNum;
    }
    public void startCmd()
    {
        ip = GetLocalIPAddress();
        var optionFilePaths = "C:\\toolText\\tool\\option.txt";
        string[] optionLines = System.IO.File.ReadAllLines(optionFilePaths);
        for (int i = 0; i < optionLines.Length; i++)
        {
            if (optionLines[i].Contains("count ="))
            {
                var numString = optionLines[i];
                var stringArray = numString.Split("=");
                num = int.Parse(stringArray[1]);
                UnityEngine.Debug.LogError(num);
            }
            if(optionLines[i].Contains("remotePort ="))
            {
                var startNumString = optionLines[i];
                var stringArray = startNumString.Split("=");
                remotePort = int.Parse(stringArray[1]);
                startNum = remotePort;
                UnityEngine.Debug.LogError(remotePort);
            }
         

        }


     /*   num = int.Parse(numString.text);
        startNum = int.Parse(startNumString.text);
        remotePort = int.Parse(remoatPortStartString.text);*/
        for (int i = 1200; i < 8000; i++)
        {
            port = i;
            try
            {
                IPAddress ipAddress = IPAddress.Parse(ip);
                TcpListener listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                listener.Stop();
                UnityEngine.Debug.Log($"Port {port} is available.");
                portAvailableList.Add(i);
            }
            catch (SocketException)
            {
                UnityEngine.Debug.Log($"Port {port} is in use.");
            }
            if (portAvailableList.Count >= num)
            {
                break;
            }

        }
        var batFilePaths = "C:\\toolText\\tool\\runss.bat";
        //batFilePaths = russPaths.text;


        Process process = new Process();
        // �����Թ���Ա�������
        process.StartInfo.Verb = "runas";
        process.StartInfo.FileName = batFilePaths;

        //�������༭bat

        string batFileContent = @"
    @echo off
    setlocal
    set ports= 2100 21003 1206 1532
    for %%p in (%ports%) do (
      netsh advfirewall firewall add rule name=""Open Port %%p"" dir=in action=allow protocol=UDP localport=%%p
      echo ""�˿� %%p �ѳɹ����š�""
    )
    ";
        System.IO.File.WriteAllText(batFilePaths, batFileContent);



        // ��ȡ.bat�ļ��е�����
        string[] lines = System.IO.File.ReadAllLines(batFilePaths);

        // �滻.bat�ļ��еĶ˿ں�Ϊ�µĶ˿ں�
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("set ports="))
            {

                var setPRotValue = "set ports=";
                foreach (var item in portAvailableList)
                {
                    setPRotValue += " " + item;
                }
                UnityEngine.Debug.LogError(setPRotValue);
                lines[i] = setPRotValue;
                break; // �ҵ����滻���˳�ѭ��
            }
        }

        // ���޸ĺ����������д��.bat�ļ�
        System.IO.File.WriteAllLines(batFilePaths, lines);

        // ��������
        process.Start();
        // �ȴ����̽���
        process.WaitForExit();
        //�򿪶˿�


        var portPath = "C:\\toolText\\tool";
        //portPath = portPaths.text;
        // �������ִ�н��
        // print(outPut);
        var copyPath = portPath + "\\copy.bat";
        //copyPath = copyPaths.text;

        // ��ȡ.bat�ļ��е�����
        string[] copyLines = System.IO.File.ReadAllLines(copyPath);
        for (int j = 1; j <= num; j += 100)
        {
            var endJ = j + 99;
            for (int k = j; k <= endJ; k += 10)
            {
                var endK = k + 9;
                for (int l = k; l <= endK; l++)
                {
                    for (int i = 0; i < copyLines.Length; i++)
                    {
                        if (copyLines[i].Contains("set destinationFolder="))
                        {
                            copyLines[i] = "set destinationFolder=" + portPath + "\\" + j + "-" + endJ + "\\" + k + "-" + endK + "\\" + l;
                        }
                    }
                    System.IO.File.WriteAllLines(copyPath, copyLines);

                    Process processCopy = new Process();
                    processCopy.StartInfo.FileName = copyPath;

                    // ��������
                    processCopy.Start();
                    // �ȴ����̽���
                    processCopy.WaitForExit();
                }

            }
        }
        //����frp�ļ�
        //û����
        /* for (int i = 0; i < copyLines.Length; i++)
         {
             if (copyLines[i].Contains("set destinationFolder="))
             {

                 var setPRotValue = "set destinationFolder=D:\\frp\\1-100";
                 var twoOtherValue = "";
                 twoOtherValue = "1-10";
                 setPRotValue += "\\" + twoOtherValue;
                 var threeValue = setPRotValue;
                 for (int k = 1; k <= 10; k++)
                 {
                     setPRotValue += "\\" + k;
                     copyLines[i] = setPRotValue;
                     // ���޸ĺ����������д��.bat�ļ�
                     System.IO.File.WriteAllLines(copyPath, copyLines);

                     Process processCopy = new Process();
                     processCopy.StartInfo.FileName = copyPath;

                     // ��������
                     processCopy.Start();
                     // �ȴ����̽���
                     processCopy.WaitForExit();
                     setPRotValue = threeValue;

                 }
                 UnityEngine.Debug.LogError(setPRotValue);

                 copyLines[i] = setPRotValue;
             }
         }*/



        //�޸�port


        // var otherValue = portPath;
        for (int j = 1; j <= num; j += 100)
        {
            var endJ = j + 99;
            var newPath = portPath + "\\" + j + "-" + endJ + "\\";
            for (int k = j; k <= endJ; k += 10)
            {
                var endK = k + 9;
                var newPathK = newPath + k + "-" + endK + "\\";
                for (int l = k; l <= endK; l++)
                {
                    var newPathL = newPathK + l;
                    startNum++;
                    var frpPath = newPathL + "\\frp";
                    var startBatPath = frpPath + "\\start.bat";
                    var frpcTomlPath = frpPath + "\\frpc.toml";

                    // UnityEngine.Debug.LogError(portPath);
                    string[] portLines = System.IO.File.ReadAllLines(frpcTomlPath);
                    for (int i = 0; i < portLines.Length; i++)
                    {
                        if (portLines[i].Contains("remotePort ="))
                        {
                            
                            portLines[i] = "remotePort = " + remotePort;
                            remotePortList.Add(remotePort);
                            remotePort++;
                        }
                        if (portLines[i].Contains("localPort ="))
                        {
                            portLines[i] = "localPort = " + portAvailableList[portLenthValue];
                            portLenthValue++;
                        }
                        if (portLines[i].Contains("name ="))
                        {
                            portLines[i] = "name = \"test-udp" + startNum + "\"";
                        }
                        if (portLines[i].Contains("localIP = "))
                        {
                            portLines[i] = "localIP = " + "\"" + ip + "\"";
                        }
                    }
                    System.IO.File.WriteAllLines(frpcTomlPath, portLines);
                    string[] startbatFileContent = new string[4];
                    for (int i = 0; i < startbatFileContent.Length; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                startbatFileContent[i] = "@echo off";
                                break;
                            case 1:
                                startbatFileContent[i] = "C:";
                                break;
                            case 2:
                                startbatFileContent[i] = "cd /d " + frpPath;
                                break;
                            case 3:
                                startbatFileContent[i] = "frpc.exe -c frpc.toml";
                                break;
                            default:
                                break;
                        }

                    }
                    System.IO.File.WriteAllLines(startBatPath, startbatFileContent);
                    Process startBat = new Process();
                    startBat.StartInfo.FileName = startBatPath;
                    startBat.StartInfo.UseShellExecute = false;
                    startBat.StartInfo.CreateNoWindow = true;

                    // ��������
                    startBat.Start();

                    processList.Add(startBat);
                    // �ȴ����̽���
                    //startBat.WaitForExit();
                    UnityEngine.Debug.LogError(startBatPath);
                    //  portPath = otherValue;

                }
            }
        }



        // string batPath = "C:\\FRP\\frp\\start.bat";

        /* process = new Process();
         process.StartInfo.FileName = batPath;
         process.StartInfo.UseShellExecute = false; // ����Ϊ false
         process.StartInfo.CreateNoWindow = true; // ��������ʾ����
         process.Start();
         UnityEngine.Debug.LogError("��");*/

        for (int i = 0; i < portAvailableList.Count; i++)
        {
            var item = portAvailableList[i];
            //var remoatItem = remotePortList[i];
            var udpObject = Instantiate(gameobject);
            //var client = udpObject.GetComponent<LYYUDPClient>();
            var server = udpObject.GetComponent<LYYUDPServer>();
            server.SetServerPort(item, this);
            //client.SetClientPort(remoatItem, this);
            server.StartAction(ip);
           //client.StartAction();
        }

    }
    public void ConnectNumAdd()
    {
        connectNum++;
    }
    public void AddNumAdd()
    {
        addNum++;
    }
    public void BackNumAdd()
    {
        backNum++;
    }
    public void ConnectServerNumAdd()
    {
        connectServerNum++;
    }
    public void GetIP()
    {
      //  num = int.Parse(numString.text);
        UnityEngine.Debug.LogError(num);
    }
    public void OnDisable()
    {
        foreach (var process in processList)
        {
            if (process != null && !process.HasExited)
            {
                process.Kill(); // ��ֹ����
                process.Close();
                process.Dispose();
                UnityEngine.Debug.LogError("�ر�");
            }
        }

    }
    string GetLocalIPAddress()
    {
        string ipAddress = "";

        // ��ȡ���ؼ��������������ӿ�
        NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface networkInterface in networkInterfaces)
        {
            // �ų�����ӿںͷǻ�ӿ�
            if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                networkInterface.OperationalStatus == OperationalStatus.Up)
            {
                // ��ȡIPv4��ַ
                foreach (UnicastIPAddressInformation ipInfo in networkInterface.GetIPProperties().UnicastAddresses)
                {
                    if (ipInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddress = ipInfo.Address.ToString();
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(ipAddress))
            {
                break;
            }
        }

        return ipAddress;
    }

}





// Update is called once per frame
