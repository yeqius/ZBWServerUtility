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

    // ��������IP��ַ�Ͷ˿�
    public string serverAddress = "������IP��ַ";
    public int serverPort = 12345;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(ClientMain.Instance.wwwReadServer.GetCurrinstance());

        // ����TCP�ͻ��˲����ӵ�������
        TcpClient client = new TcpClient(serverAddress, serverPort);
        Debug.Log("�����ӵ�������...");

        // ���������������ִ�� start.bat �ű�
        string request = "C:\\Users\\Administrator\\Desktop\\frps\\start\\start.bat"; // �������� start.bat �ű�������·��
        byte[] requestData = Encoding.ASCII.GetBytes(request);
        NetworkStream stream = client.GetStream();
        stream.Write(requestData, 0, requestData.Length);
        Debug.Log("�����������������ִ�нű���" + request);

        // �ر�����
        client.Close();
        Debug.Log("�����ѹر�.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Set(TencentCloudInstance value)
    {
        
    }



    //// PHP �������� URL
    //string url = "http://192.168.3.124/ZBWServerWebUtility/OpenWeb.php";
    //// ����HTTP����
    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

    //// ������ȫ������
    //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

    //// �������󲢻�ȡ��Ӧ
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
