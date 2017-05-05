using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public GameObject name;
	public GameObject Flash;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Go()
	{
		if (name.GetComponent<Text> ().text != "") {
			PlayerPrefs.SetString ("Name", name.GetComponent<Text> ().text);
			Flash.SetActive (true);
			Flash.GetComponent<Animation> ().Play ();
			StartCoroutine (LoadScene ());
		}
	}

	IEnumerator LoadScene()
	{
		yield return new WaitForSeconds (1.1f);
		SceneManager.LoadScene ("Main");
	}
}
