using UnityEngine;
using UnityEngine.Networking;


public class NetworkController : MonoBehaviour {

    NetworkLobbyManager networkLobbyManager;
    
    NetworkClient myClient;

	// Use this for initialization
	void Start () {
        networkLobbyManager = GetComponent<NetworkLobbyManager>();

#if UNITY_EDITOR
        networkLobbyManager.StartHost();
#else
        networkLobbyManager.networkAddress = "127.0.0.1";
        networkLobbyManager.StartClient();
#endif
    }



  
}
