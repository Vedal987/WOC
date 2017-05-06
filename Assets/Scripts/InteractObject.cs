using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour {

	public string[] Dialogue;
	public int d = 0;
	private GameObject player;
	public bool StartGame;
	public bool OnAwake;
	public bool IgnoreRaycast;
	public bool SecondDialogue;
	public int Option = 1;
	public string[] Option2;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		if (OnAwake) {
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
		if (Option == 1) {
			if (d == Dialogue.Length) {
				if (StartGame) {
					player.GetComponent<Main> ().Dialogue ("x7Start");
					return;
				}
				if (SecondDialogue) {
					Option = 2;
					d = 0;
				}
				player.GetComponent<Main> ().Dialogue ("x7Finish");
				ImportantStuff ();
				return;
			}
			string dia = Dialogue [d];
			d++;
			if (dia == "[FIGHT]") {
				player.GetComponent<Main> ().Dialogue (dia);
				this.gameObject.transform.parent.gameObject.SetActive (false);
				return;
			}
			if (dia == "[BYE]") {
				this.gameObject.SetActive (false);
				return;
			}
			if (dia == "[ANIM]") {
				this.GetComponent<Animator> ().SetTrigger ("Trigger");
				player.GetComponent<Main> ().Dialogue ("x7Finish");
				return;
			}
			player.GetComponent<Main> ().Dialogue (dia);
		} else {
			if (d == Option2.Length) {
				if (StartGame) {
					player.GetComponent<Main> ().Dialogue ("x7Start");
					return;
				}
				player.GetComponent<Main> ().Dialogue ("x7Finish");
				ImportantStuff ();
				return;
			}
			string dia = Option2 [d];
			d++;
			if (dia == "[BYE]") {
				this.gameObject.SetActive (false);
				return;
			}
			if (dia == "[ANIM]") {
				this.GetComponent<Animator> ().SetTrigger ("Trigger");
				return;
			}
			if (dia == "[FIGHT]") {
				player.GetComponent<Main> ().Dialogue (dia);
				this.gameObject.transform.parent.gameObject.SetActive (false);
				return;
			}
			player.GetComponent<Main> ().Dialogue (dia);
		}

	}

	public void ImportantStuff()
	{
		if (this.GetComponent<TitanHQNPC> ()) {
			this.GetComponent<TitanHQNPC> ().TalkingFinished ();
		}
	}
}
