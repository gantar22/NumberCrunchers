using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public int holderID;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (this.gameObject.tag == "scissors") {
			holderID = other.gameObject.GetComponent<ObjT> ().id;
		} else if (this.gameObject.tag == "weapon") {
			if (other.gameObject.GetComponent<ObjT> ().id != holderID) {
				Destroy (other.gameObject);
				Destroy (this.gameObject);
			}
		}
	}
}
