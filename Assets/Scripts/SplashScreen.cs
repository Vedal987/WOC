using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashScreen : MonoBehaviour {

	public GameObject vp;
	public MovieTexture mov;

	void Awake()
	{
		StartCoroutine ("LoadScene");
		vp.GetComponent<AudioSource> ().Play ();
		this.GetComponent<Renderer> ().material.mainTexture = mov;
		mov.Play ();

	}
	IEnumerator LoadScene()
	{
		yield return new WaitForSeconds (12f);
		SceneManager.LoadScene ("Menu");
	}
}
