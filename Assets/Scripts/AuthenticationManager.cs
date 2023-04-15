using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class AuthenticationManager : MonoBehaviour
{
    [SerializeField] private LoginPanel _login;
    [SerializeField] private RegistrationPanel _registration;
    [SerializeField] private CurrencyManager _currencyManager;
    [SerializeField] private GetPlayerCombinedInfoRequestParams _infoRequestParams;

    public string PlayFabId { get; private set; }

    private void OnEnable()
    {
        _login.LoginEvent += OnLoginCalled;
        _registration.RegisterEvent += OnRegisterCalled;
    }

    private void OnDisable()
    {
        _login.LoginEvent -= OnLoginCalled;
        _registration.RegisterEvent -= OnRegisterCalled;
    }

    private void OnLoginCalled(string userName , string password )
    {
        PlayFabClientAPI.LoginWithEmailAddress(
                new LoginWithEmailAddressRequest()
                {
                    Email = userName,
                    Password = password,
                    TitleId = PlayFabSettings.TitleId,
                    InfoRequestParameters = _infoRequestParams // this one

                },(loginResult) => {
                    Debug.Log("Successfully Logged in");
                    if (loginResult.InfoResultPayload != null)
                    {

                        InitializeConfigValues(loginResult.InfoResultPayload);
                    }
                    else
                    {
                        Debug.LogError("Info Result Payload is null");
                    }
                    PhotonNetwork.NickName = userName;
                },(error) => {
                    Debug.LogError($"Failed to login: {error.ErrorMessage}");
                }
            );
    }

    private void OnRegisterCalled(string userName, string password)
    {
        PlayFabClientAPI.LoginWithCustomID(
            new LoginWithCustomIDRequest
            {
                CustomId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true,
                TitleId = PlayFabSettings.TitleId,
                InfoRequestParameters = _infoRequestParams
            }, (loginSuccess) =>
            {
                if (loginSuccess.InfoResultPayload != null)
                {
                    InitializeConfigValues(loginSuccess.InfoResultPayload);
                }
                else
                {
                    Debug.LogError("Info Result Payload is null");
                } 

                PlayFabClientAPI.AddUsernamePassword(new AddUsernamePasswordRequest
                {
                    Email = userName,
                    Password = password,
                    Username = loginSuccess.PlayFabId
                }, (updateSuccess) =>
                    {
                        Debug.Log("Sucessfully Registered");
                        PhotonNetwork.NickName = userName;
                        SceneManager.LoadScene("PunBasics-Launcher");
                    }, (updateFail) =>
                {
                    var msg = "";
                        foreach (var VARIABLE in updateFail.ErrorDetails)
                        {
                            msg += VARIABLE.Key + "\n";
                            foreach (var item in VARIABLE.Value)
                            {
                                msg += $"#{item}";
                            }
                        }
                        Debug.LogError($"Failed to Register: {updateFail.Error}\n {updateFail.ErrorMessage} \n {msg}");
                    });

            }, (loginFailure) =>
            {
                Debug.LogError($"Unable to Login with custom Id: {loginFailure.ErrorMessage}");
            });
    }

    private void InitializeConfigValues(GetPlayerCombinedInfoResultPayload payload)
    {
        if (payload.TitleData == null) return;
        var titleData = payload.TitleData;

        if (payload.UserVirtualCurrency != null)
        {
            foreach (var item in payload.UserVirtualCurrency)
            {
                _currencyManager.Init(item.Key, item.Value);
                Debug.Log($"{item.Key}: {item.Value}");
            }
        }

        if (payload.AccountInfo != null)
        {
            PlayFabId = payload.AccountInfo.PlayFabId;
        }
    }

}
