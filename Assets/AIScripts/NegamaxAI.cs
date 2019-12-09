using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegamaxAI : AIScript {

    BoardScript reversiScript;
    int currentDepth = 0;
    int maxDepth = 3;
    uint turnNumber;
    readonly KeyValuePair<float, KeyValuePair<int, int>> gameOverKeyValue;
    int recursedScore = 0;

    //starter function for Negamax function
    public override KeyValuePair<int, int> StarterFunction(BoardSpace[][] currentBoard, BoardScript scriptReference, uint turnNum)
    {
        reversiScript = scriptReference;
        turnNumber = turnNum;
        return NegamaxFunction(currentBoard, 0).Value;
    }

    //main recursive negamax function
    public KeyValuePair<float, KeyValuePair<int, int>> NegamaxFunction(BoardSpace[][] currentBoard, int currentDepth)
    {
        List<KeyValuePair<int, int>> currentValidMoves = reversiScript.GetValidMoves(currentBoard, turnNumber);
        //if game is over we are done recursing
        if(currentValidMoves.Count == 0 || currentDepth == maxDepth)
        {
            int blackCount = 0;
            int whiteCount = 0;
            foreach (BoardSpace[] row in currentBoard)
            {
                foreach (BoardSpace space in row)
                {
                    switch (space)
                    {
                        case (BoardSpace.BLACK):
                            blackCount++;
                            break;
                        case (BoardSpace.WHITE):
                            whiteCount++;
                            break;
                    }
                }
            }
            KeyValuePair<int, int> finalMove = new KeyValuePair<int, int>(-1, -1);
            KeyValuePair<float, KeyValuePair<int, int>> ret = new KeyValuePair<float, KeyValuePair<int, int>>(whiteCount, finalMove);
            return ret;
        }

        KeyValuePair<int, int> bestMove;
        float bestScore = -Mathf.Infinity;

        //loop through all possible moves
        foreach (KeyValuePair<int, int> move in currentValidMoves)
        {
            BoardSpace[][] newBoard = currentBoard;
            PlacePiece(ref newBoard, move.Key, move.Value);

            //recurse
            KeyValuePair<float, KeyValuePair<int, int>> recursionResult = NegamaxFunction(newBoard, currentDepth + 1);

            float currentScore = -recursionResult.Key;

            //if you found a new better score update score and move
            if(currentScore > bestScore)
            {
                bestScore = currentScore;
                bestMove = move;
            }
        }

        return new KeyValuePair<float, KeyValuePair<int, int>>(bestScore, bestMove);


    }

    public override KeyValuePair<int, int> makeMove(List<KeyValuePair<int, int>> availableMoves,
        BoardSpace[][] currentBoard)
    {
        return availableMoves[Random.Range(0, availableMoves.Count)];
    }

    //from 
    public void PlacePiece(ref BoardSpace[][] currentBoard, int x, int y)
    {
        if (turnNumber % 2 == 0)
        {
            currentBoard[y][x] = BoardSpace.BLACK;
        }
        else
        {
            currentBoard[y][x] = BoardSpace.WHITE;
        }
        List<KeyValuePair<int, int>> changedSpaces = reversiScript.GetPointsChangedFromMove(currentBoard, turnNumber, x, y);
        foreach (KeyValuePair<int, int> space in changedSpaces)
        {
            if (turnNumber % 2 == 0)
            {
                currentBoard[space.Key][space.Value] = BoardSpace.BLACK;
            }
            else
            {
                currentBoard[space.Key][space.Value] = BoardSpace.WHITE;
            }
        }
        ++turnNumber;
    }
}
