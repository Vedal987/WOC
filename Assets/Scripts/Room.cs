using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

	public AudioClip music;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerEnter2D(Collider2D other) {
		if (GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource> ().clip == music) {
			return;
		} else {
			GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource> ().clip = music;
			GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource> ().Play ();
		}
	}
}
