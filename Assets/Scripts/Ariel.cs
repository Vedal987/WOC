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
				player.GetComponent<Main> ().Dialogue ("x7Finish", this.gameObject);
					d = 0;
					return;
				}
			player.GetComponent<Main> ().Dialogue (needBottle, this.gameObject);
				return;
			}
			if (d == Dialogue.Length) {
				if (StartGame) {
				player.GetComponent<Main> ().Dialogue ("x7Start", this.gameObject);
					return;
				}
			player.GetComponent<Main> ().Dialogue ("x7Finish", this.gameObject);
				return;
			}
			string dia = Dialogue [d];
			d++;
			if (dia == "You won't be able to get back any time soon.") {
				needBottle = true;
				d = 0;
			}
		if(dia == "Ok then, well make sure you take a look around and make yourself comfortable.")
			{
				GameObject.FindGameObjectWithTag ("Music").GetComponent<Music> ().ChangeMusic ();
			}
			if (dia == "Yes of course. Sorry.") {
				GameObject.FindGameObjectWithTag ("Music").GetComponent<Music> ().ChangeMusic ();
			}
			if (dia == "Follow me.") {
				this.GetComponent<Animator> ().SetTrigger ("FollowMe");
				GameObject.FindGameObjectWithTag ("Music").GetComponent<Music> ().ChangeMusic ();
				StartCoroutine ("FollowMeWait");
			}
			if (dia == "*Ariel mutters random words*") {
			GameObject.FindGameObjectWithTag ("Music").GetComponent<Music> ().ChangeMusic ();
			Vector3 newpos = new Vector3(-10.5f, -60.3f);
			player.transform.position = newpos;
			StartCoroutine ("TeleportWait");
			}
		player.GetComponent<Main> ().Dialogue (dia, this.gameObject);

	}

	IEnumerator TeleportWait()
	{
		yield return new WaitForSeconds (5f);
		player.GetComponent<Main> ().canMove = true;
		GameObject.FindGameObjectWithTag ("Music").GetComponent<Music> ().ChangeMusic ();
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
