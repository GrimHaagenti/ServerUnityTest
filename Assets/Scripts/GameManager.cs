using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
 
    public static GameManager _GAME_MANAGER;

    public GameData _GAME_DATA;
    
    JSONManager jsonManager = new JSONManager();

    private MainMenuCanvas MenuCanvas;

    private float version = -1;

    private bool finishedNetworkCheck = false;

    private int currentPlayerID = -1;
    public string player_username = "";
    public int currentRace = -1;

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
        SceneManager.LoadSceneAsync("MainMenu").completed += (AsyncOperation) =>
        {
            SetMenuCanvas();
        };
        
    }

    public void GoBackToLobby()
    {
        SceneManager.LoadSceneAsync("MainMenu").completed += (AsyncOperation) =>
        {
            SetMenuCanvas();
        };

    }

    private void  SetMenuCanvas()
    {
        GameObject[] rootObj = SceneManager.GetActiveScene().GetRootGameObjects();
        for (int i = 0; i < rootObj.Length; i++)
        {
            if (rootObj[i].TryGetComponent<MainMenuCanvas>(out MainMenuCanvas m))
            {
                MenuCanvas = m;
            }
        }

    }

    public void ManageLogin(int playerID)
    {
        if (playerID != -1)
        {
            currentPlayerID = playerID;
            Debug.Log(playerID);
            EnterLobby();
        }
    }
    public void ManageRegister(int playerID)
    {
        if (playerID != -1)
        {
            currentPlayerID = playerID;
            Debug.Log(playerID);
            EnterLobby();
        }
        

    }
    public void ChangeToRegister()
    {
        MenuCanvas.ShowRegisterScreen();
    }
    public void ChangeToLogin ()
    {
        MenuCanvas.ShowLoginScreen();
    }
    public void SetIsLoading(bool isLoading)
    {
        MenuCanvas.Loading(isLoading);
    }
    public void EnterLobby()
    {
        SetIsLoading(false);
        MenuCanvas.ShowGameLobby();
    }

    public void ParsePlayerCharacters(string charas)
    {
        string[] playerCharactersString = charas.Split('-');
        List<PlayerCharacter> playerCharacters = new List<PlayerCharacter>();

        for (int i = 0; i < playerCharactersString.Length - 1; i++) //-
        {
            string[] charaString = playerCharactersString[i].Split('.');


            //Debug.Log(newRaceValues[i]);

            int id_character = int.Parse(charaString[0]);
            string chara_name = charaString[1];
            Race chara_race = _GAME_DATA.races[int.Parse(charaString[2])];
            int owner = int.Parse(charaString[3]);

            playerCharacters.Add(PlayerCharacter.CreateCharacter(id_character, chara_name, chara_race, owner));
        }

        _GAME_DATA.playerCharacters = playerCharacters;
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

            newRace.max_hp = int.Parse(newRaceStats[1]);
            newRace.speed = int.Parse(newRaceStats[2]);
            newRace.jump_force = int.Parse(newRaceStats[3]);
            newRace.dmg = int.Parse(newRaceStats[4]);
            newRace.bullet_size = int.Parse(newRaceStats[5]);
    
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
