﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour {

    public sudokuBoard sBoard;

    [SerializeField]
    List<GameObject> players;

	void Start () {
        sBoard = new sudokuBoard();
	}
	
	void Update ()
    {
        foreach (GameObject player in players)
        {
            Debug.Log("P"+players.IndexOf(player)+": "+sBoard.playerPosToSquare(player.transform).number);
        }
    }
}
