using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {

	public bool inBattle = false;
	public GameObject creature;
	public GameObject BattleUI;
	public GameObject model;
	public GameObject DialogueBox;
	public GameObject DialogueText;
	public Animator modelAnimator;
	float playerSpeed = 4f;

	public string LastKeyPress;


	void Start () {
		modelAnimator = model.GetComponent<Animator> ();
		//StartCoroutine ("StartBattle");
	}
		
	void Update()
	{
		if (!inBattle) {
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
	}

	void FixedUpdate () {
		if (!inBattle) {
			Vector2 targetVelocity = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
			GetComponent<Rigidbody2D> ().velocity = targetVelocity * playerSpeed;
		}
	}

	IEnumerator StartBattle()
	{
		inBattle = true;
		BattleUI.GetComponent<Animation> ().Play ("BattleUI_Enter");
		yield return new WaitForSeconds (1);
		BattleUI.GetComponent<RectTransform> ().localScale.Set (1, 1, 1);
	}


}



