using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class AuthenticationManager2 : MonoBehaviour
{
    public Text messageText;
    public InputField EmailInput;
    public InputField PasswordInput;

    public static string PlayerName;
    // Start is called before the first frame update
    void Start()
    {
        messageText.text = "";
        //Login();
    }

    // Update is called once per frame

    public void RegisterButton()
    {
        if (PasswordInput.text.Length < 6)
        {
            messageText.text = "Password is too short, must be at least 6 characters long";
            return;
        }
        var request = new RegisterPlayFabUserRequest()
        {
            Email = EmailInput.text,
            Password = PasswordInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
        PhotonNetwork.NickName = EmailInput.ToString();
    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest()
        {
            Email = EmailInput.text,
            Password = PasswordInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
        PhotonNetwork.NickName = EmailInput.ToString();
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Successfully Registered and Logged In";
    }
    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
        SceneManager.LoadScene("MenuScene");
    }
    void OnLoginSuccess(LoginResult result)
    {
        messageText.text = "Logged In";
        PlayerName = EmailInput.text;
        Debug.Log("Successful Login/Account Creation");
        PlayerName = EmailInput.text;
        SceneManager.LoadScene("MenuScene");
    }
    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        Debug.Log("Error while loggin in/creating account");
        Debug.Log(error.GenerateErrorReport());
    }
}
