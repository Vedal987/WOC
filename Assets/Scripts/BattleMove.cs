using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleMove 
{
	public string name;
	public int damage;
	public int heal;
	public string details;

	public BattleMove(string Name, int Damage, int Heal, string Details)
	{
		name = Name;
		damage = Damage;
		heal = Heal;
		details = Details;
	}
}
