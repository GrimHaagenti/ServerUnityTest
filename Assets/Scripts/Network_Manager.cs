using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;

public enum ServerConnectionType {LOGIN, PING, REGISTER, GAMEDATA, VERSION }

public class Network_Manager : MonoBehaviour
{
    public static Network_Manager _NETWORK_MANAGER;

    private TcpClient socket;
    private NetworkStream stream;

    private StreamWriter writer;
    private StreamReader reader;


    //const string host = "10.40.3.70";
    const string host = "localhost";
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
        string[] parameters = data.Split('/');

        Debug.Log(data);
        switch (parameters[0])
        {
            //Login 
            case "0":
                break;

            // Ping
            case "1":
                ConnectToServer(ServerConnectionType.PING, new string[1]);


                break;

            //Register
            case "2":
                break;

            //GetData
            case "3":
                GameManager._GAME_MANAGER.UpdateGameData(parameters[1]);
                break;

            //Version Control
            case "4":
                GameManager._GAME_MANAGER.SetVersion(float.Parse( parameters[1] ));
                break;


        }

       
    }

    public void UpdateGameDataRequest()
    {
        ConnectToServer(ServerConnectionType.GAMEDATA, new string[1]);
    }

    public void GetLatestVersionRequest()
    {

        ConnectToServer(ServerConnectionType.VERSION, new string[1]);
    }
    
    public void ConnectToServer(ServerConnectionType conn, string[] parameters)
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

            switch (conn)
            {
                case ServerConnectionType.LOGIN:

                    if (parameters.Length != 2) {
                        Debug.LogError("Parametros mal");
                        return ; }

                    writer.WriteLine("0" + "/" + parameters[0] + "/" + parameters[1]);
                    writer.Flush();
                    break;



                case ServerConnectionType.PING:
                    writer.WriteLine(ServerConnectionType.PING + "/" + "Pong");
                    break;


                case ServerConnectionType.REGISTER:
                    if (parameters.Length != 2)
                    {
                        Debug.LogError("Parametros mal");
                        return ;
                    }

                    Debug.Log("Trying to register");
                    writer.WriteLine((int)ServerConnectionType.REGISTER + "/" + parameters[0] + "/" + parameters[1]);
                    writer.Flush();
                    break;



                case ServerConnectionType.GAMEDATA:
                    writer.WriteLine((int)ServerConnectionType.GAMEDATA + "/");
                    writer.Flush();
                    break;

                case ServerConnectionType.VERSION:
                    writer.WriteLine((int)ServerConnectionType.VERSION + "/");
                    writer.Flush();
                    break;
                default:

                    break;
            }

            
        }
        catch (Exception)
        {
            connected = false;
        }


    }
}
