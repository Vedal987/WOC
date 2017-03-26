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

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeMusic()
	{
		StartCoroutine (FadeClip (clips [currentM]));
	}

	IEnumerator FadeClip(AudioClip clip)
	{
		audios.clip = clip;
		audios.volume = 0.3f;
		yield return new WaitForSeconds(0.1f);
		audios.volume = 0.2f;
		yield return new WaitForSeconds(0.1f);
		audios.volume = 0.1f;
		yield return new WaitForSeconds(0.1f);
		audios.Play ();
		currentM++;
		audios.volume = 0.2f;
		yield return new WaitForSeconds(0.1f);
		audios.volume = 0.4f;
		yield return new WaitForSeconds(0.1f);
	}

	public void BattleMusic()
	{
		audios.clip = battle;
		audios.Play ();
	}
}
