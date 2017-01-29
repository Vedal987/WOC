using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {

	public bool canMove = true;
	public bool dialogue = false;
	public bool start = true;
	public GameObject creature;
	public GameObject BattleUI;
	public GameObject BagUI;
	public GameObject BagText;
	public GameObject model;
	public GameObject DialogueBox;
	public GameObject DialogueText;
	public GameObject Flash;
	public Animator modelAnimator;
	float playerSpeed = 4f;

	public GameObject demonAttackCamera;
	public GameObject camera;
	private Animator daAnimator;

	public List<string> Bag;
	public bool isBag;

	public string LastKeyPress;
	public string direction;
	Vector2 dir;

	private string str;

	public bool canSkip = true;

	void Start () {
		modelAnimator = model.GetComponent<Animator> ();
		//StartCoroutine ("StartBattle");
		Flash.SetActive(true);
	}
		
	void Update()
	{
		if (start) {
			if (Input.GetKeyDown(KeyCode.E)) {
				Flash.SendMessage ("Interact");
			}
			return;
		}
		if (!dialogue) {
			if (Input.GetKeyDown(KeyCode.Q)) {
				isBag = !isBag;
			}
			if (isBag) {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
				modelAnimator.enabled = false;
				canMove = false;
				BagUI.SetActive (true);
				string Items = "";
				foreach (string item in Bag) {
					Items = Items + item + "\n";
				}
				BagText.GetComponent<Text> ().text = Items;
			} else {
				modelAnimator.enabled = true;
				canMove = true;
				BagUI.SetActive (false);
			}
		}
		if (canMove) {
			if (GetComponent<Rigidbody2D> ().velocity.x == 0 && GetComponent<Rigidbody2D> ().velocity.y == 0) {
				modelAnimator.enabled = false;
			} else {
				modelAnimator.enabled = true;
			}
			float hor = Input.GetAxisRaw ("Horizontal");
			float ver = Input.GetAxisRaw ("Vertical");
			modelAnimator.SetFloat ("Horizontal", hor);
			modelAnimator.SetFloat ("Vertical", ver);
			if (Input.GetKeyUp (KeyCode.W)) {
				LastKeyPress = "W";
			}
			if (Input.GetKeyUp (KeyCode.A)) {
				LastKeyPress = "A";
			}
			if (Input.GetKeyUp (KeyCode.S)) {
				LastKeyPress = "S";
			}
			if (Input.GetKeyUp (KeyCode.D)) {
				LastKeyPress = "D";
			}

			if (Input.GetKeyDown (KeyCode.W)) {
				direction = "W";
			} else if (Input.GetKeyDown (KeyCode.A)) {
				direction = "A";
			} else if (Input.GetKeyDown (KeyCode.S)) {
				direction = "S";
			} else if (Input.GetKeyDown (KeyCode.D)) {
				direction = "D";
			}
//			if (!Input.GetKey (KeyCode.W)) {
//				if (!Input.GetKey (KeyCode.A)) {
//					if (!Input.GetKey (KeyCode.S)) {
//						if (!Input.GetKey (KeyCode.D)) {
//							modelAnimator.SetTrigger ("Reset");
//						}
//					}
//				}
//			}
			if (Input.GetKeyDown (KeyCode.E)) {
				if (direction == "W") {
					dir = Vector2.up;
				}
				if (direction == "A") {
					dir = Vector2.left;
				}
				if (direction == "S") {
					dir = Vector2.down;
				}
				if (direction == "D") {
					dir = Vector2.right;
				}
				RaycastHit2D hit = Physics2D.Raycast (transform.position, dir, 1.8f);
				if (hit.collider != null) {
					if (hit.collider.gameObject.GetComponent<InteractObject> () || hit.collider.gameObject.GetComponent<Ariel> ()) {
						hit.collider.gameObject.SendMessage ("Interact");
					}
				}

			}
		} else {
			if (dialogue) {
				if (canSkip) {
					GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
					modelAnimator.enabled = false;
					if (Input.GetKeyDown (KeyCode.E)) {
						if (direction == "W") {
							dir = Vector2.up;
						}
						if (direction == "A") {
							dir = Vector2.left;
						}
						if (direction == "S") {
							dir = -Vector2.up;
						}
						if (direction == "D") {
							dir = -Vector2.left;
						}
						RaycastHit2D hit = Physics2D.Raycast (transform.position, dir, 1.8f);
						if (hit.collider != null) {
							if (hit.collider.gameObject.GetComponent<InteractObject> () || hit.collider.gameObject.GetComponent<Ariel> ()) {
								hit.collider.gameObject.SendMessage ("Interact");
							}
						}
					}
				}
				
			} else {
				modelAnimator.enabled = true;
			}
		}
		if (!Input.anyKey) {
			if (LastKeyPress == "W") {
				modelAnimator.SetBool ("IdleUp", true);
				modelAnimator.SetBool ("IdleRight", false);
				modelAnimator.SetBool ("IdleDown", false);
				modelAnimator.SetBool ("IdleLeft", false);
			}
			if (LastKeyPress == "A") {
				modelAnimator.SetBool ("IdleLeft", true);
				modelAnimator.SetBool ("IdleRight", false);
				modelAnimator.SetBool ("IdleDown", false);
				modelAnimator.SetBool ("IdleUp", false);
			}
			if (LastKeyPress == "S") {
				modelAnimator.SetBool ("IdleDown", true);
				modelAnimator.SetBool ("IdleRight", false);
				modelAnimator.SetBool ("IdleLeft", false);
				modelAnimator.SetBool ("IdleUp", false);
			}
			if (LastKeyPress == "D") {
				modelAnimator.SetBool ("IdleRight", true);
				modelAnimator.SetBool ("IdleDown", false);
				modelAnimator.SetBool ("IdleLeft", false);
				modelAnimator.SetBool ("IdleUp", false);
			}
		} 

	}

	public void Dialogue(string d)
	{
		if (canSkip) {
			canSkip = false;
			if (d.Contains ("x7Item")) {
				d = "You found a" + d.Replace ("x7Item", "");
				string Item = d.Remove (0, 12);
				if (Bag.Count == 8) {
					d = d + "\nBut your bag is full.";
				} else {
					Bag.Add (Item);
				}

			}
			if (d == "x7Finish") {
				DialogueBox.SetActive (false);
				canMove = true;
				dialogue = false;
				canSkip = true;
				return;
			}
			if (d == "x7Start") {
				DialogueBox.SetActive (false);
				Flash.GetComponent<Animation> ().Play ();
				canMove = true;
				dialogue = false;
				start = false;
				canSkip = true;
				return;
			}
			if (d == "ARAGGHHH") {
				daAnimator.SetTrigger ("2");

			}
			if (d.Contains ("[NAME]")) {
				string name = PlayerPrefs.GetString ("Name");
				d = d.Replace ("[NAME]", name);
			}
			DialogueBox.SetActive (true);
			canMove = false;
			dialogue = true;
			if (d == "*Ariel mutturs random words*") {
				
			}
			StartCoroutine (AnimateText (d));
		}
	}

	IEnumerator FlashCamera()
	{
		Flash.GetComponent<SpriteRenderer> ().color = Color.white;
		yield return new WaitForSeconds (1f);
		Flash.GetComponent<Animation> ().Play ();
	}

	IEnumerator AnimateText(string strComplete){
		int i = 0;
		str = "";
		while( i < strComplete.Length ){
			str += strComplete[i++];
			DialogueText.GetComponent<Text> ().text = str;
			yield return new WaitForSeconds(0.05F);
		}
		canSkip = true;
	}

	void FixedUpdate () {
		if (canMove) {
			Vector2 targetVelocity = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
			GetComponent<Rigidbody2D> ().velocity = targetVelocity * playerSpeed;
		}
	}

	IEnumerator StartBattle()
	{
		canMove = false;
		BattleUI.GetComponent<Animation> ().Play ("BattleUI_Enter");
		yield return new WaitForSeconds (1);
		BattleUI.GetComponent<RectTransform> ().localScale.Set (1, 1, 1);
	}


}



