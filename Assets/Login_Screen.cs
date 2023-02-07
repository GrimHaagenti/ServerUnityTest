using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Login_Screen : MonoBehaviour
{
    [SerializeField] private Button loginButton;
    [SerializeField] private TMP_InputField loginText;
    [SerializeField] private TMP_InputField passwordText;

    private void Awake()
    {
        loginButton.onClick.AddListener(SendLogin);
    }
    

    private void SendLogin()
    {
        Network_Manager._NETWORK_MANAGER.ConnectToServer(loginText.text, passwordText.text);
    }

}
