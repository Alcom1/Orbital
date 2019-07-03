using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public enum GameState
{
    Play,
    Win,
    Lose
}

public class Master : MonoBehaviour
{
    public BarRenderer BarBlue;
    public BarRenderer BarRed;
    public GameObject RockParent;
    public List<SpriteRenderer> Borders;

    private GameState gamestate = GameState.Play;
    private int currentLevel = 0;

    private float blueProgress = 0;
    private float blueRate = 10;

    private float redProgress = 0;
    private float redRate = 0.25f;

    private LevelCollection levelCollection;
    private readonly string leveblueProgressath = "Assets/xml/Levels.xml";

    // Use this for initialization
    void Start ()
    {
        levelCollection = GetLevels(leveblueProgressath);
        BuildLevel(currentLevel);
    }

    //Construct the level of a specific index
    private void BuildLevel(int index)
    {
        currentLevel = index;
        foreach (LevelObject levelObject in levelCollection.levels[index].objects)
        {
            var gameObject = Resources.Load<GameObject>(levelObject.name);
            var newLevelObject = Instantiate(gameObject, RockParent.transform);

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
        switch(gamestate)
        {
            case GameState.Play:
                if(!UpdateRed())
                {
                    UpdateBlue();   //Update blue only if red is not progressing.
                }

                //Check red status
                if (redProgress >= BarRed.valueMax)
                {
                    gamestate = GameState.Lose; //Lose
                }

                //Check Blue status
                if (blueProgress >= BarBlue.valueMax)
                {
                    gamestate = GameState.Win;  //Win
                }
            break;
            case GameState.Win:
                ClearLevel();
                BuildLevel((++currentLevel) % levelCollection.levels.Length);
                gamestate = GameState.Play;
            break;
            case GameState.Lose:
                ClearLevel();
                BuildLevel(currentLevel);
                gamestate = GameState.Play;
            break;
        }
    }

    //Clear the level
    private void ClearLevel()
    {
        //Reset progress
        blueProgress = 0;
        redProgress = 0;

        //Delete all rocks
        foreach (Transform child in RockParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    //Update blue bar
    private void UpdateBlue()
    {
        //Blue bar progresses consitently over time.
        blueProgress += blueRate * Time.deltaTime;

        BarBlue.UpdateBar(blueProgress); //Update blue bar
    }

    //Update red bar and attributes, returns true if red is progressing.
    private bool UpdateRed()
    {
        bool isColliding = false;

        //Update Red progress while blue progress is incomplete, for each rock that is colliding.
        foreach (var rockObject in GameObject.FindGameObjectsWithTag("Rock"))
        {
            var rock = rockObject.GetComponent<Rock>();

            if (rock.IsColliding)
            {
                isColliding = true;
                redProgress += redRate * Time.deltaTime;
            }
        }

        BarRed.UpdateBar(redProgress);  //Update red bar

        foreach (var border in Borders) //Update border colors
        {
            float channel = 1 - 0.85f * Mathf.Min(redProgress, 1);
            border.color = new Color(channel, channel, channel);
        }

        return isColliding;
    }
}
