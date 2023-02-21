using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
 
    public static GameManager _GAME_MANAGER;

    GameData _GAME_DATA;
    
    JSONManager jsonManager = new JSONManager();

    private float version = -1;

    private bool finishedNetworkCheck = false;


    /// List for the local player characters
    //List<Character>

    private void Awake()
    {
        if (_GAME_MANAGER != null && _GAME_MANAGER != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _GAME_MANAGER = this;
            DontDestroyOnLoad(gameObject);
        }

        Network_Manager._NETWORK_MANAGER.GetLatestVersionRequest();
        

    }

    private void LoadGame()
    {
        Debug.Log("Juego Actualizado");
    }

    public void UpdateGameData(string newData)
    {
        string[] newRaceValues = newData.Split('-');
        List<Race> newRaces = new List<Race>();

        for (int i = 0; i < newRaceValues.Length-1; i++) //-
        {
            string[] newRaceStats = newRaceValues[i].Split('.');
            Race newRace = new Race();

            Debug.Log(newRaceValues[i]);

            newRace.max_hp = int.Parse(newRaceStats[0]);
            newRace.speed = int.Parse(newRaceStats[1]);
            newRace.jump_force = int.Parse(newRaceStats[2]);
            newRace.dmg = int.Parse(newRaceStats[3]);
            newRace.bullet_size = int.Parse(newRaceStats[4]);
    
            newRaces.Add(newRace);
        }

        _GAME_DATA.races = newRaces;
        jsonManager.WriteJsonData(_GAME_DATA);

        LoadGame();
    }

    public void SetVersion(float vers)
    {
        GameData JsonGameData = jsonManager.ReadJsonData();
        //Compare versions
        if (JsonGameData.version != vers)
        {
            //Desactualizado. Leer del la DB
            _GAME_DATA = new GameData();
            _GAME_DATA.version = vers;
            Network_Manager._NETWORK_MANAGER.UpdateGameDataRequest();

        }
        else
        {
            //Actualizado. Usar datos del JSON
            _GAME_DATA = JsonGameData;
            LoadGame();

        }


    }

}
