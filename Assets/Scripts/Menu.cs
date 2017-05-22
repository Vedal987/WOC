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
		Flash.GetComponent<Animation> ().Play ("FadeOut");
		StartCoroutine (FlashFadeInWait ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator FlashFadeInWait()
	{
		yield return new WaitForSeconds (2f);
		Flash.SetActive (false);
	}

	public void New()
	{
		if (name.GetComponent<Text> ().text != "") {
			PlayerPrefs.DeleteAll ();
			PlayerPrefs.SetString ("Name", name.GetComponent<Text> ().text);
			Flash.SetActive (true);
			Flash.GetComponent<Animation> ().Play ();
			StartCoroutine (LoadScene ());
		}
	}

	public void Continue()
	{
		if (PlayerPrefs.GetFloat ("PlayerXPos", 1337.1337f) == 1337.1337f) {
			New ();
			return;
		}
		Flash.SetActive (true);
		Flash.GetComponent<Animation> ().Play ();
		StartCoroutine (LoadScene ());
	}

	IEnumerator LoadScene()
	{
		yield return new WaitForSeconds (1.1f);
		SceneManager.LoadScene ("Main");
	}
}
