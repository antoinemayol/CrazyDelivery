using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    RoomInfo info;
    public void Setup(RoomInfo roominfo)
    {
        info = roominfo;
        text.text = roominfo.Name;
    }

    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);
    }
}
