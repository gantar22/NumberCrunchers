using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class endvideoscript : MonoBehaviour {
	public GameObject gc;
	public GameObject skipBut;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!GetComponent<VideoPlayer>().isPlaying)
		{
			gameObject.SetActive(false);
			skipBut.SetActive(false);
			gc.SetActive(true);

		}
	}
}
