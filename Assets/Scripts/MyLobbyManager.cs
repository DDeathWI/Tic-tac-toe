using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyLobbyManager : NetworkLobbyManager {

    static public MyLobbyManager s_Singleton;

    public InputField PlayerNameField;

    string PlayerName;

    NetworkClient networkClient;

    private void Start()
    {
        s_Singleton = this;

        networkClient = StartHost();

        if (networkClient != null)
        {
            Debug.LogError("Created");
        }
        else
        {
            networkAddress = "127.0.0.1";
            StartClient();
        }
        PlayerNameField.onValueChange.AddListener(SetPlayerName);

    }

    private void SetPlayerName(string _name)
    {
        PlayerName = _name;
    }


    //public override void OnClientConnect(NetworkConnection conn)
    //{
    //    TryToAddPlayer();
    //}

    int count;
    public override void OnLobbyClientEnter()
    {
        if (PlayerNameField == null)
        {
            PlayerNameField = FindObjectOfType<InputField>();
            PlayerNameField.onValueChange.AddListener(SetPlayerName);
        }

        count = Random.Range(0,2);
        base.OnLobbyClientEnter();
    }

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = Instantiate(gamePlayerPrefab, Vector3.zero, Quaternion.identity);
        player.name = PlayerName;
        Debug.LogError("Name " + PlayerName);
        player.GetComponent<PlayerScript>().PlayerName = PlayerNameField.text;
        player.GetComponent<PlayerScript>().playerIndex = count;

        count = count == 1 ? 0 : 1;

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        return player;
    }
    
}

