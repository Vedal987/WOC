using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

	void Start()
	{
		((MovieTexture)GetComponent<Renderer> ().material.mainTexture).Play ();
		StartCoroutine ("LoadScene");
	}

	IEnumerator LoadScene()
	{
		yield return new WaitForSeconds (10f);
		SceneManager.LoadScene ("Menu");
	}
}
