using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TMPro;
using System.Text;
using UnityEngine.UI;
using System.Collections.Generic;
using static ServerTCP;

public class ServerTCP : MonoBehaviour
{
    Socket socket;
    Thread mainThread = null;


    //string serverName = "DefaultServerName";
    public GameObject UItextObj;
    TextMeshProUGUI UItext;
    public TMP_InputField chatText;
    public TMP_InputField serverName;
    public TMP_InputField hostName;
    string serverText;

    //string hostName = "DefaultHostName";

    List<User> userList;
    User testSubject;

    public struct User
    {
        public string name;
        public Socket socket;
    }

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
        userList = new List<User>();
    }


    void Update()
    {
        UItext.text = serverText;

    }

    public void startWaitingRoom()
    {
        serverText = "Host joined waiting room...";
    }

    public void startServer()
    {

        serverText = $"{hostName.text} hosted TCP Server {serverName.text}...";

        try
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 9051);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(localEndPoint);
            socket.Listen(10);
        }
        catch(SocketException e) 
        {
            Debug.LogException(e);
        }

        mainThread = new Thread(CheckNewConnections);
        mainThread.Start();
    }

    void CheckNewConnections()
    {
        while(true)
        {
            User newUser = new User();
            newUser.name = "DefaultName";

            try
            {
                newUser.socket = socket.Accept();//accept the socket
                IPEndPoint clientep = (IPEndPoint)newUser.socket.RemoteEndPoint;

                byte[] nameData = new byte[1024];
                int nameRecv = newUser.socket.Receive(nameData);
                newUser.name = Encoding.ASCII.GetString(nameData, 0, nameRecv);

                string newConnectionText = $"{newUser.name} joined lobby  |  IP Adress " + clientep.Address.ToString() + " at port " + clientep.Port.ToString();
                serverText = serverText + "\n" + newConnectionText;

                userList.Add(newUser);

                Thread answerOnConnect = new Thread(() => SendOnConnection(newUser, newConnectionText));
                answerOnConnect.Start();

                Thread newConnection = new Thread(() => Receive(newUser));
                newConnection.Start();
            }
            catch (SocketException e) 
            {
                serverText = "\n" + $"failed connection Exception {e.Message}";
                Debug.LogException(e);
            }
        }

    }

    void Receive(User user)
    {

        byte[] data = new byte[1024];
        int recv = 0;

        while (true)
        {
            try
            {
                recv = user.socket.Receive(data);

                if (recv == 0)
                {
                    Debug.Log("recv = 0");
                    break;
                }
                else
                {
                    serverText = serverText + "\n" + $"{user.name}: " + Encoding.ASCII.GetString(data, 0, recv);
                }
                
            }
            catch (SocketException e)
            {
                Debug.LogException(e);
                break;
            }

            Thread answerOnReceive = new Thread(() => SendOnReceive(user, Encoding.ASCII.GetString(data)));
            answerOnReceive.Start();
        }
    }

    void SendOnConnection(User user, string message)
    {
        try
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            for (int i = 0; i < userList.Count; i++)
            {
                user.socket.Send(data);
            }
        }
        catch(SocketException e)
        {
            Debug.LogException(e);
        }
    }

    void SendOnReceive(User user, string message)
    {
        try
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            for (int i = 0; i < userList.Count; i++)
            {
                if (user.name != userList[i].name) { user.socket.Send(data); }
            }
        }
        catch (SocketException e)
        {
            Debug.LogException(e);
        }
    }

    public void SendText()
    {
        try
        {
            serverText += "\n" + $"{hostName.text}: " + chatText.text;

            for (int i = 0; i < userList.Count; i++ )
            {
                userList[i].socket.Send(Encoding.ASCII.GetBytes($"{hostName.text}: " + chatText.text));
                //testSubject.socket.Send(Encoding.ASCII.GetBytes(chatText.text));
            }
        }
        catch (SocketException e)
        {
            Debug.LogException(e);
        }
    }

    private void OnApplicationQuit()
    {
        //socket?.Close();
        //Debug.Log($"Server {serverName} closed");
    }
}
