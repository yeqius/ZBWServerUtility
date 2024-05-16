using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using static WWWReadServer;

public class WWWTest : MonoBehaviour
{
   // public WebRequestExample example;

    // 服务器的IP地址和端口
    public string serverAddress = "服务器IP地址";
    public int serverPort = 12345;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(ClientMain.Instance.wwwReadServer.GetCurrinstance());

        // 创建TCP客户端并连接到服务器
        TcpClient client = new TcpClient(serverAddress, serverPort);
        Debug.Log("已连接到服务器...");

        // 向服务器发送请求执行 start.bat 脚本
        string request = "C:\\Users\\Administrator\\Desktop\\frps\\start\\start.bat"; // 服务器上 start.bat 脚本的完整路径
        byte[] requestData = Encoding.ASCII.GetBytes(request);
        NetworkStream stream = client.GetStream();
        stream.Write(requestData, 0, requestData.Length);
        Debug.Log("已向服务器发送请求执行脚本：" + request);

        // 关闭连接
        client.Close();
        Debug.Log("连接已关闭.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Set(TencentCloudInstance value)
    {
        
    }



    //// PHP 服务器的 URL
    //string url = "http://192.168.3.124/ZBWServerWebUtility/OpenWeb.php";
    //// 创建HTTP请求
    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

    //// 允许不安全的连接
    //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

    //// 发送请求并获取响应
    //using (WebResponse response = request.GetResponse())
    //{
    //    using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream()))
    //    {
    //        string jsonResponse = reader.ReadToEnd();
    //        Debug.Log("Response: " + jsonResponse);
    //    }
    //}
    //yield return  null;
}
