using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public SudokuBoard sBoard;
	public List<GameObject> players;

	void Start () {
        sBoard = new SudokuBoard();
	}
	
	void Update ()
    {
        //foreach (GameObject player in players)
        //{
        //    Debug.Log("P" + players.IndexOf(player) + ": " + sBoard.objTransformToSquare(player.transform).number);
        //}
    }
}
