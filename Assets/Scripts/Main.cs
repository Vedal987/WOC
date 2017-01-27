using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {

	public bool inBattle;
	public GameObject creature;
	public GameObject BattleUI;
	public GameObject model;
	public Animator modelAnimator;
	float playerSpeed = 4f;

	void Start () {
		modelAnimator = model.GetComponent<Animator> ();
	}
		
	void Update()
	{
		float hor = Input.GetAxisRaw ("Horizontal");
		float ver = Input.GetAxisRaw ("Vertical");
		modelAnimator.SetFloat ("Horizontal", hor);
		modelAnimator.SetFloat ("Vertical", ver);
	}

	void FixedUpdate () {
		Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		GetComponent<Rigidbody2D>().velocity=targetVelocity * playerSpeed;
	}

	IEnumerator StartBattle()
	{
		inBattle = true;
		BattleUI.GetComponent<Animation> ().Play ("BattleUI_Enter");
		yield return new WaitForSeconds (1);
		BattleUI.GetComponent<RectTransform> ().localScale.Set (1, 1, 1);
	}


}



