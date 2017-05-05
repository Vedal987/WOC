using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSpawner : MonoBehaviour {

	public GameObject Fireball;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("Spawn", 0f, 3f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Spawn()
	{
		if (this.transform.parent.gameObject.activeSelf == true) {
			GameObject fb = GameObject.Instantiate (Fireball, this.transform.position, Quaternion.identity);
			int z = Random.Range (0, 150);
			fb.transform.Rotate (0, 0, z);
		}
	}
}
