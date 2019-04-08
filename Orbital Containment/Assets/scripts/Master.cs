using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class Master : MonoBehaviour
{
    public LevelCollection levelCollection;

	// Use this for initialization
	void Start ()
    {
        var path = "Assets/xml/Levels.xml";

        XmlSerializer serializer = new XmlSerializer(typeof(LevelCollection));

        StreamReader reader = new StreamReader(path);

        var levels = serializer.Deserialize(reader) as LevelCollection;

        reader.Close();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
