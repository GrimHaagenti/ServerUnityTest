using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Photon_Manager : MonoBehaviourPunCallbacks
{
    public static Photon_Manager _PHOTON_MANAGER;

    public int player1raceID = -1;
    public int player2raceID = -1;
    private void Awake()
    {

        //Singleton
        if (_PHOTON_MANAGER != null && _PHOTON_MANAGER != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _PHOTON_MANAGER = this;
            DontDestroyOnLoad(this.gameObject);

            ///Photon Connect
            PhotonConnect();

        }
    }


    public void PhotonConnect()
    {

        PhotonNetwork.AutomaticallySyncScene = true;


        //Conexion al servidor con la configuración establecida
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conexión realizada correctamente");

        PhotonNetwork.JoinLobby(TypedLobby.Default);


    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("He implosionado porque " +cause);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Accediendo al lobby");
    }

    public void CreateRoom(string roomName, int p1RaceID)
    {
        PhotonNetwork.CreateRoom(roomName, new RoomOptions{ MaxPlayers = 2} );
        player1raceID = p1RaceID;
        Debug.Log("Room Created");
    }


    public void JoinRoom(string nameRoom, int p2RaceID)
    {
        PhotonNetwork.JoinRoom(nameRoom) ;
        player2raceID = p2RaceID;
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("Me he unido a la sala " + PhotonNetwork.CurrentRoom.Name + " con " + PhotonNetwork.CurrentRoom.PlayerCount + " jugadores conectados en ella.");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Gameplay");
        }

    }

}
