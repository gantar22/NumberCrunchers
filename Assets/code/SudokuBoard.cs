using System.Collections.Generic;
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
    
    public float[] xLims = { -4.17f, 3.52f };
    public float[] zLims = { -3.92f, 3.8f };
    public float xRes;
    public float zRes;

    public Square[,] boardSquares = new Square[9, 9];


    public SudokuBoard ()
    {
        xRes = (xLims[1] - xLims[0]) / 9;
        zRes = (zLims[1] - zLims[0]) / 9;

        int[,] boardSol = new int[,] {
            { 4, 3, 5,   2, 6, 9,   7, 8, 1},
            { 6, 8, 2,   5, 7, 1,   4, 9, 3},
            { 1, 9, 7,   8, 3, 4,   5, 6, 2},

            { 8, 2, 6,   1, 9, 5,   3, 4, 7},
            { 3, 7, 4,   6, 8, 2,   9, 1, 5},
            { 9, 5, 1,   7, 4, 3,   6, 2, 8},

            { 5, 1, 9,   3, 2, 6,   8, 7, 4},
            { 2, 4, 8,   9, 5, 7,   1, 3, 6},
            { 7, 6, 3,   4, 1, 8,   2, 5, 9} };

        int[,] boardFill = new int[,] {
            { 4, 3, 5, 2, 6, 0,  7,  0,  1},
            { 0, 0, 2, 0, 7, 0,  0,  9,  0},
            { 0, 0, 7, 0, 0, 4,  5,  0,  2},
            { 8, 2, 0, 1, 0, 0,  0,  4,  0},
            { 0, 0, 4, 6, 0, 2,  9,  0,  0},
            { 0, 5, 0, 0, 0, 3,  0,  2,  8},
            { 0, 0, 9, 3, 0, 0,  0,  7,  4},
            { 0, 4, 0, 0, 5, 0,  0,  3,  6},
            { 7, 0, 3, 0, 1, 8,  0,  0,  0} };

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                boardSquares[8-i, j] = new Square(boardFill[8-i, j],
                                                boardSol[8-i, j],
                                                0,
                                                false,
                                                8-i,
                                                j,
                                                (xLims[0]+j*xRes),
                                                (zLims[0]+i*zRes));
            }
        }
    }

    public Square FillRandSquare()
    {
        List<Square> unfilledSquares = new List<Square>();
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (boardSquares[i,j].fillNum == 0)
                {
                    unfilledSquares.Add(boardSquares[i, j]);
                }
            }
        }
        if (unfilledSquares.Count == 0) return null;
        Square squareToFill = unfilledSquares[Random.Range(0, unfilledSquares.Count)];
        squareToFill.fillNum = squareToFill.solNum;
        squareToFill.ownedBy = -1;
        return squareToFill;
    }

    public Square TransformToSquare(Transform tilePos)
    {
        if (tilePos.position.x < xLims[0] || tilePos.position.x > xLims[1] ||
            tilePos.position.z < zLims[0] || tilePos.position.z > zLims[1])
        {
            return null;
        }
        return boardSquares[(int)(8 - Mathf.Clamp(Mathf.Floor((tilePos.position.z - zLims[0]) / zRes), 0, 8)),
                            (int)Mathf.Clamp(Mathf.Floor((tilePos.position.x - xLims[0]) / xRes), 0, 8)];
    }

    public void SetSquare(Square sqr)
    {
        boardSquares[sqr.row, sqr.col] = sqr;
    }

    public List<int> GetWinnerList()
    {
        List<int> winList = new List<int>();
        List<int> tileCount = new List<int>(new int[] {0, 0, 0, 0});
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                int own = boardSquares[i, j].ownedBy;
                Debug.Log(own);
                if (own != 0 && own != -1) tileCount[own-1]++; 
            }
        }
        List<int> sortList = new List<int>(tileCount);
        sortList.Sort();
        for (int i = 0; i < 4; i++)
        {
            if (sortList[0] == tileCount[i]) winList.Add(i+1); 
        }
        Debug.Log(sortList.ToString());
        Debug.Log(winList.ToString());
        return winList;
    }

    public int totalTiles(){
        int r = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (boardSquares[i, j].ownedBy > 0) r++;
            }
        }
        return r;
    }

    public int tilesForPlayer(int id){
        int r = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (boardSquares[i, j].ownedBy == id) r++;
            }
        }
        return r;
    }
}
