using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {

	public AudioClip[] clips;
	public AudioClip battle;

	public AudioSource audios;

	public int currentM = 0;

	// Use this for initialization
	void Start () {
		ChangeMusic ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeMusic()
	{
		audios.clip = clips [currentM];
		audios.Play ();
		currentM++;
	}

	public void BattleMusic()
	{
		audios.clip = battle;
		audios.Play ();
	}
}
