using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class Master : MonoBehaviour
{
    public BarRenderer BarBlue;
    public BarRenderer BarRed;

    private float blueProgress = 0;
    private float blueRate = 10;

    private LevelCollection levelCollection;
    private readonly string leveblueProgressath = "Assets/xml/Levels.xml";

    // Use this for initialization
    void Start ()
    {
        levelCollection = GetLevels(leveblueProgressath);
        BuildLevel(0);
    }

    private void BuildLevel(int index)
    {
        foreach(LevelObject levelObject in levelCollection.levels[index].objects)
        {
            var gameObject = Resources.Load<GameObject>(levelObject.name);
            var newLevelObject = Instantiate(gameObject, this.transform);

            newLevelObject.transform.position = new Vector3(levelObject.xPos, levelObject.yPos, newLevelObject.transform.position.z);
            newLevelObject.transform.localScale = new Vector3(levelObject.scale, levelObject.scale, levelObject.scale);
        }
    }

    //Read Levels
    private LevelCollection GetLevels(string path)
    {
        using (StreamReader reader = new StreamReader(leveblueProgressath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LevelCollection));
            var levelCollectionNew = serializer.Deserialize(reader) as LevelCollection;

            return levelCollectionNew;
        }
    }

    // Update is called once per frame
    void Update()
    {
        blueProgress += blueRate * Time.deltaTime;

        BarBlue.UpdateBar(blueProgress);

        BarRed.UpdateBar(blueProgress / 2);
    }
}
