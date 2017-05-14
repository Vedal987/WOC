using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefStart : MonoBehaviour {

	public string Pref;

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt (Pref, 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
