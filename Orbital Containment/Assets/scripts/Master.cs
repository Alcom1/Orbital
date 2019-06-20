using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class Master : MonoBehaviour
{
    public BarRenderer BarBlue;
    public BarRenderer BarRed;
    public List<SpriteRenderer> Borders;

    private float blueProgress = 0;
    private float blueRate = 10;

    private float redProgress = 0;
    private float redRate = 0.1f;

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

        if(blueProgress < 100)
        {
            foreach (var rockObject in GameObject.FindGameObjectsWithTag("Rock"))
            {
                var rock = rockObject.GetComponent<Rock>();

                if (rock.IsColliding)
                {
                    redProgress += redRate * Time.deltaTime;
                }
            }
        }

        BarBlue.UpdateBar(blueProgress);
        BarRed.UpdateBar(redProgress);

        foreach(var border in Borders)
        {
            float channel = 1 - 0.85f * Mathf.Min(redProgress, 1);
            border.color = new Color(channel, channel, channel);
        }
    }
}
