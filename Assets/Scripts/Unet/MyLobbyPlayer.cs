using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyLobbyPlayer : NetworkLobbyPlayer {

    public Button readyBttn;

    public InputField PlayerNameField;

    [SyncVar] public string PlayerName;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();

        


        if (MyLobbyManager.s_Singleton != null) MyLobbyManager.s_Singleton.OnPlayersNumberModified(1);

        readyToBegin = false;

        readyBttn = GameObject.Find("StartGameBttn").GetComponent<Button>();
        
        readyBttn.onClick.AddListener(PlayerReady);

        PlayerNameField = GameObject.Find("PlayerNameField").GetComponent<InputField>();

        if (isLocalPlayer)
        {
            if (PlayerName != null && PlayerNameField != null) ;
            PlayerNameField.text = PlayerName;
        }
        //PlayerNameField.onValueChange.RemoveAllListeners();
        PlayerNameField.onValueChange.AddListener(SetPlayerName);
    }

    [Command]
    void CmdNameChanged(string str)
    {
        PlayerName = str;
    }

    public void SetPlayerName(string _name)
    {
        if (isLocalPlayer)
        {
            CmdNameChanged(_name);
        }
    }

    public void PlayerReady()
    {

        if (isLocalPlayer)
        {
            SendReadyToBeginMessage();
        }

    }
    

}

