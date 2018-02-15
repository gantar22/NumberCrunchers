using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cartersshittyshadow : MonoBehaviour {
	public GameObject p;
	public bool point_four;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		GetComponent<SpriteRenderer>().sprite = p.GetComponent<SpriteRenderer>().sprite;
		GetComponent<SpriteRenderer>().color  = new Color(0,0,0,.07f);
		//transform.localEulerAngles = p.transform.localEulerAngles;
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,transform.localEulerAngles.y,
			Mathf.LerpAngle(-2,-40,(transform.position.x + 4.5f ) / 9));
		transform.position = new Vector3(p.transform.position.x + Mathf.Lerp(-.5f,1.2f,(transform.position.x + 4.5f ) / 9),transform.position.y,transform.position.z);

		//insert scaling!
		
	}
}
