using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XInputDotNetPure;


public class GameController : MonoBehaviour {

    [SerializeField] Image[] playerWinImg;
    [SerializeField] Sprite[] tileNumSprites;

    public List<GameObject> players;
    public GameObject autoFillTilePrefab;
    public SudokuBoard sBoard;
    [SerializeField]
    public bool autoFillGotFilled;

    private bool gameEnd = false;
    //private float initWait = 0.05f;
    //private float autoFillTime = 0.05f;
    //private float autoSpawnTime = 0.05f;
    private float initWait = 2.0f;
    private float autoFillTime = 30.0f;
    private float autoSpawnTime = 1.0f;

    private float timeAutoStarted;

    void Start () {
        sBoard = new SudokuBoard();
        StartCoroutine(AutoFillTiles());
    }

    void OnAwake(){
        Cursor.visible = false;
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
                yield return new WaitForSeconds(autoSpawnTime);
                timeAutoStarted = Time.time;
                Vector3 fillPos = new Vector3(autoFillSquare.xMinLim + (sBoard.xRes / 2), -0.6f, autoFillSquare.zMinLim + (sBoard.zRes / 2));
                GameObject autoFillTile = Instantiate(autoFillTilePrefab, fillPos, new Quaternion());
                yield return new WaitUntil(() => (Time.time - timeAutoStarted > autoFillTime || autoFillGotFilled));
                if (!autoFillGotFilled) autoFillTile.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = tileNumSprites[autoFillSquare.fillNum-1];
                autoFillGotFilled = false;
            }
            else
            {
                gameEnd = true;
            }
        }
    }

    static public void QuitGame(){
        for(int i = 0; i < 4; i++) GamePad.SetVibration((PlayerIndex)i,0,0);
    	Application.Quit();
    }
}
