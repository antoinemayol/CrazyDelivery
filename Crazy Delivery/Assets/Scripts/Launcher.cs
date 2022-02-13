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
            foreach (var player in PhotonNetwork.PlayerList)
            {
                    Instantiate(PlayerListPrefab, PlayerListContent).GetComponent<PlayerListItem>().Setup(player);
            
            }
        }
        public void JoinRoom(RoomInfo info)
        {
            PhotonNetwork.JoinRoom(info.Name);
            MenuManager.Instance.OpenMenu("Loading");
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            errorText.text = "Something went Crazy... \n Room Creation Failed \n Error Code " + message;
            MenuManager.Instance.OpenMenu("Error");
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
            foreach (var room in listroom)
            {
                Instantiate(roomListPrefab, roomListContent).GetComponent<RoomListItem>().Setup(room);
            }
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
                Instantiate(PlayerListPrefab, PlayerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
        }
    }