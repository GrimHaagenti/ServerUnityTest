using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;


public class Network_Manager : MonoBehaviour
{
    public static Network_Manager _NETWORK_MANAGER;

    private TcpClient socket;
    private NetworkStream stream;

    private StreamWriter writer;
    private StreamReader reader;

    const string host = "10.40.3.70";
    const int port = 6543;

    private bool connected = false;

    private void Awake()
    {
        if(_NETWORK_MANAGER !=null && _NETWORK_MANAGER != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _NETWORK_MANAGER = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Update()
    {
        if (connected)
        {
            if (stream.DataAvailable)
            {
                string data = reader.ReadLine();
                if(data != null)
                {
                    ManageData(data);
                }
            }
        }
    }

    private void ManageData(string data)
    {
        if(data == "Ping")
        {
            Debug.Log("Ping");
            writer.WriteLine("1" + "/" + "Pong");
        }
    }


    public void ConnectToServer(string nick, string password)
    {
        try
        {
            //Conexión con el servidor
            socket = new TcpClient(host, port);

            //almacenamiento el canal de envío y recepción de datos
            stream = socket.GetStream();

            connected = true;
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            writer.WriteLine("0" + "/" + nick + "/" + password);
            writer.Flush();
        }
        catch(Exception ex)
        {
            connected = false;
        }


    }
}
