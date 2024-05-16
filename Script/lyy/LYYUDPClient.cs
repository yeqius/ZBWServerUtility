using UnityEngine;
using Unity.Networking.Transport;

public class LYYUDPClient : MonoBehaviour
{
    NetworkDriver m_Driver;
    NetworkConnection m_Connection;
    public NetworkEndpoint m_ServerEndPoint;
    public int pornet;
    private bool action;
    private LYYCreatePrefab mainScript;
    private LYYClientAction clientManager;
    private float time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        /* m_Driver = NetworkDriver.Create();
         m_ServerEndPoint = NetworkEndpoint.Parse("82.156.76.152",(ushort) pornet);//6000
         //var endpoint = NetworkEndpoint.LoopbackIpv4.WithPort(7777);
         m_Connection = m_Driver.Connect(m_ServerEndPoint);*/
    }

    // Update is called once per frame
    void Update()
    {
        if (!action)
        {
            return;
        }

        time += Time.deltaTime;
        if (time > 1f)
        {
            time = 0;

            m_Driver.ScheduleUpdate().Complete();

            if (!m_Connection.IsCreated)
            {
                return;
            }
            Unity.Collections.DataStreamReader stream;
            NetworkEvent.Type cmd;
            while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Connect)
                {
                    Debug.Log("We are now connected to the server.");
                    mainScript.ConnectServerNumAdd();
                    clientManager.AddConnectServerNum();
                    //text.text += "We are now connected to the server.";
                    uint value = 1;
                    m_Driver.BeginSend(m_Connection, out var writer);
                    writer.WriteUInt(value);
                    m_Driver.EndSend(writer);
                }
                else if (cmd == NetworkEvent.Type.Data)
                {
                    uint value = stream.ReadUInt();
                    Debug.Log($"Got the value {value} back from the server.");
                    mainScript.BackNumAdd();
                    clientManager.AddBackNum();
                    // text.text += $"Got the value {value} back from the server.";
                    //m_Connection.Disconnect(m_Driver);
                    //m_Connection = default;
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client got disconnected from server." + pornet);
                    clientManager.AddDisconnectNum();
                    // text.text += "Client got disconnected from server.";
                    m_Connection = default;
                }
            }
        }





        // text.text = "backNum  " + backNum + "    " + "connectServerNum  " + connectServerNum;

    }



    void OnDestroy()
    {
        m_Driver.Dispose();
    }
    public void SetClientPort(int vallue, LYYCreatePrefab value, LYYClientAction clientManager)
    {
        pornet = vallue;
        mainScript = value;
        this.clientManager = clientManager;
        StartAction();
    }
    public void StartAction()
    {
        action = true;
        m_Driver = NetworkDriver.Create();
        m_ServerEndPoint = NetworkEndpoint.Parse("82.156.76.152", (ushort)pornet);//6000
        //var endpoint = NetworkEndpoint.LoopbackIpv4.WithPort(7777);
        m_Connection = m_Driver.Connect(m_ServerEndPoint);



    }
}
