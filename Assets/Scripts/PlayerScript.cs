using UnityEngine;
using UnityEngine.Networking;


public class PlayerScript : NetworkBehaviour {

    // GameObject EmptyField
    public GameObject Field;

    // value set who turn first
    [SyncVar (hook = "SetPlayerIndex")] public int playerIndex;

    private void Start()
    {
        if(!isLocalPlayer)
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
        playerIndex = _index;
    }

    private void Update()
    {
        // Check if localPLayer gameobject attemp to click
        if (!isLocalPlayer)
            return;

        if (GameController.instance.PlayerSide == playerIndex)
        {
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
    }

    /// <summary>
    /// PlayerTurn Command To Server
    /// </summary>
    /// <param name="fieldIndex"></param>
    [Command]
    void CmdPlayerTurn(int fieldIndex, int _playerId)
    {
        GameController.instance.SetField(fieldIndex, _playerId);
    }

}
