using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnPlayer1;
    private GameObject player1;

    [SerializeField]
    private GameObject spawnPlayer2;
    private GameObject player2;

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            player1 =  PhotonNetwork.Instantiate("Player", spawnPlayer1.transform.position, Quaternion.identity);
            player1.GetComponent<Character>().InitCharacter(Photon_Manager._PHOTON_MANAGER.player1raceID);
        }
        else
        {
            player2 = PhotonNetwork.Instantiate("Player" , spawnPlayer2.transform.position, Quaternion.identity);
            player2.GetComponent<Character>().InitCharacter(Photon_Manager._PHOTON_MANAGER.player2raceID);

        }
    }
}
