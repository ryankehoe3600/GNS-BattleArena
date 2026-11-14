using UnityEngine;
using Photon.Pun;


  public class MultiplayerLobby : MonoBehaviourPunCallbacks
  {
    public void LoginButtonClicked()
    {
      PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
      Debug.Log("We have connected to the Masterserver!");
    }
  }

