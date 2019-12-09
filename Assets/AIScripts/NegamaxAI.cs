﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NegamaxAI : AIScript {
    uint maxDepth = 4;

    protected int countSpaces(BoardSpace[][] currentBoard, BoardSpace spaceType) {
        return currentBoard.SelectMany(row => row).Count(space => space.Equals(spaceType));
    }

    protected BoardSpace[][] copyBoard(BoardSpace[][] oldBoard) {
        return oldBoard.Select(a => a.ToArray()).ToArray();
    }

    //main recursive negamax function
    private KeyValuePair<float, KeyValuePair<int, int>>
        NegamaxFunction(BoardSpace[][] currentBoard, uint currentDepth) {
        uint turnNumber = color.Equals(BoardSpace.BLACK) ? 0 : 1 + currentDepth;
        List<KeyValuePair<int, int>> currentValidMoves = BoardScript.GetValidMoves(currentBoard, turnNumber);

        //if game is over we are done recursing
        if (currentValidMoves.Count == 0 || currentDepth == maxDepth) {
//            int blackCount = countSpaces(currentBoard, BoardSpace.BLACK);
            int myCount = countSpaces(currentBoard, turnNumber % 2 == 0 ? BoardSpace.BLACK : BoardSpace.WHITE);

            KeyValuePair<int, int> finalMove = new KeyValuePair<int, int>(-1, -1);
            return new KeyValuePair<float, KeyValuePair<int, int>>(1 * myCount, finalMove);
        }

        KeyValuePair<int, int> bestMove;
        float bestScore = -1 * Mathf.Infinity;

        //loop through all possible moves
        foreach (KeyValuePair<int, int> move in currentValidMoves) {
            BoardSpace[][] newBoard = copyBoard(currentBoard);
            SimulateMove(ref newBoard, move.Key, move.Value, turnNumber);

            //recurse
            KeyValuePair<float, KeyValuePair<int, int>> recursionResult = NegamaxFunction(newBoard, currentDepth + 1);

            float currentScore = -1 * recursionResult.Key;

            //if you found a new better score update score and move
            if (currentScore > bestScore) {
                bestScore = currentScore;
                bestMove = move;
            }
        }

        return new KeyValuePair<float, KeyValuePair<int, int>>(bestScore, bestMove);
    }

    public override KeyValuePair<int, int> makeMove(List<KeyValuePair<int, int>> availableMoves,
        BoardSpace[][] currentBoard) {
        return NegamaxFunction(currentBoard, 0).Value;
    }

//from 
    protected void SimulateMove(ref BoardSpace[][] currentBoard, int x, int y, uint turnNumber) {
        if (turnNumber % 2 == 0) {
            currentBoard[y][x] = BoardSpace.BLACK;
        }
        else {
            currentBoard[y][x] = BoardSpace.WHITE;
        }

        List<KeyValuePair<int, int>> changedSpaces =
            BoardScript.GetPointsChangedFromMove(currentBoard, turnNumber, x, y);
        foreach (KeyValuePair<int, int> space in changedSpaces) {
            if (turnNumber % 2 == 0) {
                currentBoard[space.Key][space.Value] = BoardSpace.BLACK;
            }
            else {
                currentBoard[space.Key][space.Value] = BoardSpace.WHITE;
            }
        }

        ++turnNumber;
    }
}