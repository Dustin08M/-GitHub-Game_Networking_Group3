using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ConnectingToPhoton : MonoBehaviourPunCallbacks
{
    public void ConnectPhoton()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
}
