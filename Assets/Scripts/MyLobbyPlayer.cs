using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyLobbyPlayer : NetworkLobbyPlayer {

    public Button readyBttn;

    private void Start()
    {
        DontDestroyOnLoad(this);

        
    }

    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();

        readyToBegin = false;

        readyBttn = GameObject.Find("StartGameBttn").GetComponent<Button>();

        readyBttn.onClick.AddListener(PlayerReady);
    }

    public void PlayerReady()
    {

        if (isLocalPlayer)
        {
            SendReadyToBeginMessage();
        }

    }
    

}

