using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateAndJoinSystem : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField CreateRoomCode;
    [SerializeField] private InputField JoinRoomCode;
    // Start is called before the first frame update
    void Start()
    {
        //PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom()
    {
        if (CreateRoomCode != null)
        {
            Debug.Log($"CREATING ROOM: {CreateRoomCode.text}");
            PhotonNetwork.ConnectUsingSettings();
            Invoke("LoadCreateRoom", 5f);
        }
        if (string.IsNullOrEmpty(CreateRoomCode.text))
        {
            Debug.LogError("InputField is Empty!");
        }
        
    }

    public void JoinRoom()
    {

        if (JoinRoomCode != null)
        {
            Debug.Log($"JOINING ROOM: {JoinRoomCode.text}");
            PhotonNetwork.ConnectUsingSettings();
            Invoke("LoadJoinRoom", 5f);
        }
        if (string.IsNullOrEmpty(JoinRoomCode.text))
        {
            Debug.LogError("InputField is Empty!");
        }
        
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Gameplay");
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void LoadCreateRoom()
    {
        PhotonNetwork.CreateRoom(CreateRoomCode.text);
    }

    public void LoadJoinRoom()
    {
        PhotonNetwork.JoinRoom(JoinRoomCode.text);
    }
}
