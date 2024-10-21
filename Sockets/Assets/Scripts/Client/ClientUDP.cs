using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;

public class ClientUDP : MonoBehaviour
{
    Socket socket;
    public GameObject UItextObj;
    TextMeshProUGUI UItext;
    string clientText;

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();

    }
    public void StartClient()
    {
        Thread mainThread = new Thread(Send);
        mainThread.Start();
    }

    void Update()
    {
        UItext.text = clientText;
    }

    void Send()
    {
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        byte[] data = Encoding.ASCII.GetBytes("DOUBLE KILL!!! Botlane enemies are on FIRE");
        string handshake = "Hello World";
        socket.SendTo(data, ipep);

        Thread receive = new Thread(Receive);
        receive.Start();

    }

    void Receive()
    {
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)sender;
        byte[] data = new byte[1024];
        int recv = socket.ReceiveFrom(data, ref Remote);

        clientText = ("Message received from {0}: " + Remote.ToString());
        clientText = clientText += "\n" + Encoding.ASCII.GetString(data, 0, recv);

    }

}

