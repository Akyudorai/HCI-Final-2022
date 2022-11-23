using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Game Data")]
    public bool columnSelected = false;
    public int currentPlayer = 1;
    public int currentColumn = 1;
    public int[,] board = new int[6,5]; // [x][y]


    [Header("References")]
    public GameObject chipPrefab;
    public GameObject playerChip = null;
    public Transform[] columnStartPos = new Transform[6];

    private void Start() 
    {
        InitializeBoard(0, "");
        LoadGameCommands();
        currentPlayer = 1;
        currentColumn = 1;
    }

    public void LoadGameCommands()
    {
        VoiceCommands.Commands.Add("reset", new Action<int, string>(InitializeBoard));
        VoiceCommands.Commands.Add("column", new Action<int, string>(Column));
        VoiceCommands.Commands.Add("confirm", new Action<int, string>(Confirm));
        VoiceCommands.Commands.Add("set", new Action<int, string>(Confirm));
        VoiceCommands.Commands.Add("cancel", new Action<int, string>(Cancel));
        VoiceCommands.Commands.Add("1", new Action<int, string>(C1));
        VoiceCommands.Commands.Add("one", new Action<int, string>(C1));
        VoiceCommands.Commands.Add("2", new Action<int, string>(C2));
        VoiceCommands.Commands.Add("two", new Action<int, string>(C2));
        VoiceCommands.Commands.Add("3", new Action<int, string>(C3));
        VoiceCommands.Commands.Add("three", new Action<int, string>(C3));
        VoiceCommands.Commands.Add("4", new Action<int, string>(C4));
        VoiceCommands.Commands.Add("four", new Action<int, string>(C4));
        VoiceCommands.Commands.Add("5", new Action<int, string>(C5));
        VoiceCommands.Commands.Add("five", new Action<int, string>(C5));
        VoiceCommands.Commands.Add("6", new Action<int, string>(C6));
        VoiceCommands.Commands.Add("six", new Action<int, string>(C6));
    }

    public void InitializeBoard(int index, string last)
    {
        for (int i = 0; i < 6; i++) {
            for (int j = 0; j < 5; j++) 
            {
                board[i,j] = 0;
            }
        }
    }

    public void CheckBoardState() 
    {
        bool gameComplete = false;

        // TODO: Need to add functionality to detect if there is 4 of the same color chip along the
            // - Horizontal
            // - Vertical
            // - Diagonal

        int i = 0; 
        int j = 0;
        string output = "Board State";
        while (i < 5) 
        {   
            j = 0;
            output += "\n";

            while (j < 6) 
            {
                output += "[" + board[j,i] + "] ";

                j++;
            }

            i++;
        }
 
        Debug.Log(output);
    }

    public void LoadChip(int index)
    {
        if (playerChip == null) {
            playerChip = GameObject.Instantiate(chipPrefab, columnStartPos[index-1].position, Quaternion.identity);    
            playerChip.GetComponent<Rigidbody2D>().gravityScale = 0;     
            playerChip.GetComponent<SpriteRenderer>().color = (currentPlayer == 1) ? Color.blue : Color.red;   
        } else {
            playerChip.transform.position = columnStartPos[index-1].position;
        }

        columnSelected = true;
    }

    public bool SetChip(int index) 
    {
        for (int i = 4; i >= 0; i--) 
        {       
            Debug.Log("Current Column: " + (currentColumn-1));        
            if (board[currentColumn-1, i] == 0) 
            {
                board[currentColumn-1, i] = currentPlayer;                
                return true;
            }            
        }

        Debug.Log("Column is full, cannot place here.");
        return false;
    }

    #region Game Commands

        private void Column(int index, string last) 
        {
            // Don't really do anything on its own
        }

        private void C1(int index, string last) 
        {
            if (last == "column") 
            {   
                currentColumn = 1;
                LoadChip(1);
            }
            
        }

        private void C2(int index, string last) 
        {
            if (last == "column") 
            { 
                currentColumn = 2;
                LoadChip(2);
            }
        }

        private void C3(int index, string last) 
        {
           if (last == "column") 
            { 
                currentColumn = 3;
                LoadChip(3);
            }
        }

        private void C4(int index, string last) 
        {
            if (last == "column") 
            { 
                currentColumn = 4;
                LoadChip(4);
            }
        }

        private void C5(int index, string last) 
        {
            if (last == "column") 
            {   
                currentColumn = 5;
                LoadChip(5);
            }
        }

        private void C6(int index, string last) 
        {
            if (last == "column") 
            {  
                currentColumn = 6;
                LoadChip(6);
            }
        }

        private void Confirm(int index, string last) 
        {
            if (columnSelected == true) 
            {                   
                if (SetChip(currentColumn) == true) {
                    Debug.Log("Column Confirmed\nMaking move and passing turn");
                    currentPlayer = (currentPlayer == 1) ? 2 : 1;                
                    currentColumn = 0;
                    columnSelected = false;
                    playerChip.GetComponent<Rigidbody2D>().gravityScale = 1;
                    playerChip = null;
                    CheckBoardState();
                }
            }
        }

        private void Cancel(int index, string last) 
        {
            if (columnSelected == true) 
            {
                Debug.Log("Column Cancelled\nReturning to column selection.");
                columnSelected = false;
            }
        }

    #endregion

    public static void Quit(int order, string prevKeyword) {

        // Exit the application
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif 

        Application.Quit();
    }
}
