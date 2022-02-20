using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Linq;
public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;
    
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText; 
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform PlayerListContent;
    [SerializeField] GameObject roomListPrefab;
    [SerializeField] GameObject PlayerListPrefab;
    
    [SerializeField] GameObject startGameButton;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("Title");
        Debug.Log("Joined Lobby");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("Loading");
    }

        public override void OnJoinedRoom()
        {
            MenuManager.Instance.OpenMenu("Room");
            roomNameText.text = PhotonNetwork.CurrentRoom.Name;
            Player[] players = PhotonNetwork.PlayerList;

            foreach (Transform child in PlayerListContent)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < players.Count(); i++)
            {
                Instantiate(PlayerListPrefab, PlayerListContent).GetComponent<PlayerListItem>().Setup(players[i]);
            }
            startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            errorText.text = "Something went Crazy... \n Room Creation Failed \n Error Code " + message;
            MenuManager.Instance.OpenMenu("Error");
        }
   		public void JoinRoom(RoomInfo info)
        {
            PhotonNetwork.JoinRoom(info.Name);
            MenuManager.Instance.OpenMenu("Loading");
        }

        public void StartGame()
        {
            PhotonNetwork.LoadLevel(1);
        }

        public void LeaveRoom() 
        {
            PhotonNetwork.LeaveRoom();
            MenuManager.Instance.OpenMenu("Loading");
        }

        public override void OnLeftRoom()
        {
            MenuManager.Instance.OpenMenu("Title");
        }
        public override void OnRoomListUpdate(List<RoomInfo> listroom)
        {
            foreach (Transform transform in roomListContent)
            {
                Destroy(transform.gameObject);   
            }
            for (int i=0; i < listroom.Count; i++)
			{
				if (listroom[i].RemovedFromList)
					{
						continue;
					}
				Instantiate(roomListPrefab, roomListContent).GetComponent<RoomListItem>().Setup(listroom[i]);
			}
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
                Instantiate(PlayerListPrefab, PlayerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
        }

        //public override void Matchmaking()
        //{
        //    JoinRoom(roomListPrefab, roomListContent);
        //}
    }