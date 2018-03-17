using UnityEngine;
using UnityEngine.Networking;
    
public class MyLobbyManager : NetworkLobbyManager {

    private void Start()
    {

#if UNITY_EDITOR
        StartHost();

#else
        networkAddress = "127.0.0.1";
        StartClient();
#endif
    }
    
    public override void OnClientConnect(NetworkConnection conn)
    {
        TryToAddPlayer();
    }

    //int count = 0;



    public override void OnLobbyServerPlayersReady()
    {
        Debug.Log("Ready");

        ServerChangeScene(playScene);
        
        //base.OnLobbyServerPlayersReady();
    }
    

    //public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    //{

    //    return null;// base.OnLobbyServerCreateGamePlayer(conn, playerControllerId);
    //}

    //public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    //{
    //    //base.OnServerAddPlayer(conn, playerControllerId);
    //    GameObject player = Instantiate().gameObject;
    //    NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

    //    player.GetComponent<PlayerScript>().playerIndex = count;
    //    count++;

    //    Debug.Log("Player Add");
    //}

}
