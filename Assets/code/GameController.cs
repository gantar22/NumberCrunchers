using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//using XInputDotNetPure;


public class GameController : MonoBehaviour {

    [SerializeField] Image[] playerWinImg;
    [SerializeField] Sprite[] tileNumSprites;
    [SerializeField] GameObject tileStack;

    public List<GameObject> players;
    public GameObject autoFillTilePrefab;
    public SudokuBoard sBoard;
    [SerializeField]
    public bool autoFillGotFilled;



    private Dictionary<int,int> numbersOut;
    private bool gameEnd = false;
    private float autoSpawnTime = 1.0f;
    //private float initWait = 1.0f;
    //private float autoFillTime = 0.05f;
    private float initWait = 2.0f;
    private float autoFillTime = 30.0f;
    private int tilesFilled;

    private float timeAutoStarted;

    void Start () {
        sBoard = new SudokuBoard();
        StartCoroutine(AutoFillTiles());
        numbersOut = new Dictionary<int,int>();
        for(int i = 1; i < 10; i++){
            numbersOut[i] = 0;
        }
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
            tilesFilled++;
            if(tilesFilled % 4 == 0) StartCoroutine(AutoFillTiles());
            Square autoFillSquare = sBoard.FillRandSquare();
            if (autoFillSquare != null)
            {
                yield return new WaitForSeconds(autoSpawnTime);
                timeAutoStarted = Time.time;
                Vector3 fillPos = new Vector3(autoFillSquare.xMinLim + (sBoard.xRes / 2), -0.6f, autoFillSquare.zMinLim + (sBoard.zRes / 2));
                GameObject autoFillTile = Instantiate(autoFillTilePrefab, fillPos, new Quaternion());
                foreach(Transform t in tileStack.transform) {
                    if(t.gameObject.GetComponent<ObjT>().id == autoFillSquare.solNum) 
                    {t.gameObject.SetActive(true);
                    numbersOut[autoFillSquare.solNum]++;}
                }
                yield return new WaitUntil(() => (Time.time - timeAutoStarted > autoFillTime || autoFillGotFilled));
                foreach(Transform t in tileStack.transform) {
                    
                    if(t.gameObject.GetComponent<ObjT>().id == autoFillSquare.solNum) 
                        {print(numbersOut[autoFillSquare.solNum]); if(--numbersOut[autoFillSquare.solNum] == 0) t.gameObject.SetActive(false);}
                }                
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
        //for(int i = 0; i < 4; i++) GamePad.SetVibration((PlayerIndex)i,0,0);
    	Application.Quit();
    }
}
