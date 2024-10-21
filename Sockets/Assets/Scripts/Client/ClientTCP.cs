using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;
using static ServerTCP;
using UnityEngine.UI;

public class ClientTCP : MonoBehaviour
{
    public GameObject UItextObj;
    TextMeshProUGUI UItext;
    string clientText;
    Socket server;

    public TMP_InputField chatText;
    public TMP_InputField serverIP;
    public TMP_InputField clientName;

    // Start is called before the first frame update
    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        UItext.text = clientText;
        //if(server == null) { Debug.LogError(" server was null"); }
        //if(!server.Connected) { Debug.LogError(" server not connected"); }
    }

    public void StartClient()
    {
        Thread connect = new Thread(Connect);
        connect.Start();
    }
    void Connect()
    {
        try
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(serverIP.text), 9051);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Connect(ipep);
        }
        catch(SocketException e)
        {
            clientText += "\n" + $"Failed Connection with server {e}";
            Debug.LogException(e);
        }

        Thread sendThread = new Thread(Send);
        sendThread.Start();

        Thread receiveThread = new Thread(Receive);
        receiveThread.Start();

    }
    void Send()
    {
        try
        {
            server.Send(Encoding.ASCII.GetBytes(clientName.text));
        }
        catch (SocketException e)
        {
            Debug.LogException(e);
        }
    }

    public void SendText()
    {
        if (server != null && server.Connected) 
        { 
            try
            {
                clientText += "\n" + $"{clientName.text}: " + chatText.text;
                server.Send(Encoding.ASCII.GetBytes(chatText.text));
            }
            catch(SocketException e)
            {
                Debug.LogException(e);
            }
        }
        else
        {
            clientText += "\n Server is not connected.";
        }
        
    }

    
    void Receive()
    {
        byte[] data = new byte[1024];
        int recv = 0;

        while (true)
        {
            try
            {
                recv = server.Receive(data);

                if (recv == 0)
                {
                    Debug.Log("recv = 0");
                    break;
                }
                else
                {
                    clientText += "\n" + Encoding.ASCII.GetString(data, 0, recv);
                }
            }
            catch (SocketException e)
            {
                Debug.LogException(e);
                break;
            }
        }
    }

    private void OnApplicationQuit()
    {
        //server?.Close();
    }
}
