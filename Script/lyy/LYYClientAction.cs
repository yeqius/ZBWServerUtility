using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class LYYClientAction : MonoBehaviour
{
    public int num;
    public GameObject clientStyle;
    public LYYCreatePrefab createPrefab;
    public int remotePoteStart;
    public Text clientText;
    private int connectToServerNum = 0;
    private int getbackNum = 0;
    private int disconnectNum = 0;
    // Start is called before the first frame update
    void Start()
    {

        var optionFilePaths = "C:\\Client\\option.txt";
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
            if (optionLines[i].Contains("startPort ="))
            {
                var startNumString = optionLines[i];
                var stringArray = startNumString.Split("=");
                remotePoteStart = int.Parse(stringArray[1]);
            }


        }
        for (int i = 0; i < num; i++)
        {
            var remoatItem = remotePoteStart + i;
            var udpObject = Instantiate(clientStyle);
            var client = udpObject.GetComponent<LYYUDPClient>();
            client.SetClientPort(remoatItem, createPrefab, this);
        }

    }
    // Update is called once per frame
    void Update()
    {
        clientText.text = "CliConnectToSerNum  " + connectToServerNum + "  GetBackNum " + getbackNum + " DisconnectNum " + disconnectNum;
    }
    public void AddConnectServerNum()
    {
        connectToServerNum++;
    }
    public void AddBackNum()
    {
        getbackNum++;

    }
    public void AddDisconnectNum()
    {
        disconnectNum++;

    }

}
