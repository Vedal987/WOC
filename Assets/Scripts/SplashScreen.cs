using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashScreen : MonoBehaviour {

	public GameObject vp;

	void Awake()
	{
		StartCoroutine ("LoadScene");
		vp.GetComponent<VideoPlayer> ().Play ();
		vp.GetComponent<AudioSource> ().Play ();
	}

	IEnumerator LoadScene()
	{
		yield return new WaitForSeconds (12f);
		SceneManager.LoadScene ("Menu");
	}
}
