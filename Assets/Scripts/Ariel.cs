using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ariel : MonoBehaviour {


	public string[] Dialogue;
	public int d = 0;
	private GameObject player;
	public bool switched = false;
	public bool StartGame;

	private bool needBottle;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		if (StartGame) {
			Interact ();

		}
	}

	public void Interact()
	{
			if (needBottle) {
				string needBottle = "Take a look around, what can you find?";
				d++;
				if (d == 2) {
					player.GetComponent<Main> ().Dialogue ("x7Finish");
					d = 0;
					return;
				}
				player.GetComponent<Main> ().Dialogue (needBottle);
				return;
			}
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
			if (dia == "You won't be able to get back any time soon.") {
				needBottle = true;
				d = 0;
			}
			if (dia == "Follow me.") {
				this.GetComponent<Animator> ().SetTrigger ("FollowMe");
				StartCoroutine ("FollowMeWait");
			}
			if (dia == "*Ariel mutturs random words*") {
			Vector3 newpos = new Vector3(-10.5f, -60.3f);
			player.transform.position = newpos;
			StartCoroutine ("TeleportWait");
			}
			player.GetComponent<Main> ().Dialogue (dia);

	}

	IEnumerator TeleportWait()
	{
		yield return new WaitForSeconds (5f);
		player.GetComponent<Main> ().canMove = true;
		player.GetComponent<Main> ().dialogue = false;
		player.GetComponent<Main> ().DialogueBox.SetActive (false);
	}

	public string[] dialogue2;
	public string[] dialogue3;

	
	// Update is called once per frame
	void Update () {
		if (player.GetComponent<Main> ().Bag.Contains ("Bottle Of Healing") && !switched) {
			switched = true;
			Dialogue = dialogue2;
			needBottle = false;
			d = 0;
		}
	}

	IEnumerator FollowMeWait()
	{
		yield return new WaitForSeconds (1);
		player.GetComponent<Main> ().canMove = true;
		player.GetComponent<Main> ().dialogue = false;
		player.GetComponent<Main> ().DialogueBox.SetActive (false);
		yield return new WaitForSeconds (0.3f);
		transform.position = new Vector3 (-9.56f, -8.53f, 0f);
		Dialogue = dialogue3;
		d = 0;
	}
}
