using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[XmlRoot("CreatureCollection")]
public class CreatureContainer{

	[XmlArray("Creatures")]
	[XmlArrayItem("Creature")]
	public List<CreatureTemplate> creatures = new List<CreatureTemplate> ();

	public static CreatureContainer Load(string path)
	{
		TextAsset _xml = Resources.Load<TextAsset> (path);

		XmlSerializer serializer = new XmlSerializer (typeof(CreatureContainer));

		StringReader reader = new StringReader (_xml.text);

		CreatureContainer creatures = serializer.Deserialize (reader) as CreatureContainer;

		reader.Close();

		return creatures;
	}
}
