using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class CreatureTemplate {
	
	[XmlAttribute("Name")]
	public string Name;

	[XmlElement("name1")]
	public string name1;
	[XmlElement("damage1")]
	public int damage1;
	[XmlElement("heal1")]
	public int heal1;
	[XmlElement("details1")]
	public string details1;
	[XmlElement("tooltip1")]
	public string tooltip1;

	[XmlElement("name2")]
	public string name2;
	[XmlElement("damage2")]
	public int damage2;
	[XmlElement("heal2")]
	public int heal2;
	[XmlElement("details2")]
	public string details2;
	[XmlElement("tooltip2")]
	public string tooltip2;

	[XmlElement("name3")]
	public string name3;
	[XmlElement("damage3")]
	public int damage3;
	[XmlElement("heal3")]
	public int heal3;
	[XmlElement("details3")]
	public string details3;
	[XmlElement("tooltip3")]
	public string tooltip3;

	[XmlElement("name4")]
	public string name4;
	[XmlElement("damage4")]
	public int damage4;
	[XmlElement("heal4")]
	public int heal4;
	[XmlElement("details4")]
	public string details4;
	[XmlElement("tooltip4")]
	public string tooltip4;

	[XmlElement("health")]
	public int health;
	[XmlElement("MaxHealth")]
	public int MaxHealth;

	[XmlElement("Level")]
	public int Level;
}
