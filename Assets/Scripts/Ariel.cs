using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ariel : MonoBehaviour {

	private GameObject player;

	public InteractObject IO;

	public string[] dialogue2;


	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (player.GetComponent<Main> ().Bag.Contains ("Bottle Of Healing")) {
			IO.Dialogue = dialogue2;
		}
	}
}
