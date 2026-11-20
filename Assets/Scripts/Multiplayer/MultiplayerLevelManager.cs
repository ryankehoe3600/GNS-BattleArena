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
    public Text resultText;
    public Text timerText;
    public float matchDuration = 180f;
    private double matchEndTime;
    private bool timerReady = false;
    private bool matchEnded = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PhotonNetwork.LocalPlayer.SetScore(0);

        PhotonNetwork.Instantiate(
            "MultiplayerPlayer",
            Vector3.zero,
            Quaternion.identity

        );

        if (PhotonNetwork.IsMasterClient)
        {
            ExitGames.Client.Photon.Hashtable properties = 
                new ExitGames.Client.Photon.Hashtable();

                properties["MatchEndTime"] =
                    PhotonNetwork.Time + matchDuration;

                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }
    }

    public override void OnPlayerPropertiesUpdate(
        Photon.Realtime.Player targetPlayer,
        ExitGames.Client.Photon.Hashtable changedProps)
        {
            if(targetPlayer.GetScore() >= maxKills)
            {
                matchEnded = true;

                winnerText.text = targetPlayer.NickName;

                if (targetPlayer == PhotonNetwork.LocalPlayer)
                {
                    resultText.text = "YOU WIN!";
                }
                else
                {
                    resultText.text = "YOU LOSE!";
                }
                
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

    public override void OnRoomPropertiesUpdate(
        ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.ContainsKey("MatchEndTime"))
            {
                matchEndTime =
                    System.Convert.ToDouble(propertiesThatChanged["MatchEndTime"]);

                    timerReady = true;
            }

            if (propertiesThatChanged.ContainsKey("RestartRound"))
            {
                gameOverPopup.SetActive(false);
                matchEnded = false;
                PhotonNetwork.LocalPlayer.SetScore(0);

                foreach (Multiplayer player in 
                        FindObjectsByType<Multiplayer>(FindObjectsSortMode.None))
                {
                    player.ResetPlayerForNewRound();
                }

                Debug.Log("New round started!");
            }

        }

    void Update()
    {
        if (!timerReady || matchEnded)
            return;

        double remainingTime = System.Math.Max(
            0,
            matchEndTime - PhotonNetwork.Time
        );

        int minutes = (int)(remainingTime / 60);
        int seconds = (int)(remainingTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (remainingTime <= 0)
        {
            FinishMatchByTime();
        }

    }

    void FinishMatchByTime()
    {
        matchEnded = true;
        timerText.text = "00:00";

        Photon.Realtime.Player winner = null;
        int highestScore = -1;
        bool isDraw = false;

        foreach (var player in PhotonNetwork.PlayerList)
        {
            int score = player.GetScore();

            if (score > highestScore)
            {
                highestScore = score;
                winner = player;
                isDraw = false;
            }
            else if (score == highestScore)
            {
                isDraw = true;
            }
        }

        if (isDraw || winner == null)
        {
            winnerText.text = "DRAW!";
            resultText.text = "ITS A DRAW";
        }
        else
        {
            winnerText.text = winner.NickName;

            resultText.text = winner == PhotonNetwork.LocalPlayer
                ? "YOU WIN!"
                : "YOU LOSE!";
        }

        gameOverPopup.SetActive(true);
    }
    
    public void PlayAgain()
    {
        ExitGames.Client.Photon.Hashtable properties =
            new ExitGames.Client.Photon.Hashtable();

        properties["RestartRound"] = PhotonNetwork.ServerTimestamp;
        properties["MatchEndTime"] = PhotonNetwork.Time + matchDuration;

        PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
    }

}
