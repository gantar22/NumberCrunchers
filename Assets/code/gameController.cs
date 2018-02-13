using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour {

	public List<GameObject> players;
    public GameObject autoFillTilePrefab;
    public SudokuBoard sBoard;

    private bool gameEnd = false;
    private float initWait = 20.0f;
    private float autoFillTime = 10.0f;

	void Start () {
        sBoard = new SudokuBoard();
        StartCoroutine(AutoFillTiles());
    }
	
	void Update ()
    {
        //foreach (GameObject player in players)
        //{
        //    Debug.Log("P" + players.IndexOf(player) + ": " + sBoard.objTransformToSquare(player.transform).number);
        //}
    }

    IEnumerator AutoFillTiles()
    {
        yield return new WaitForSeconds(initWait);
        while (!gameEnd)
        {
            Square autoFillSquare = sBoard.FillRandSquare();
            if (autoFillSquare != null)
            {
                Debug.Log("FillSquare:R" + autoFillSquare.row + "C:" + autoFillSquare.col);
                Vector3 fillPos = new Vector3(autoFillSquare.xMinLim + (sBoard.xRes / 2), -0.6f, autoFillSquare.zMinLim + (sBoard.zRes / 2));
                Instantiate(autoFillTilePrefab, fillPos, new Quaternion());
                yield return new WaitForSeconds(autoFillTime);
            }
            else
            {
                Debug.Log("End");
                gameEnd = true;
            }
        }
    }
}
