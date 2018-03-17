using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour {

    // Create Singleton
    public static GameController instance;

    // Player Side X or O
    [SyncVar (hook ="SetSide")]public int PlayerSide;

    // Empty Field Gamobject
    public GameObject EmptyField;

    // List Script for action with gamefields
    private List<Field> fieldsScript;

    // Keep turn number for check if GameEnd
    private int turnCounter;

    private void SetSide(int _side)
    {
        PlayerSide = _side;
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

        if (isServer)
            CreateGameField();

        // Create GameField when Server Start


    }


    
    public void SetField(int index, int _side)
    {
        fieldsScript[index].Click(_side);

        turnCounter++;
        
        Calculate();

        PlayerSide = _side == 1 ? 0 : 1;
    }



    // Create GameField Field 
    // Spawn them on the Server
    private void CreateGameField()
    {
        // create new list
        fieldsScript = new List<Field>();

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
            GameOver();
            return;
        }

        if (fieldsScript[3].Owner == PlayerSide && fieldsScript[4].Owner == PlayerSide && fieldsScript[5].Owner == PlayerSide)
        {
            GameOver();
            return;
        }

        if (fieldsScript[6].Owner == PlayerSide && fieldsScript[7].Owner == PlayerSide && fieldsScript[8].Owner == PlayerSide)
        {
            GameOver();
            return;
        }

        if (fieldsScript[0].Owner == PlayerSide && fieldsScript[3].Owner == PlayerSide && fieldsScript[6].Owner == PlayerSide)
        {
            GameOver();
            return;
        }

        if (fieldsScript[1].Owner == PlayerSide && fieldsScript[4].Owner == PlayerSide && fieldsScript[7].Owner == PlayerSide)
        {
            GameOver();
            return;
        }

        if (fieldsScript[2].Owner == PlayerSide && fieldsScript[5].Owner == PlayerSide && fieldsScript[8].Owner == PlayerSide)
        {
            GameOver();
            return;
        }

        if (fieldsScript[0].Owner == PlayerSide && fieldsScript[4].Owner == PlayerSide && fieldsScript[8].Owner == PlayerSide)
        {
            GameOver();
            return;
        }

        if (fieldsScript[2].Owner == PlayerSide && fieldsScript[4].Owner == PlayerSide && fieldsScript[6].Owner == PlayerSide)
        {
            GameOver();
            return;
        }

        if (turnCounter == 9)
        {
            NoOneWin();
        }
    }

    private void GameOver()
    {
        Debug.LogError("Win " + PlayerSide);
    }
    

    private void NoOneWin()
    {

    }

}
