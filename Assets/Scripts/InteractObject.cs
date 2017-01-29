using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour {

	public string[] Dialogue;
	private int d = 0;
	private GameObject player;
	public bool StartGame;
	public bool OnAwake;
	public bool IgnoreRaycast;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		if (OnAwake) {
			Interact ();

		}
	}

	void Update()
	{
		if(player.GetComponent<Main>().canSkip && IgnoreRaycast)
		{
			if(Input.GetKeyDown(KeyCode.E))
			{
				Interact ();	
			}
		}
	}

	public void Interact()
	{
			if (d == Dialogue.Length) {
				if (StartGame) {
					player.GetComponent<Main> ().Dialogue ("x7Start");
					return;
				}
				player.GetComponent<Main> ().Dialogue ("x7Finish");
				return;
			}
			string dia = Dialogue [d];
			d++;
			player.GetComponent<Main> ().Dialogue (dia);

	}
}
