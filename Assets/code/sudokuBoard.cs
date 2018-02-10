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
    
    public static float[] xLims = { -2.5f, 2.5f };
    public static float[] zLims = { -3.6f, 2.0f };
    public static float xRes    = (xLims[1] - xLims[0]) / 9;
    public static float zRes    = (zLims[1] - zLims[0]) / 9;

    public Square[,] boardSquares = new Square[9, 9];

    public class Square
    {
        public int prefillNum;
        public int solNum;
        public int ownedBy;
        public bool spawningPwrUp;
        
        public Square(int prefillNum, int solNum, int ownedBy, bool spawningPwrUp)
        {
            this.prefillNum = prefillNum;
            this.solNum = solNum;
            this.ownedBy = ownedBy;
            this.spawningPwrUp = spawningPwrUp;
        }
    }

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

        //for (int oC = 0; oC < 9; oC=+3)
        //{
        //    for (int oR = 0; oR < 9; oR=+3)
        //    {
        //        for (int iC = 0; iC < 3; iC++)
        //        {
        //            int sR = 0;
        //            if (iC == 2) sR = Random.Range(0, 1);
        //            else         sR = Random.Range(0, 2);
        //            for (int iR = 0; iR < 3; iR++)
        //            {
        //                Debug.Log("oC:"+oC+",oR:"+oR+",");
        //                if (iC == 2 && iR == 2) boardSquares[oC+iC,oR+iR] = new Square(boardSol[oC+iC,oR+iR], boardSol[oC+iC,oR+iR], 0, false);
        //                else if (iR == sR)      boardSquares[oC+iC,oR+iR] = new Square(boardSol[oC+iC,oR+iR], boardSol[oC+iC,oR+iR], 0, false);
        //                else
        //                {
        //                    boardSquares[oC+iC, oR+iR] = new Square(0, boardSol[oC+iC, oR+iR], 0, false);
        //                }
        //            }
        //        }
        //    }
        //}
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
