using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefStart : MonoBehaviour {

	public bool SeaDemon;
	public bool SeaDemonDone;

	// Use this for initialization
	void Start () {
		if(SeaDemon)
		{
			GameObject.FindGameObjectWithTag ("Player").GetComponent<Main> ().SeaDemonPoint = 1;
		}
		if(SeaDemonDone)
		{
			GameObject.FindGameObjectWithTag ("Player").GetComponent<Main> ().SeaDemonDonePoint = 1;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
