using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
		Application.LoadLevel ("Main");
	}

	public void ContinueGame()
	{

	}
}
