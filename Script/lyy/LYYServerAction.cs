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
        // ��ȡ���ؼ������IP��ַ
        ipAddressCurrent = GetLocalIPAddress();

        // ���IP��ַ
        Debug.Log("Local IP Address: " + ipAddressCurrent);

    }

    // Update is called once per frame
    void Update()
    {
        
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
