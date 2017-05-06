using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureLoader : MonoBehaviour {

	public const string path = "Creature";

	// Use this for initialization
	void Start () 
	{
		CreatureContainer cc = CreatureContainer.Load (path);
		foreach (CreatureTemplate creature in cc.creatures) {
			print (creature.Name);
		}
	}
	

}
