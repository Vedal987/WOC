using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Creature : MonoBehaviour {

	public GameObject Health;

	[Space(10)]

	public string name1;
	public int damage1;
	public int heal1;
	public string details1;
	public string tooltip1;

	[Space(10)]

	public string name2;
	public int damage2;
	public int heal2;
	public string details2;
	public string tooltip2;

	[Space(10)]

	public string name3;
	public int damage3;
	public int heal3;
	public string details3;
	public string tooltip3;

	[Space(10)]

	public string name4;
	public int damage4;
	public int heal4;
	public string details4;
	public string tooltip4;

	[Space(10)]

	public int health;
	public int MaxHealth;

	public bool AI;

	public string CreatureName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Health.GetComponent<Text> ().text = health.ToString() + "/" + MaxHealth.ToString();
	}
		
}
