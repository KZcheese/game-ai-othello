using System.Collections.Generic;
using UnityEngine;

public class NegamaxABAI : NegamaxAI {
    private const uint MaxDepth = 5;

    private float boardValue(BoardSpace[][] currentBoard, uint turnNumber) {
        float blackMod = turnNumber % 2 == 0 ? 1 : -1;
        float whiteMod = turnNumber % 2 == 1 ? 1 : -1;
        float rawSpaces = countSpaces(currentBoard, BoardSpace.BLACK) * blackMod + 
                          countSpaces(currentBoard, BoardSpace.WHITE) * whiteMod;

        float corners = (currentBoard[0][0] == BoardSpace.BLACK ? 1.0f : 0.0f) * blackMod +
                        (currentBoard[0][0] == BoardSpace.WHITE ? 1.0f : 0.0f) * whiteMod +
                        (currentBoard[7][0] == BoardSpace.BLACK ? 1.0f : 0.0f) * blackMod +
                        (currentBoard[7][0] == BoardSpace.WHITE ? 1.0f : 0.0f) * whiteMod +
                        (currentBoard[0][7] == BoardSpace.BLACK ? 1.0f : 0.0f) * blackMod +
                        (currentBoard[0][7] == BoardSpace.WHITE ? 1.0f : 0.0f) * whiteMod +
                        (currentBoard[7][7] == BoardSpace.BLACK ? 1.0f : 0.0f) * blackMod +
                        (currentBoard[7][7] == BoardSpace.WHITE ? 1.0f : 0.0f) * whiteMod;
        
        return rawSpaces * 1.0f + corners * 100.0f;
    } 

    //main recursive negamax function
    private KeyValuePair<float, KeyValuePair<int, int>>
        NegamaxABFunction(BoardSpace[][] currentBoard, uint currentDepth, float alpha, float beta) {
        uint turnNumber = color.Equals(BoardSpace.BLACK) ? 0 : 1 + currentDepth;
        List<KeyValuePair<int, int>> currentValidMoves = BoardScript.GetValidMoves(currentBoard, turnNumber);

        //if game is over we are done recursing
        if (currentValidMoves.Count == 0 || currentDepth == MaxDepth) {
            KeyValuePair<int, int> finalMove = new KeyValuePair<int, int>(-1, -1);
            return new KeyValuePair<float, KeyValuePair<int, int>>(boardValue(currentBoard, turnNumber), finalMove);
        }

        KeyValuePair<int, int> bestMove;
        float bestScore = Mathf.NegativeInfinity;

        //loop through all possible moves
        foreach (KeyValuePair<int, int> move in currentValidMoves) {
            BoardSpace[][] newBoard = copyBoard(currentBoard);
            SimulateMove(ref newBoard, move.Key, move.Value, turnNumber);

            //recurse
            KeyValuePair<float, KeyValuePair<int, int>> recursionResult =
                NegamaxABFunction(newBoard, currentDepth + 1, -1 * beta, -1 * alpha);

            float currentScore = -1 * recursionResult.Key;

            //if you found a new better score update score and move
            if (currentScore > bestScore) {
                bestScore = currentScore;
                bestMove = move;
            }

            if (currentScore > alpha) {
                alpha = currentScore;
            }

            if (alpha >= beta) {
                break;
            }
        }

        return new KeyValuePair<float, KeyValuePair<int, int>>(bestScore, bestMove);
    }

    public override KeyValuePair<int, int> makeMove(List<KeyValuePair<int, int>> availableMoves,
        BoardSpace[][] currentBoard) {
        return NegamaxABFunction(currentBoard, 0, Mathf.NegativeInfinity, Mathf.Infinity).Value;
    }
}