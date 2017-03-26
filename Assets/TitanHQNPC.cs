using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanHQNPC : MonoBehaviour {

	public ManagerTitanHQ Manager;

	public string Name;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TalkingFinished()
	{
		if (Name == "guard") {
			Manager.guard = true;
		}
		if (Name == "weirdo") {
			Manager.weirdo = true;
			GameObject.Find ("NPC2Cousin").GetComponent<InteractObject> ().Option = 1;
		}
		if (Name == "cousin") {
			Manager.cousin = true;
		}
		if (Name == "chest") {
			Manager.chest = true;
		}
	}
}
