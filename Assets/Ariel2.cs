using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ariel2 : MonoBehaviour {

	private GameObject player;

	public GameObject demonAttackCamera;
	public GameObject camera;
	private Animator daAnimator;
	public GameObject demon;

	
	public string[] Dialogue;
	private int d = 0;
	public bool StartGame;
	public bool IgnoreRaycast;
	public string[] dialogue2;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		daAnimator = demonAttackCamera.GetComponent<Animator> ();
		if (StartGame) {
			Interact ();
		}
	}

	void Update()
	{
		if (player.GetComponent<Main> ().canSkip && IgnoreRaycast) {
			if (Input.GetKeyDown (KeyCode.E)) {
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
		if (dia == "Go talk to some people, make yourself at home.") {
			StartCoroutine ("DemonAttackWait");
		}
		if (dia == "*You recieved Ariel*") {
			this.GetComponent<SpriteRenderer> ().enabled = false;
		}
		d++;

		player.GetComponent<Main> ().Dialogue (dia);

	}

	IEnumerator DemonAttackWait()
	{
		yield return new WaitForSeconds (80);
		camera.SetActive (false);
		demonAttackCamera.SetActive (true);
		daAnimator.SetTrigger ("1");
		demon.SetActive (true);
	}

	public void StartDemonHelp()
	{
		d = 0;
		Dialogue = dialogue2;
		IgnoreRaycast = true;
		Interact ();
	}

}
