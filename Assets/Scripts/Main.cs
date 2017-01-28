using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {

	public bool canMove = true;
	public bool dialogue = false;
	public GameObject creature;
	public GameObject BattleUI;
	public GameObject model;
	public GameObject DialogueBox;
	public GameObject DialogueText;
	public Animator modelAnimator;
	float playerSpeed = 4f;

	public string LastKeyPress;
	public string direction;
	Vector2 dir;


	void Start () {
		modelAnimator = model.GetComponent<Animator> ();
		//StartCoroutine ("StartBattle");
	}
		
	void Update()
	{
		if (canMove) {
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
				RaycastHit2D hit = Physics2D.Raycast (transform.position, dir, 0.5f);
				if (hit.collider != null) {
					if (hit.collider.gameObject.GetComponent<InteractObject> ()) {
						hit.collider.gameObject.SendMessage ("Interact");
					}
				}
			}
		} else {
			if (dialogue) {
				if (Input.GetKeyDown(KeyCode.E)) {
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
					RaycastHit2D hit = Physics2D.Raycast (transform.position, dir, 0.5f);
					if (hit.collider != null) {
						if (hit.collider.gameObject.GetComponent<InteractObject> ()) {
							hit.collider.gameObject.SendMessage ("Interact");
						}
					}
				}
				
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
		} else {
			modelAnimator.SetBool ("IdleRight", false);
			modelAnimator.SetBool ("IdleDown", false);
			modelAnimator.SetBool ("IdleLeft", false);
			modelAnimator.SetBool ("IdleUp", false);
		}

	}

	public void Dialogue(string d)
	{
		if (d=="x7Finish") {
			DialogueBox.SetActive (false);
			canMove = true;
			dialogue = false;
			return;
		}
		DialogueBox.SetActive (true);
		canMove = false;
		dialogue = true;
		DialogueText.GetComponent<Text> ().text = d;
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



