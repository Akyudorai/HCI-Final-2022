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
            // - Horizontal - DONE
            // - Vertical - 
            // - Diagonal

        //bool hCheck = CheckHorizontal();
        //bool vCheck = CheckVertical();
        bool result = CheckWin(board);
        Debug.Log(result);
       
    }

    private bool CheckWin(int[,] matrix) 
    {   
        /* This method checks if there are four identical elements in a matrix either
     horizontally, vertically, or diagonally */

        /* We traverse each element in the matrix */
        for( int row = 0; row < 5; row++ )
        {
            for( int col = 0; col < 6; col++ )
            {

                // This is the current element in our matrix
                int element = matrix[row,col];

                if (element != 0) 
                {
                    /* If there are 3 elements remaining to the right of the current element's
                    position and the current element equals each of them, then return true */
                    if( col <= 6-4 && element == matrix[row,col+1] && element == matrix[row,col+2] && element == matrix[row,col+3] )
                    return true;

                    /* If there are 3 elements remaining below the current element's position
                    and the current element equals each of them, then return true */
                    if( row <= 5 - 4 && element == matrix[row+1,col] && element == matrix[row+2,col] && element == matrix[row+3,col] )
                    {
                        return true;
                    }


                    /* If we are in a position in the matrix such that there are diagonals
                    remaining to the bottom right of the current element, then we check */
                    if( row <= 5-4 && col <= 6-4 )
                    {
                    // If the current element equals each element diagonally to the bottom right
                    if( element == matrix[row+1,col+1] && element == matrix[row+2,col+2] && element == matrix[row+3,col+3] )
                        return true;
                    }


                    /* If we are in a position in the matrix such that there are diagonals
                    remaining to the bottom left of the current element, then we check */
                    if( row <= 5-4 && col >= 6-4 )
                    {
                    // If the current element equals each element diagonally to the bottom left
                    if( element == matrix[row+1,col-1] && element == matrix[row+2,col-2] && element == matrix[row+3,col-3] )
                        return true;
                    }
                }

                

            }
        }

        /* If all the previous return statements failed, then we found no such
        patterns of four identical elements in this matrix, so we return false */
        return false;
    }
    

    private bool CheckVertical() 
    {
        int columnPrevious = -1;
        int columnCount = 0;

        int i = 0;
        int j = 0;
        while (i < 6) 
        {
            j = 0; 
            columnPrevious = -1;
            while (j < 5) 
            {
                if (columnPrevious == -1) {
                    columnPrevious = board[i,j];
                    if (columnPrevious != 0) {
                        columnCount++;
                    }
                }

                else {
                    if (board[i,j] == columnPrevious && columnPrevious != 0) {
                        columnCount++;
                        if (columnCount == 4) {
                            Debug.Log("Match Found along Vertical");
                            return true;
                        }
                    } else {
                        columnCount = 0;
                    }
                }

                j++;
            }
            i++;
        }  

        return false;      
    }

    private bool CheckHorizontal() 
    {
        int rowPrevious = -1;
        int rowCount = 0;

        int i = 0; 
        int j = 0;
        string output = "Board State";
        while (i < 5) // Column (Up-Down Check)
        {           
            j = 0;
            output += "\n";

            Debug.Log("New line");

            rowPrevious = -1;
            while (j < 6) // Row (Left-Right Check)
            {
                output += "[" + board[j,i] + "] ";
                
                // If this is the first column of the row, set previous to whatever it is.
                if (rowPrevious == -1) {
                    rowPrevious = board[j,i];
                    if (rowPrevious != 0) {
                        rowCount++;
                    }
                }

                else 
                {
                    if (board[j,i] == rowPrevious && rowPrevious != 0) {
                        rowCount++;
                        if (rowCount == 4) {
                            Debug.Log("Match Found along horizontal");
                            return true;
                        }
                    } else {
                        rowCount = 0;
                    }
                }

                Debug.Log("Board["+j+","+i+"]");
                j++;
            }

            i++;
        }

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
