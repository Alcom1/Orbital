using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class Master : MonoBehaviour
{
    public LevelCollection levelCollection;

    private string levelPath = "Assets/xml/Levels.xml";

    // Use this for initialization
    void Start ()
    {
        levelCollection = GetLevels(levelPath);

        BuildLevel(0);
    }

    private void BuildLevel(int index)
    {
        foreach(LevelObject levelObject in levelCollection.levels[index].objects)
        {
            var gameObject = Resources.Load<GameObject>(levelObject.name);

            gameObject.transform.position = new Vector2(levelObject.xPos, levelObject.yPos);
            gameObject.transform.localScale = new Vector3(levelObject.scale, levelObject.scale, levelObject.scale);

            gameObject.transform.parent = this.transform;

            Instantiate(gameObject);
        }
    }

    //Read Levels
    private LevelCollection GetLevels(string path)
    {
        using (StreamReader reader = new StreamReader(levelPath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LevelCollection));
            var levelCollectionNew = serializer.Deserialize(reader) as LevelCollection;

            return levelCollectionNew;
        }
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
