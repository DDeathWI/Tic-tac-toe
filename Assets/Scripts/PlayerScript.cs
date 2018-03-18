using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerScript : NetworkBehaviour {

    private Text turnLeftTimeLabel;

    private Text whoTurnLabel;

    // value set who turn first
    [SyncVar (hook = "SetPlayerIndex")] public int playerIndex;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        turnLeftTimeLabel = GameObject.Find("TurnLabel").GetComponent<Text>(); 
        whoTurnLabel = GameObject.Find("WhoTurnLabel").GetComponent<Text>(); 
        
    }
    
    private void Start()
    {

        if (!isLocalPlayer)
            return;

        if (isServer)
        {
            Debug.LogError("1");
            playerIndex = 1;
            return;
        }

        Debug.LogError("0");
        playerIndex = 0;
    }

    private void SetPlayerIndex(int _index)
    {

    }


    private void Update()
    {
        // Check if localPLayer gameobject attemp to click
        if (!isLocalPlayer)
            return;

        if (GameController.instance.PlayerSide == playerIndex)
        {
            whoTurnLabel.color = Color.green;
            whoTurnLabel.text = "Your Turn";

            turnLeftTimeLabel.text = "Time Left: " + GameController.instance.TimeLeft;

            if (Input.GetMouseButtonDown(0))
            {

                // ClickPotion to WorldPotion
                Vector2 vector2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // Raycast for find field
                RaycastHit2D hit = Physics2D.Raycast(vector2, -Vector2.up, .01f);

                // If field
                if (hit.collider != null)
                {
                    // GetField Index
                    int index = hit.collider.GetComponent<Field>().Index;
                    // Command to Server for Player Turn
                    //int c = isServer ? 1 : 0;
                    CmdPlayerTurn(index, playerIndex);
                }

            }
        }
        else {
            whoTurnLabel.color = Color.red;
            whoTurnLabel.text = "Opponents Turn";

            turnLeftTimeLabel.text = "Time Left: " + GameController.instance.TimeLeft;
        }
    }

    /// <summary>
    /// PlayerTurn Command To Server
    /// </summary>
    /// <param name="fieldIndex"></param>
    [Command]
    void CmdPlayerTurn(int fieldIndex, int _playerId)
    {
        GameController.instance.SetChoosenField(fieldIndex, _playerId);
    }

}
