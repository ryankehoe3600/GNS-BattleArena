using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Photon.Pun;

public class MultiplayerLevelManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PhotonNetwork.Instantiate(
            "MultiplayerPlayer",
            Vector3.zero,
            Quaternion.identity

        );
    }

}
