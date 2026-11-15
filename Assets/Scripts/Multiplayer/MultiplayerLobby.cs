using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;


  public class MultiplayerLobby : MonoBehaviourPunCallbacks
  {
    public Transform LoginPanel;
    public Transform SelectionPanel;
    public Transform CreateRoomPanel;
    public Transform InsideRoomPanel;
    public Transform ListRoomsPanel;
    public InputField roomNameInput;
    public InputField playerNameInput;
    string playerName;
    public GameObject textPrefab;
    public Transform insideRoomPlayerList;
    public Transform listRoomPanel;
    public GameObject roomEntryPrefab;
    public Transform listRoomPanelContent;

    private void Start()
    {
        playerNameInput.text = playerName =
        string.Format("Player {0}", Random.Range(1, 1000000));

        Debug.Log("Generated player name: " + playerName);
    }
    
    public void LoginButtonClicked()
    {
        PhotonNetwork.LocalPlayer.NickName = playerName = playerNameInput.text;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("We have connected to the Master server!");

        ActivatePanel("Selection");
    }

    public void ActivatePanel(string panelName)
    {
        LoginPanel.gameObject.SetActive(false);
        SelectionPanel.gameObject.SetActive(false);
        CreateRoomPanel.gameObject.SetActive(false);
        InsideRoomPanel.gameObject.SetActive(false);
        ListRoomsPanel.gameObject.SetActive(false);

        if (panelName == LoginPanel.gameObject.name)
        {
            LoginPanel.gameObject.SetActive(true);
        }
        else if (panelName == SelectionPanel.gameObject.name)
        {
            SelectionPanel.gameObject.SetActive(true);
        }
        else if (panelName == CreateRoomPanel.gameObject.name)
        {
            CreateRoomPanel.gameObject.SetActive(true);
        }
        else if (panelName == InsideRoomPanel.gameObject.name)
        {
            InsideRoomPanel.gameObject.SetActive(true);
        }
        else if (panelName == ListRoomsPanel.gameObject.name)
        {
            ListRoomsPanel.gameObject.SetActive(true);
        }

    }

    public void DisconnectButtonClicked()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from the master server!");

        ActivatePanel("Login");
    }

    public void CreateARoom()
    {
        RoomOptions roomOptions = new RoomOptions();

        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = true;

        PhotonNetwork.CreateRoom(roomNameInput.text, roomOptions);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room has beencreated!");

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room: " + message);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Room has been joined!");

        ActivatePanel("InsideRoom");

        foreach (var player in PhotonNetwork.PlayerList)
        {
            var playerListEntry = Instantiate(
                textPrefab,
                insideRoomPlayerList
            );

            playerListEntry.GetComponent<Text>().text = player.NickName;
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Room has been left!");

        ActivatePanel("CreateRoom");

        DestroyChildren(insideRoomPlayerList);
    }

    public void DestroyChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    public void ListRoomsClicked()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby!");

        ActivatePanel("ListRooms");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Room updates: " + roomList.Count);

        foreach (var room in roomList)
        {
            var newRoomEntry = Instantiate(
                roomEntryPrefab,
                listRoomPanelContent
            );
        }
    }

    public void LeaveLobbyClicked()
    {
        PhotonNetwork.LeaveLobby();
    }

    public override void OnLeftLobby()
    {
        Debug.Log("Left lobby!");

        ActivatePanel("Selection");
    }
  }

