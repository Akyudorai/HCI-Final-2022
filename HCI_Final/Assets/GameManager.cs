using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game Data")]
    public bool columnSelected = false;
    public int currentPlayer = 1;
    public int currentColumn = 1;

    private int columnLength = 5; // Up and Down
    private int rowLength = 6; // Left and Right
    public int[,] board; // [x][y]

    [Header("References")]
    public GameObject chipPrefab;
    public GameObject playerChip = null;
    public Transform[] columnStartPos = new Transform[6];

    [Header("UI")]
    public GameObject startScreen = null;
    public GameObject endScreen = null;
    public TMP_Text winDisplay = null;    

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
        VoiceCommands.Commands.Add("slot", new Action<int, string>(Column));
        VoiceCommands.Commands.Add("confirm", new Action<int, string>(Confirm));
        VoiceCommands.Commands.Add("set", new Action<int, string>(Confirm));        
        VoiceCommands.Commands.Add("cancel", new Action<int, string>(Cancel));
        
        VoiceCommands.Commands.Add("1", new Action<int, string>(C1));
        VoiceCommands.Commands.Add("one", new Action<int, string>(C1));
        VoiceCommands.Commands.Add("a", new Action<int, string>(C1));
        VoiceCommands.Commands.Add("2", new Action<int, string>(C2));
        VoiceCommands.Commands.Add("two", new Action<int, string>(C2));
        VoiceCommands.Commands.Add("b", new Action<int, string>(C2));
        VoiceCommands.Commands.Add("3", new Action<int, string>(C3));
        VoiceCommands.Commands.Add("three", new Action<int, string>(C3));
        VoiceCommands.Commands.Add("c", new Action<int, string>(C3));
        VoiceCommands.Commands.Add("4", new Action<int, string>(C4));
        VoiceCommands.Commands.Add("four", new Action<int, string>(C4));
        VoiceCommands.Commands.Add("d", new Action<int, string>(C4));
        VoiceCommands.Commands.Add("5", new Action<int, string>(C5));
        VoiceCommands.Commands.Add("five", new Action<int, string>(C5));
        VoiceCommands.Commands.Add("e", new Action<int, string>(C5));
        VoiceCommands.Commands.Add("6", new Action<int, string>(C6));
        VoiceCommands.Commands.Add("six", new Action<int, string>(C6));
        VoiceCommands.Commands.Add("f", new Action<int, string>(C6));
        
        VoiceCommands.Commands.Add("play", new Action<int, string>(Begin));
        VoiceCommands.Commands.Add("begin", new Action<int, string>(Begin));
    }

    public void Begin(int index, string last) 
    {
        startScreen.SetActive(false);
    }

    public void InitializeBoard(int index, string last)
    {
        board = new int[rowLength, columnLength];

        for (int i = 0; i < rowLength; i++) {
            for (int j = 0; j < columnLength; j++) 
            {
                board[i,j] = 0;
            }
        }
    }

    public void CheckBoardState() 
    {        
        bool result = CheckWin(board);

        if (result) 
        {
            endScreen.SetActive(true);
            winDisplay.text = "Player " + currentPlayer + " wins the game!";
        }
    }

    private bool CheckWin(int[,] matrix) 
    {   
        // Traverse each element in the matrix 
        for( int row = 0; row < rowLength; row++ )
        {
            for( int col = 0; col < columnLength; col++ )
            {
                // This is the current element in our matrix
                int element = matrix[row,col];

                if (element != 0) // If it is not empty
                {
                    /* If there are 3 elements remaining to the right of the current element's
                    position and the current element equals each of them, then return true */
                    if( col <= rowLength-4 && element == matrix[row,col+1] && element == matrix[row,col+2] && element == matrix[row,col+3]) {
                        return true;
                    }
                    

                    /* If there are 3 elements remaining below the current element's position
                    and the current element equals each of them, then return true */
                    if( row <= columnLength-4 && element == matrix[row+1,col] && element == matrix[row+2,col] && element == matrix[row+3,col])
                    {
                        return true;
                    }


                    /* If we are in a position in the matrix such that there are diagonals
                    remaining to the bottom right of the current element, then we check */
                    if( row <= columnLength-4 && col <= rowLength-4 )
                    {
                        // If the current element equals each element diagonally to the bottom right
                        if( element == matrix[row+1,col+1] && element == matrix[row+2,col+2] && element == matrix[row+3,col+3]) {
                            return true;
                        }                            
                    }


                    /* If we are in a position in the matrix such that there are diagonals
                    remaining to the bottom left of the current element, then we check */
                    if( row <= columnLength-4 && col >= rowLength-4)
                    {
                        // If the current element equals each element diagonally to the bottom left
                        if( element == matrix[row+1,col-1] && element == matrix[row+2,col-2] && element == matrix[row+3,col-3]) {
                            return true;
                        }   
                            
                    }
                }

                

            }
        }

        /* If all the previous return statements failed, then we found no such
        patterns of four identical elements in this matrix, so we return false */
        return false;
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
