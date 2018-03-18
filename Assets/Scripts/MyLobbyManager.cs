using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyLobbyManager : NetworkLobbyManager {

    static public MyLobbyManager s_Singleton;

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

    }

    //public override void OnClientConnect(NetworkConnection conn)
    //{
    //    TryToAddPlayer();
    //}


    //public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    //{
    //    return base.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
    //}

}

