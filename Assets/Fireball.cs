using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

	public int ExplodeTime;
	public GameObject explosion;

	// Use this for initialization
	void Awake () {
		ExplodeTime = Random.Range (1, 3);
		StartCoroutine (Bomb (ExplodeTime));
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Translate (Vector3.left * 0.2f);
	}

	IEnumerator Bomb(int time)
	{
		yield return new WaitForSeconds (time);
		Instantiate (explosion, this.transform.position, Quaternion.identity);
		GameObject.Destroy (this.gameObject);
	}
}
