using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;
using static ServerTCP;

public class ServerUDP : MonoBehaviour
{
    Socket socket;

    public GameObject UItextObj;
    TextMeshProUGUI UItext;
    string serverText;

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();

    }
    public void startServer()
    {
        serverText = "Starting UDP Server...";

        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(ipep);

        Thread newConnection = new Thread(Receive);
        newConnection.Start();
    }

    void Update()
    {
        UItext.text = serverText;
    }

 
    void Receive()
    {
        int recv;
        byte[] data = new byte[1024];
        
        serverText = serverText + "\n" + "Waiting for new Client...";

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)(sender);
        
        while (true)
        {
            recv = socket.ReceiveFrom(data, ref Remote);

            serverText = serverText + "\n" + "Message received from {0}:" + Remote.ToString();
            serverText = serverText + "\n" + Encoding.ASCII.GetString(data, 0, recv);

            Thread answer = new Thread(() => Send(Remote));
            answer.Start();
        }

    }

    void Send(EndPoint Remote)
    {
        byte[] data = Encoding.ASCII.GetBytes("PING");
        string welcome = "Ping";
        socket.SendTo(data, Remote);
    }

   
}
