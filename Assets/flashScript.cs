using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashScript : MonoBehaviour {
	[SerializeField]
	Color c1;
	[SerializeField]
	Color c2;
	[SerializeField]
	float period;


	
	// Update is called once per frame
	void Update () {
		GetComponent<SpriteRenderer>().color = (Time.time % period < (period / 2) ? c1 : c2);
	}
}
