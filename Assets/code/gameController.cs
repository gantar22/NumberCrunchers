using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour {

	
	public List<GameObject> players;

    public sudokuBoard sBoard;

	void Start () {
        sBoard = new sudokuBoard();
	}
	
	void Update () {
		
	}

}
