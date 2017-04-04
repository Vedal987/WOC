using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

	public GameObject nameInput;
	public GameObject placeholder;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void NewGame()
	{
		PlayerPrefs.SetString ("Name", nameInput.GetComponent<Text> ().text);
		SceneManager.LoadScene ("Main");
	}

	public void ContinueGame()
	{

	}
}
