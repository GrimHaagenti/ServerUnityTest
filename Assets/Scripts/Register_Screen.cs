using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Register_Screen : MonoBehaviour
{

    [SerializeField] private Button registerButton;
    [SerializeField] private Button toLoginButton;
    [SerializeField] private TMP_InputField registerText;
    [SerializeField] private TMP_InputField passwordText;

    private void Start()
    {
        registerButton.onClick.AddListener(SendRegister);
        toLoginButton.onClick.AddListener(GameManager._GAME_MANAGER.ChangeToLogin);
    }


    private void SendRegister()
    {
        string[] arr = new string[2];
        arr[0] = registerText.text;
        arr[1] = passwordText.text;

        //Debug.Log(arr[0] + "  " + arr[1]);
        Network_Manager._NETWORK_MANAGER.ConnectToServer( ServerConnectionType.REGISTER, arr);
        GameManager._GAME_MANAGER.SetIsLoading(true);
    }
}
