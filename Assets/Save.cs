using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour {

	public string[] Dialogue;
	public int d = 0;
	private GameObject player;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	public void Interact()
	{
		if (d == Dialogue.Length) {
			player.GetComponent<Main> ().Dialogue ("x7Finish");
			return;
		}
		string dia = Dialogue [d];
		d++;
		player.GetComponent<Main> ().Dialogue (dia);
	}

}
