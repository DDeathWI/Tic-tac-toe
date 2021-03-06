﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class PlayerScript : NetworkBehaviour {

    private Text turnLeftTimeLabel;

    private Text whoTurnLabel;

    public Text ResultLabel;

    // value set who turn first
    [SyncVar(hook = "SetPlayerIndex")] public int playerIndex;

    [SyncVar] public string PlayerName = "default";

    [SyncVar] public string EnemyName = "default";


    [SyncVar (hook = "ShowResult")] public int RoundResult = 3;

    private int SpendTime;

    Dictionary<int, string> resultState;

    public void ShowResultLabel(int time)
    {
        if (isLocalPlayer)
        {
            SpendTime = time;
            ResultLabel.text = resultState[RoundResult] + "\n TimeSpend: " + SpendTime + "sec";

            XMLController.instance.resultContainer.Results.Add(new Result(PlayerName, EnemyName, resultState[RoundResult],SpendTime));
            string server;
            if (isServer) {
                server = "Host";
                    }
            else {
                server = "Client";
            }
            XMLController.instance.resultContainer.Save(Path.Combine(Application.persistentDataPath, server + "Results.xml"));
        }
    }

    private void Awake()
    {
        turnLeftTimeLabel = GameObject.Find("TurnLabel").GetComponent<Text>();
        whoTurnLabel = GameObject.Find("WhoTurnLabel").GetComponent<Text>();
        resultState = new Dictionary<int, string>();
        resultState.Add(0, "Lose");
        resultState.Add(1, "Win");
        resultState.Add(2, "Draw");
    }

    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(1);
        if(isLocalPlayer)
            CmdAddToPlayerDictionary();
    }


    private void SetPlayerIndex(int _index)
    {

    }


    private void ShowResult(int res)
    {
        RoundResult = res;
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

    [Command]
    void CmdAddToPlayerDictionary()
    {
        GameController.instance.AddPlayerToDictionary(playerIndex, PlayerName);
    }

}
