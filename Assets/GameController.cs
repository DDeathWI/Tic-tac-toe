using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

    private Coroutine gameResult;

    private string gameResultMsg;

    [SyncVar(hook = "GameOver")] private bool gameOver;

    private void CalculateTimeLeft(float _time)
    {
        TimeLeft = _time;

        if (TimeLeft <= 0)
        {

            StartTime(Time.timeSinceLevelLoad);
            SetRandomField(PlayerSide);
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
        if(gameOver)
            return;

        fieldsScript[index].Click(_side);
        emptyFields.Remove(fieldsScript[index]);

        turnCounter++;
        
        Calculate();

        SetSide(_side == 1 ? 0 : 1);
    }

    public void SetChoosenField(int index, int _side)
    {
        SetField(index,_side);
    }

    public void SetRandomField(int _side)
    {
        if (!isServer)
            return;

        int randomTurnIndex = Random.Range(0, emptyFields.Count);

        SetField(emptyFields[randomTurnIndex].Index, _side);
    }


    // Create GameField Field 
    // Spawn them on the Server
    private void CreateGameField()
    {
        // create new list
        fieldsScript = new List<Field>();
        emptyFields = new List<Field>();

        gameResultMsg = "";
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

    // Player Turn
    //public void Turn(int fieldIndex)
    //{
    //    fieldsScript[fieldIndex].Owner = PlayerSide;

    //    Calculate();    
    //}
    
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

        if (turnCounter == 9)
        {
            NoOneWin();
            return;
        }

        StartTime(Time.timeSinceLevelLoad);

    }

    private IEnumerator ShowResult()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        MyLobbyManager.s_Singleton.ServerReturnToLobby();
    }

    private void HaveWinner()
    {
        gameResultMsg = "Win" + PlayerSide;
        gameOver = true;
    }
    

    private void NoOneWin()
    {
        gameResultMsg = "No Winner";
        gameOver = true;
    }

    private void GameOver(bool isGameOver)
    {
        gameResult = StartCoroutine(ShowResult());
    }

}
