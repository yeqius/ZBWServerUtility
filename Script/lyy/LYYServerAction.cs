using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;

public class LYYServerAction : MonoBehaviour
{
    private string ipAddressCurrent;
    // Start is called before the first frame update
    void Start()
    {
        // 获取本地计算机的IP地址
        ipAddressCurrent = GetLocalIPAddress();

        // 输出IP地址
        Debug.Log("Local IP Address: " + ipAddressCurrent);

    }

    // Update is called once per frame
    void Update()
    {
        
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
