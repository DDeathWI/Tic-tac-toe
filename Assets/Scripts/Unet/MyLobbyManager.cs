using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyLobbyManager : NetworkLobbyManager {

    static public MyLobbyManager s_Singleton;

    NetworkClient networkClient;

    int _playerNumber = 0;

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

    }

    public void OnPlayersNumberModified(int count)
    {
        _playerNumber += count;

        int localPlayerCount = 0;
        foreach (PlayerController p in ClientScene.localPlayers)
            localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;

        //addPlayerButton.SetActive(localPlayerCount < maxPlayersPerConnection && _playerNumber < maxPlayers);
    }


    //public override void OnClientConnect(NetworkConnection conn)
    //{
    //    TryToAddPlayer();
    //}

    int count;
    int CountName = 0;

    public override void OnLobbyClientEnter()
    {
        count = Random.Range(0,2);
        CountName = 0;
       
        base.OnLobbyClientEnter();
    }

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = Instantiate(gamePlayerPrefab, Vector3.zero, Quaternion.identity);

        //for (int i = 0; i < lobbySlots.Length; i++)
        //{
        //if (lobbySlots[i].playerControllerId == playerControllerId)
        //{


        Debug.LogError(CountName + " " + (lobbySlots[CountName] as MyLobbyPlayer).PlayerName);
        //}
        //}
        player.name = (lobbySlots[CountName] as MyLobbyPlayer).PlayerName;
                player.GetComponent<PlayerScript>().PlayerName = player.name;
                player.GetComponent<PlayerScript>().playerIndex = count;

                count = count == 1 ? 0 : 1;
        CountName++;
                //NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        return player;
    }
    
}

