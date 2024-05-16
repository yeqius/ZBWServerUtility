using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;

public class LYYUDPServer : MonoBehaviour
{
    NetworkDriver m_Driver;
    NativeList<NetworkConnection> m_Connections;
    public NetworkEndpoint m_EndPoint;

    public int port;
    private bool action = false;
    private LYYCreatePrefab mainScript;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(action==false)
        {
            return;
        }

        m_Driver.ScheduleUpdate().Complete();
        // Clean up connections.
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
            {
                m_Connections.RemoveAtSwapBack(i);
                i--;
            }
        }
        // Accept new connections.
        NetworkConnection c;
        while ((c = m_Driver.Accept()) != default)
        {
            m_Connections.Add(c);
            Debug.Log("Accepted a connection.");
            mainScript.ConnectNumAdd();
           /// text.text += "   Accepted a connection.";
        }
        for (int i = 0; i < m_Connections.Length; i++)
        {
            DataStreamReader stream;
            NetworkEvent.Type cmd;
            while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    uint number = stream.ReadUInt();
                    Debug.Log($"Got {number} from a client, adding 2 to it.");
                    mainScript.AddNumAdd();
                    //text.text += $"Got {number} from a client, adding 2 to it.";
                    number += 2;

                    m_Driver.BeginSend(NetworkPipeline.Null, m_Connections[i], out var writer);
                    writer.WriteUInt(number);
                    m_Driver.EndSend(writer);
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from the server.");
                   // text.text += "Client disconnected from the server.";
                  m_Connections[i] = default;
                    break;
                }
            }
        }
    }
    void OnDestroy()
    {
        if (m_Driver.IsCreated)
        {
            m_Driver.Dispose();
            m_Connections.Dispose();
        }
    }


    public void SetServerPort(int vallue,LYYCreatePrefab value)
    {
        port = vallue;
        mainScript = value;
    }
    public void StartAction(string ip)
    {
        action = true;
        m_Driver = NetworkDriver.Create();
        m_Connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);
        m_EndPoint = NetworkEndpoint.Parse(ip, (ushort)port);//1386
                                                                          // var endpoint = NetworkEndpoint.AnyIpv4.WithPort(7777);
        if (m_Driver.Bind(m_EndPoint) != 0)
        {
            Debug.LogError("Failed to bind to port 7777.");
            //text.text += "    ERROR:Failed to bind to port 7777.";
            return;
        }
        m_Driver.Listen();

       

    }
}
