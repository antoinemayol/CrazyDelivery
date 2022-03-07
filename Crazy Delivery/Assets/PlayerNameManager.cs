using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameInputCR;
    [SerializeField] TMP_InputField usernameInputFR;
    
    void Start()
    {
        if (usernameInputCR != null)
            {
                if (PlayerPrefs.HasKey("username"))
                {
                    usernameInputCR.text = PlayerPrefs.GetString("username");
                    PhotonNetwork.NickName = PlayerPrefs.GetString("username");
                }
                /*else
                {
                    usernameInputCR.text = "Player " + Random.Range(0, 10000).ToString("0000");
                    OnUsernameInputValueChanged();
                }*/
                
            }
        else if (usernameInputFR != null)
            {
                if (PlayerPrefs.HasKey("username"))
                {
                    usernameInputFR.text = PlayerPrefs.GetString("username");
                    PhotonNetwork.NickName = PlayerPrefs.GetString("username");
                }
                /*else
                {
                    usernameInputFR.text = "Player " + Random.Range(0, 10000).ToString("0000");
                    OnUsernameInputValueChanged();
                }*/
            }
    }   
    public void OnUsernameInputValueChanged()
    {
        if (usernameInputCR != null)
        {
            PhotonNetwork.NickName = usernameInputCR.text;
            PlayerPrefs.SetString("username", usernameInputCR.text);
        }
        else if (usernameInputFR != null)
        {
            PhotonNetwork.NickName = usernameInputFR.text;
            PlayerPrefs.SetString("username", usernameInputFR.text);
        }
    }

}
