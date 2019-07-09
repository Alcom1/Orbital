using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[XmlRoot("LevelCollection")]
public class LevelCollection
{
    [XmlArray("Levels")]
    [XmlArrayItem("Level")]
    public Level[] levels;
}

public class Level
{
    [XmlAttribute("isMenu")]
    public bool isMenu;

    [XmlArray("Objects")]
    [XmlArrayItem("Object")]
    public LevelObject[] objects;
}

public struct LevelObject
{
    [XmlAttribute("name")]
    public string name;

    [XmlAttribute("xPos")]
    public float xPos;

    [XmlAttribute("yPos")]
    public float yPos;

    [XmlAttribute("scale")]
    public float scale;
}
