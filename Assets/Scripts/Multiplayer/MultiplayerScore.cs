using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MultiplayerScore : MonoBehaviourPunCallbacks
{
    public GameObject playerScorePrefab;
    public Transform panel;
    Dictionary<int, GameObject> playerScore = new Dictionary<int, GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            player.SetScore(0);
            var playerScoreObject = Instantiate(playerScorePrefab, panel);

            var playerScoreObjectText =
                playerScoreObject.GetComponent<Text>();

            playerScoreObjectText.text = 
                string.Format("{0} Score: {1}",
                    player.NickName,
                    player.GetScore());

            playerScore[player.ActorNumber] = playerScoreObject;
        }
    }

    public override void OnPlayerPropertiesUpdate(
        Photon.Realtime.Player targetplayer,
        ExitGames.Client.Photon.Hashtable changedProps)
        {

        }
        
    // Update is called once per frame
    void Update()
    {
        
    }
}
