﻿using System.Collections.Generic;
using UnityEngine;
/*
    { 4, 3, 5, 2, 6, 9, 7, 8, 1},
    { 6, 8, 2, 5, 7, 1, 4, 9, 3},
    { 1, 9, 7, 8, 3, 4, 5, 6, 2},
    { 8, 2, 6, 1, 9, 5, 3, 4, 7},
    { 3, 7, 4, 6, 8, 2, 9, 1, 5},
    { 9, 5, 1, 7, 4, 3, 6, 2, 8},
    { 5, 1, 9, 3, 2, 6, 8, 7, 4},
    { 2, 4, 8, 9, 5, 7, 1, 3, 6},
    { 7, 6, 3, 4, 1, 8, 2, 5, 9};
*/
public class SudokuBoard {
    
    public static float[] xLims = { -2.5f, 2.5f };
    public static float[] zLims = { -3.8f, 3.2f };
    public static float xRes    = (xLims[1] - xLims[0]) / 9;
    public static float zRes    = (zLims[1] - zLims[0]) / 9;

    public Square[,] boardSquares = new Square[9, 9];

	public SudokuBoard ()
    {
        int[,] boardSol = new int[,] {
            { 4, 3, 5, 2, 6, 9, 7, 8, 1},
            { 6, 8, 2, 5, 7, 1, 4, 9, 3},
            { 1, 9, 7, 8, 3, 4, 5, 6, 2},
            { 8, 2, 6, 1, 9, 5, 3, 4, 7},
            { 3, 7, 4, 6, 8, 2, 9, 1, 5},
            { 9, 5, 1, 7, 4, 3, 6, 2, 8},
            { 5, 1, 9, 3, 2, 6, 8, 7, 4},
            { 2, 4, 8, 9, 5, 7, 1, 3, 6},
            { 7, 6, 3, 4, 1, 8, 2, 5, 9} };

        int[,] boardFill = new int[,] {
            { 4, 3, 5, 2, 6, 0, 7, 0, 1},
            { 0, 0, 2, 0, 7, 0, 0, 9, 0},
            { 0, 0, 7, 0, 0, 4, 5, 0, 2},
            { 8, 2, 0, 1, 0, 0, 0, 4, 0},
            { 0, 0, 4, 6, 0, 2, 9, 0, 0},
            { 0, 5, 0, 0, 0, 3, 0, 2, 8},
            { 0, 0, 9, 3, 0, 0, 0, 7, 4},
            { 0, 4, 0, 0, 5, 0, 0, 3, 6},
            { 7, 0, 3, 0, 1, 8, 0, 0, 0} };

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                boardSquares[i, j] = new Square(boardFill[i, j], boardSol[i, j], 0, false);
            }
        }
    }

    public Square objTransformToSquare(Transform playerPos)
    {
        if (playerPos.position.x < xLims[0] || playerPos.position.x > xLims[1] ||
            playerPos.position.z < zLims[0] || playerPos.position.z > zLims[1])
        {
            return null;
        }
        return boardSquares[(int)Mathf.Clamp(Mathf.Floor((playerPos.position.x - xLims[0]) / xRes), 0, 8),
            (int)Mathf.Clamp(Mathf.Floor((playerPos.position.z - zLims[0]) / zRes), 0, 8)];
    }

}
