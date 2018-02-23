using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour {

	public AudioClip musicLoop;
	AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		audioSource.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!audioSource.isPlaying) {
			GetComponent<AudioSource> ().loop = true;
			audioSource.clip = musicLoop;
			audioSource.Play ();
		}
	}
}
