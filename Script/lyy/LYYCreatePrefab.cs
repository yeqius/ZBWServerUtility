
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
        // 设置以管理员身份运行
        process.StartInfo.Verb = "runas";
        process.StartInfo.FileName = batFilePaths;

        //创建并编辑bat

        string batFileContent = @"
    @echo off
    setlocal
    set ports= 2100 21003 1206 1532
    for %%p in (%ports%) do (
      netsh advfirewall firewall add rule name=""Open Port %%p"" dir=in action=allow protocol=UDP localport=%%p
      echo ""端口 %%p 已成功开放。""
    )
    ";
        System.IO.File.WriteAllText(batFilePaths, batFileContent);



        // 读取.bat文件中的内容
        string[] lines = System.IO.File.ReadAllLines(batFilePaths);

        // 替换.bat文件中的端口号为新的端口号
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
                break; // 找到并替换后退出循环
            }
        }

        // 将修改后的内容重新写入.bat文件
        System.IO.File.WriteAllLines(batFilePaths, lines);

        // 启动进程
        process.Start();
        // 等待进程结束
        process.WaitForExit();
        //打开端口


        var portPath = "C:\\toolText\\tool";
        //portPath = portPaths.text;
        // 输出命令执行结果
        // print(outPut);
        var copyPath = portPath + "\\copy.bat";
        //copyPath = copyPaths.text;

        // 读取.bat文件中的内容
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

                    // 启动进程
                    processCopy.Start();
                    // 等待进程结束
                    processCopy.WaitForExit();
                }

            }
        }
        //复制frp文件
        //没用了
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
                     // 将修改后的内容重新写入.bat文件
                     System.IO.File.WriteAllLines(copyPath, copyLines);

                     Process processCopy = new Process();
                     processCopy.StartInfo.FileName = copyPath;

                     // 启动进程
                     processCopy.Start();
                     // 等待进程结束
                     processCopy.WaitForExit();
                     setPRotValue = threeValue;

                 }
                 UnityEngine.Debug.LogError(setPRotValue);

                 copyLines[i] = setPRotValue;
             }
         }*/



        //修改port


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

                    // 启动进程
                    startBat.Start();

                    processList.Add(startBat);
                    // 等待进程结束
                    //startBat.WaitForExit();
                    UnityEngine.Debug.LogError(startBatPath);
                    //  portPath = otherValue;

                }
            }
        }



        // string batPath = "C:\\FRP\\frp\\start.bat";

        /* process = new Process();
         process.StartInfo.FileName = batPath;
         process.StartInfo.UseShellExecute = false; // 设置为 false
         process.StartInfo.CreateNoWindow = true; // 创建不显示窗口
         process.Start();
         UnityEngine.Debug.LogError("打开");*/

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
                process.Kill(); // 终止进程
                process.Close();
                process.Dispose();
                UnityEngine.Debug.LogError("关闭");
            }
        }

    }
    string GetLocalIPAddress()
    {
        string ipAddress = "";

        // 获取本地计算机的所有网络接口
        NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface networkInterface in networkInterfaces)
        {
            // 排除虚拟接口和非活动接口
            if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                networkInterface.OperationalStatus == OperationalStatus.Up)
            {
                // 获取IPv4地址
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
