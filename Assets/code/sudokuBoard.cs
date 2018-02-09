using UnityEngine;

public class sudokuBoard : MonoBehaviour {
    
    public static float[] xLims = { -2.5f, 2.5f };
    public static float[] zLims = { -3.6f, 2.0f };
    public static float xRes    = (xLims[1] - xLims[0]) / 9;
    public static float zRes    = (zLims[1] - zLims[0]) / 9;

    public Square[,] boardSquares = new Square[9, 9];

    private int[,] defaultBoardNos = { 
                               { 0, 9, 3, 1, 0, 5, 6, 4, 0},
                               { 7, 0, 0, 0, 0, 0, 0, 0, 5},
                               { 5, 0, 1, 2, 0, 9, 3, 0, 7},
                               { 2, 0, 0, 0, 0, 0, 0, 0, 3},
                               { 0, 3, 6, 9, 0, 7, 5, 2, 0},
                               { 9, 0, 0, 0, 0, 0, 0, 0, 1},
                               { 3, 0, 2, 4, 0, 8, 1, 0, 9},
                               { 6, 0, 0, 0, 0, 0, 0, 0, 4},
                               { 0, 4, 7, 3, 0, 2, 8, 5, 0} };
    public class Square
    {
        public int number;
        public int ownedBy;
        public bool spawningPwrUp;

        public Square(int number, int ownedBy, bool spawningPwrUp)
        {
            this.number = number;
            this.ownedBy = ownedBy;
            this.spawningPwrUp = spawningPwrUp;
        }
    }

	public sudokuBoard ()
    {
        for(int i = 0; i < 9; i++)
        {
            for(int j = 0; j < 9; j++)
            {
                boardSquares[i,j] = new Square(defaultBoardNos[i,j], -1, false);
            }
        }
	}

    public Square playerPosToSquare(Transform playerPos)
    {
        Debug.Log("IndX:"+ (int)Mathf.Clamp(Mathf.Floor((playerPos.position.x - xLims[0]) / xRes), 0, 8) +
            ", IndZ:" + (int)Mathf.Clamp(Mathf.Floor((playerPos.position.z - zLims[0]) / zRes), 0, 8));
        return boardSquares[(int)Mathf.Clamp(Mathf.Floor((playerPos.position.x - xLims[0]) / xRes), 0, 8),
            (int)Mathf.Clamp(Mathf.Floor((playerPos.position.z - zLims[0]) / zRes), 0, 8)];
    }
}
