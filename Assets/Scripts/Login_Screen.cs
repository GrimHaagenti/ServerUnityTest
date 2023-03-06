using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Login_Screen : MonoBehaviour
{
    [SerializeField] private Button loginButton;
    [SerializeField] private Button toRegisterButton;
    [SerializeField] private TMP_InputField loginText;
    [SerializeField] private TMP_InputField passwordText;

    private void Start()
    {
        loginButton.onClick.AddListener(SendLogin);
        toRegisterButton.onClick.AddListener(GameManager._GAME_MANAGER.ChangeToRegister);
    }
    

    private void SendLogin()
    {
        string[] login = new string[2];
        login[0] = loginText.text;
        login[1] = passwordText.text;


        Network_Manager._NETWORK_MANAGER.ConnectToServer( ServerConnectionType.LOGIN,login);
        GameManager._GAME_MANAGER.player_username = loginText.text;
        GameManager._GAME_MANAGER.SetIsLoading(true);
    }

}
