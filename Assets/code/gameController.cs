using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour {

    [SerializeField] Image[] playerWinImg;

    public List<GameObject> players;
    public GameObject autoFillTilePrefab;
    public SudokuBoard sBoard;

    private bool gameEnd = false;
    //private float initWait = 2.0f;
    //private float autoFillTime = 0.05f;
    private float initWait = 20.0f;
    private float autoFillTime = 10.0f;

    void Start () {
        sBoard = new SudokuBoard();
        StartCoroutine(AutoFillTiles());
    }

    void Update()
    {
        if (gameEnd)
        {
            List<int> winners = sBoard.GetWinnerList();
            for (int i = 0; i < winners.Count; i++)
            {
                playerWinImg[i].enabled = true;
                playerWinImg[i].sprite = players[winners[i]-1].GetComponent<PMove>().winSprite;
            }
        }
    }

    IEnumerator AutoFillTiles()
    {
        yield return new WaitForSeconds(initWait);
        while (!gameEnd)
        {
            Square autoFillSquare = sBoard.FillRandSquare();
            if (autoFillSquare != null)
            {
                Vector3 fillPos = new Vector3(autoFillSquare.xMinLim + (sBoard.xRes / 2), -0.6f, autoFillSquare.zMinLim + (sBoard.zRes / 2));
                Instantiate(autoFillTilePrefab, fillPos, new Quaternion());
                yield return new WaitForSeconds(autoFillTime);
            }
            else
            {
                gameEnd = true;
            }
        }
    }

    static public void QuitGame(){
    	Application.Quit();
    }
}
