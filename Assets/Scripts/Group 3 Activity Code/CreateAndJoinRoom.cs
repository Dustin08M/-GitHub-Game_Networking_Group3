using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button CreateButton;
    [SerializeField] private InputField JoinRoomNumber;
    [SerializeField] private Button JoinRoomButton;
    [SerializeField] private Text RoomCodeNumber;
    private string EnteredCode;

    public static string DisplayCodeToGame;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnEnable()
    {
        if (CreateButton)
        {
            CreateButton.onClick.AddListener(ConnectToPhoton);
        }

        if (JoinRoomButton)
        {
            JoinRoomButton.onClick.AddListener(JoinRoom);
        }
    }

    public override void OnDisable()
    {
        if (CreateButton)
        {
            CreateButton.onClick.RemoveListener(ConnectToPhoton);
        }

        if (JoinRoomButton)
        {
            JoinRoomButton.onClick.RemoveListener(JoinRoom);
        }
    }
    private void Start()
    {
        Debug.Log(GetMessage($"Is Connected and Ready: {PhotonNetwork.IsConnectedAndReady}"));
        RoomCodeNumber.text = "";
    }
    public void CreateRoom()
    {
        /*if (PhotonNetwork.IsConnectedAndReady)
        {
            string RoomNumber = "Room #" + Random.Range(0001, 1000);
            PhotonNetwork.CreateRoom(RoomNumber, new RoomOptions { MaxPlayers = 6 }, null);
            RoomCodeNumber.text = RoomNumber;
            DisplayCodeToGame = RoomNumber;
            Debug.Log("Room #" + RoomNumber);

        }
        else
        {
            Debug.Log("Not connected to Master Server or not ready to perform operations.");
        }*/
    }

    private void ConnectToPhoton()
    {
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log(GetMessage($"Connecting To Photon, {name}"));
    }

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(JoinRoomNumber.text))
        {
            Debug.LogError("The Field is Empty, please type it again");
        }

        if (JoinRoomNumber != null)
        {
            string JoinRoomCode = "Room #" + JoinRoomNumber.text;
            EnteredCode = JoinRoomCode;
            DisplayCodeToGame = JoinRoomCode;
        }
    }

    public void OnJoinedRoom()
    {
        PhotonNetwork.JoinRoom(EnteredCode);
    }

    public override void OnCreatedRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            base.OnCreatedRoom();
            string RoomNumber = "Room #" + Random.Range(0001, 1000);
            PhotonNetwork.CreateRoom(RoomNumber, new RoomOptions { MaxPlayers = 6 }, null);
            RoomCodeNumber.text = RoomNumber;
            DisplayCodeToGame = RoomNumber;
            Debug.Log("Room #" + RoomNumber);
            SceneManager.LoadScene(4);
        }
        else
        {
            Debug.Log("Not connected to Master Server or not ready to perform operations.");
        }
    }

    private string GetMessage(string message)
    {
        return $"{nameof(CreateAndJoinRoom)}: {message}";
    }

    public override void OnJoinedLobby()
    {
        Debug.Log(GetMessage("Successfully Connected to Lobby"));
    }

    public override void OnConnected()
    {
        Debug.Log(GetMessage("Successfully Connected to Server"));
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError(GetMessage($"Discoonected due to {cause}"));
    }
}
