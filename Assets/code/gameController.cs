using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour {

    public sudokuBoard sBoard;


	public List<GameObject> players;

	void Start () {
        sBoard = new sudokuBoard();
	}
	
	void Update ()
    {
        Debug.Log("x:" + players[3].transform.position.x + "z:" + players[3].transform.position.z);
        //foreach (GameObject player in players)
        //{
        //    Debug.Log("P" + players.IndexOf(player) + ": " + sBoard.playerPosToSquare(player.transform).number);
        //}
    }
}
