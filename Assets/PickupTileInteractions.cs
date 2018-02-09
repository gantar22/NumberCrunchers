using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTileInteractions : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    private void OnCollisionStay(GameObject colObj)
    {
        Debug.Log("Test");
        if (colObj.CompareTag("Player"))
        {
            Debug.Log("Hello");
        }
    }

    private void OnCollisionEnter(GameObject colObj)
    {
        Debug.Log("Test");
        if (colObj.CompareTag("Player"))
        {
            Debug.Log("Hello");
        }
    }
}
