using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class WinTriggerCheck : MonoBehaviour
{
    [Header("Win Condition")]
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private TMP_Text PlayerWinText;
    //[SerializeField] private TMP_Text YouLostText;

    private static GameObject localPlayer;
    private PhotonView photonView;

    void Start()
    {
        WinPanel.SetActive(false);
        photonView = GetComponent<PhotonView>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PhotonView.Get(collision.gameObject).IsMine)
        {
            // Show the WinPanel with the PlayerWinText if the local player collides with the finish line
            WinPanel.SetActive(true);
            PlayerWinText.gameObject.SetActive(true);
            PlayerWinText.text = "You Won!";

            // Call the displayWin() function on all other players to display the YouLostText
            photonView.RPC("displayEnemyWin", RpcTarget.Others);
        }
    }

    [PunRPC]
    private void displayEnemyWin()
    {
        // Show the WinPanel with the YouLostText if other players collide with the finish line
        WinPanel.SetActive(true);
        PlayerWinText.gameObject.SetActive(false);
        PlayerWinText.gameObject.SetActive(true);
        PlayerWinText.text = "You Lost!";
    }
}
