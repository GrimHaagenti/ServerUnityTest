using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCanvas : MonoBehaviour
{
    [SerializeField]
    private GameObject LoginScreen;
    [SerializeField]
    private GameObject RegisterScreen;
    [SerializeField]
    private GameObject GameLobby;
    [SerializeField]
    private GameObject LoadingIcon;


    public void Loading(bool isLoading)
    {
        LoadingIcon.SetActive(isLoading);
    }
    public void ShowLoginScreen()
    {
        RegisterScreen.SetActive(false);
        GameLobby.SetActive(false);
        LoginScreen.SetActive(true);
    }
    public void ShowRegisterScreen()
    {
        RegisterScreen.SetActive(true);
        GameLobby.SetActive(false);
        LoginScreen.SetActive(false);
    }
    public void ShowGameLobby()
    {
        RegisterScreen.SetActive(false);
        GameLobby.SetActive(true);
        LoginScreen.SetActive(false);
    }

}
