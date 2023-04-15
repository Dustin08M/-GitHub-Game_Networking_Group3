using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] public static Transform _spawnLocation;
    [SerializeField] private  Transform _spawnLocation_thisScene;

    [SerializeField] private GameObject localPlayer;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate("Characters/Player", _spawnLocation_thisScene.position, Quaternion.identity);
    }
}
