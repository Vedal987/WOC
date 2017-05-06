using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour {

	public bool hasBeenTriggered = false;


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void OnTriggerEnter2D(Collider2D player)
	{
		if (!hasBeenTriggered) {
			hasBeenTriggered = true;
			this.gameObject.transform.parent.GetComponent<Animator> ().SetTrigger ("Trigger2");
		}
	}
}
