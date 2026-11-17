using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Photon.Pun.UtilityScripts;

public class MultiplayerLevelManager : MonoBehaviourPunCallbacks
{

    public int maxKills = 3;
    public GameObject gameOverPopup;
    public Text winnerText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PhotonNetwork.Instantiate(
            "MultiplayerPlayer",
            Vector3.zero,
            Quaternion.identity

        );
    }

    public override void OnPlayerPropertiesUpdate(
        Photon.Realtime.Player targetPlayer,
        ExitGames.Client.Photon.Hashtable changedProps)
        {
            if(targetPlayer.GetScore() >= maxKills)
            {
                winnerText.text = targetPlayer.NickName;
                gameOverPopup.SetActive(true);

            }

        }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("Lobby");
    }
    

}
