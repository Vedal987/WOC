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

	public bool demonWaiting;

	public ManagerTitanHQ Manager;

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
		if (demonWaiting) {
			if (Manager.cousin && Manager.chest && Manager.weirdo && Manager.guard) {
				StartCoroutine ("DemonAttackWait");
				demonWaiting = false;
			}
		}
	}

	public void Interact()
	{
		if (d == Dialogue.Length) {
			if (StartGame) {
				player.GetComponent<Main> ().Dialogue ("x7Start", this.gameObject);
				return;
			}
			player.GetComponent<Main> ().Dialogue ("x7Finish", this.gameObject);
			return;
		}

		string dia = Dialogue [d];
		if (dia == "Go talk to some people, make yourself at home.") {
			demonWaiting = true;
		}
		if (dia == "*You received Ariel*") {
			this.GetComponent<SpriteRenderer> ().enabled = false;
		}
		d++;

		player.GetComponent<Main> ().Dialogue (dia, this.gameObject);

	}

	IEnumerator DemonAttackWait()
	{
		yield return new WaitForSeconds (3f);
		GameObject.FindGameObjectWithTag ("Music").GetComponent<Music> ().ChangeMusic ();
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
