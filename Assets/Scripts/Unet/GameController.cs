using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameController : NetworkBehaviour {

    // Create Singleton
    public static GameController instance;

    // Player Side X or O
    [SyncVar(hook = "SetSide")] public int PlayerSide;

    // Empty Field Gamobject
    public GameObject EmptyField;

    // List Script for action with gamefields
    private List<Field> fieldsScript;

    private List<Field> emptyFields;
    // Keep turn number for check if GameEnd
    private int turnCounter;

    [SyncVar(hook = "StartTime")] public float TimeStart;

    [SyncVar(hook = "CalculateTimeLeft")] public float TimeLeft;

    public int TurnTime = 10;

    public GameObject ResultPanel;

    public Button BackToMenuBttn;

    private Coroutine gameResult;

    public Text ResultLabel;

    [SyncVar(hook = "GameOver")] private bool gameOver;

    Dictionary<int, string> playersDictionary;

    [SyncVar(hook = "RpcRoundTime")] private int spendTime;

    private void CalculateTimeLeft(float _time)
    {
        TimeLeft = _time;

        if (TimeLeft <= 0)
        {

            StartTime(Time.timeSinceLevelLoad);
            RpcSetRandomField(PlayerSide);
        }
    }

    private void Update()
    {
        if (!isServer)
            return;

        TimeLeft = TurnTime - (int)(Time.timeSinceLevelLoad - TimeStart);
    }


    private void SetSide(int _side)
    {
        PlayerSide = _side;
        StartTime(Time.timeSinceLevelLoad);
    }

    private void StartTime(float _time)
    {
        TimeStart = _time;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            playersDictionary = new Dictionary<int, string>();
            BackToMenuBttn.onClick.AddListener(BackToMenu);
        }
    }

    [ClientRpc]
    private void RpcRoundTime(int time)
    {
        spendTime = time;
    }

    private void BackToMenu()
    {
        if (isServer)
        {
            MyLobbyManager.s_Singleton.ServerReturnToLobby();
        }
        else {
            MyLobbyManager.s_Singleton.SendReturnToLobby();
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        // Create GameField when Server Start
        if (isServer)
            CreateGameField();

    }

    public void SetField(int index, int _side)
    {
        if (gameOver)
            return;

        // fast fix
        if (fieldsScript[index].Owner != -1)
            return;

        fieldsScript[index].Click(_side);
        emptyFields.Remove(fieldsScript[index]);

        Calculate();
    }

    public void SetChoosenField(int index, int _side)
    {
        SetField(index, _side);
    }
    
    [ClientRpc]
    public void RpcSetRandomField(int _side)
    {
        if (gameOver)
            return;

        if (!isServer)
            return;

        int randomTurnIndex = Random.Range(0, emptyFields.Count);

        SetField(emptyFields[randomTurnIndex].Index, _side);
    }

    public void AddPlayerToDictionary(int _side, string playerName)
    {
        if (!isServer)
            return;

        Debug.LogError(_side + " " + playerName);
        playersDictionary.Add(_side, playerName);
    }

    // Create GameField Field 
    // Spawn them on the Server
    private void CreateGameField()
    {
        // create new list
        fieldsScript = new List<Field>();
        emptyFields = new List<Field>();

        // 
        turnCounter = 0;

        int _fieldIndex = 0;

        // width
        // from left to right
        for (int x = -1; x <= 1; x++)
        {
            // height
            // from top to bottom
            for (int y = 1; y >= -1; y--)
            {
                //Create field GameObject
                GameObject field = Instantiate(EmptyField, new Vector3(x * 2, y * 2, 0), Quaternion.identity);

                fieldsScript.Add(field.GetComponent<Field>());
                emptyFields.Add(field.GetComponent<Field>());

                // Set fieldIndex
                fieldsScript[_fieldIndex].Index = _fieldIndex;

                _fieldIndex++;

                // Spawn gameobject at the Server
                NetworkServer.Spawn(field);
            }
        }

    }

    // Check if Player Win
    private void Calculate()
    {
        if (fieldsScript[0].Owner == PlayerSide && fieldsScript[1].Owner == PlayerSide && fieldsScript[2].Owner == PlayerSide)
        {
            HaveWinner();
            return;
        }

        if (fieldsScript[3].Owner == PlayerSide && fieldsScript[4].Owner == PlayerSide && fieldsScript[5].Owner == PlayerSide)
        {
            HaveWinner();
            return;
        }

        if (fieldsScript[6].Owner == PlayerSide && fieldsScript[7].Owner == PlayerSide && fieldsScript[8].Owner == PlayerSide)
        {
            HaveWinner();
            return;
        }

        if (fieldsScript[0].Owner == PlayerSide && fieldsScript[3].Owner == PlayerSide && fieldsScript[6].Owner == PlayerSide)
        {
            HaveWinner();
            return;
        }

        if (fieldsScript[1].Owner == PlayerSide && fieldsScript[4].Owner == PlayerSide && fieldsScript[7].Owner == PlayerSide)
        {
            HaveWinner();
            return;
        }

        if (fieldsScript[2].Owner == PlayerSide && fieldsScript[5].Owner == PlayerSide && fieldsScript[8].Owner == PlayerSide)
        {
            HaveWinner();
            return;
        }

        if (fieldsScript[0].Owner == PlayerSide && fieldsScript[4].Owner == PlayerSide && fieldsScript[8].Owner == PlayerSide)
        {
            HaveWinner();
            return;
        }

        if (fieldsScript[2].Owner == PlayerSide && fieldsScript[4].Owner == PlayerSide && fieldsScript[6].Owner == PlayerSide)
        {
            HaveWinner();
            return;
        }

        turnCounter++;

        if (turnCounter == 9)
        {
            NoOneWin();
            return;
        }


        SetSide(PlayerSide == 1 ? 0 : 1);

        StartTime(Time.timeSinceLevelLoad);

    }
    

    private void HaveWinner()
    {
        spendTime = (int)Time.timeSinceLevelLoad;

        RpcSetRoundResults();

        //gameResultMsg = "Win " + playersDictionary[PlayerSide] + "\n " + Time.timeSinceLevelLoad +"sec";
        gameOver = true;
    }

    [ClientRpc]
    private void RpcSetRoundResults()
    {

        PlayerScript[] playersScript = FindObjectsOfType<PlayerScript>();

        for (int i = 0; i < playersScript.Length; i++)
        {
            playersScript[i].EnemyName = i == 1 ? playersScript[0].PlayerName : playersScript[1].PlayerName;
            playersScript[i].RoundResult = PlayerSide == playersScript[i].playerIndex ? 1 : 0;
            playersScript[i].ResultLabel = ResultLabel;
            playersScript[i].ShowResultLabel(spendTime);
        }

    }

    [ClientRpc]
    private void RpcSetDraw()
    {
        
        PlayerScript[] playersScript = FindObjectsOfType<PlayerScript>();

        for (int i = 0; i < playersScript.Length; i++)
        {
            playersScript[i].RoundResult = 2;
            playersScript[i].ResultLabel = ResultLabel;
            playersScript[i].ShowResultLabel(spendTime);
        }

    }

    private void ChangeResultLabel(string str)
    {
        ResultLabel.text = str;
    }

    private void NoOneWin()
    {
        spendTime = (int)Time.timeSinceLevelLoad;
        RpcSetDraw();
        gameOver = true;
    }

    private void GameOver(bool isGameOver)
    {

        ResultPanel.SetActive(isGameOver);
    }

}
