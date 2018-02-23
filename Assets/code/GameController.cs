using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



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
    private int[] na = new int[9]; //active tiles
    private bool gameEnd = false;
    private bool winnerCalc = false;
    //private float initWait = 0.05f;
    //private float autoFillTime = 0.05f;
    //private float autoSpawnTime = 0.05f;
    private float initWait = 2.0f;
    private float autoFillTime = 15.0f;
    private float autoSpawnTime = 1.0f;

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
        if (gameEnd && !winnerCalc && false)
        {
            List<int> winners = sBoard.GetWinnerList();
            for (int i = 0; i < winners.Count; i++)
            {
                playerWinImg[i].enabled = true;
                playerWinImg[i].sprite = players[winners[i]-1].GetComponent<PMove>().winSprite;
            }
            winnerCalc = true;
        }
    }

    IEnumerator AutoFillTiles()
    {
        yield return new WaitForSeconds(initWait);
        while (!gameEnd)
        {   
            tilesFilled++;
            if(tilesFilled % 4 == 0 && tilesFilled < 15) {
                StartCoroutine(AutoFillTiles());
                autoFillTime -= 3;
            }
            Square autoFillSquare = sBoard.FillRandSquare();
            if (autoFillSquare != null)
            {

                na[autoFillSquare.solNum - 1]++;
                yield return new WaitForSeconds(autoSpawnTime + Random.value);
                timeAutoStarted = Time.time;
                Vector3 fillPos = new Vector3(autoFillSquare.xMinLim + (sBoard.xRes / 2), -0.6f, autoFillSquare.zMinLim + (sBoard.zRes / 2));
                GameObject autoFillTile = Instantiate(autoFillTilePrefab, fillPos, new Quaternion());
                foreach(Transform t in tileStack.transform) {
                    if(t.gameObject.GetComponent<ObjT>().id == autoFillSquare.solNum) 
                    {t.gameObject.SetActive(true);
                    numbersOut[autoFillSquare.solNum]++;}
                }
                yield return new WaitUntil(() => (Time.time - timeAutoStarted > (autoFillTime + Random.value)) || na[autoFillSquare.solNum - 1] == 0);
                if (na[autoFillSquare.solNum - 1] != 0) 
                {
                    na[autoFillSquare.solNum - 1]--;
                    if(autoFillTile != null){
                        autoFillTile.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = tileNumSprites[autoFillSquare.fillNum-1];
                        autoFillTile.GetComponentsInChildren<flashScript>()[0].enabled = false;
                        foreach(Transform t in autoFillTile.transform) t.gameObject.GetComponent<SpriteRenderer>().color = new Color(0,0,0,1);
                    }
                }
                foreach(Transform t in tileStack.transform) {
                    
                    if((t.gameObject.GetComponent<ObjT>().id == autoFillSquare.solNum) && tilesFilled < 15)
                        {
                            if (na[autoFillSquare.solNum - 1] == 0) t.gameObject.SetActive(false);
                        }
                }                
        
                
                autoFillGotFilled = false;
            }
            else 
            {
                bool r = false;
                foreach(int n in na){
                    if(n != 0) r = true;
                }
                if (!r) gameEnd = true;
            }
        }
    }

    public void fill(int num){
        na[num - 1]--;
    }

    static public void QuitGame(){
        //for(int i = 0; i < 4; i++) GamePad.SetVibration((PlayerIndex)i,0,0);
    	Application.Quit();
    }

}
