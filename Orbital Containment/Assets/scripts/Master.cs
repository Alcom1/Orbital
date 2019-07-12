using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public enum GameState
{
    Menu,
    Play,
    Grow,
    Win,
    Lose
}

public class Master : MonoBehaviour
{
    public BarRenderer BarBlue;
    public BarRenderer BarRed;
    public GameObject RockParent;
    public GameObject Laser;
    public List<SpriteRenderer> Borders;

    private GameState gameState = GameState.Play;
    private int currentLevel = 0;

    private float blueProgress = 0;
    private readonly float blueRate = 10;

    private float redProgress = 0;
    private readonly float redRate = 0.25f;

    private LevelCollection levelCollection;
    private readonly string leveblueProgressath = "Levels";

    // Use this for initialization
    void Start ()
    {
        levelCollection = GetLevels(leveblueProgressath);
        BuildLevel(currentLevel);
    }

    //Build the level of a specific index
    private void BuildLevel(int index)
    {
        //Instantiate each object in the level and set attributes
        currentLevel = index;
        foreach (LevelObject levelObject in levelCollection.levels[index].objects)   
        {
            var gameObject = Resources.Load<GameObject>(levelObject.name);
            var newLevelObject = Instantiate(gameObject, RockParent.transform);

            newLevelObject.transform.position = new Vector3(levelObject.xPos, levelObject.yPos, newLevelObject.transform.position.z);
            newLevelObject.transform.localScale = new Vector3(levelObject.scale, levelObject.scale, levelObject.scale);
        }

        //Set to play or menu based on level type
        if (levelCollection.levels[index].isMenu)
        {
            LaserActivate();
            gameState = GameState.Menu;
        }
        else
        {
            gameState = GameState.Grow;
        }
    }

    //Read Levels
    private LevelCollection GetLevels(string path)
    {
        var xmlTextFile = Resources.Load<TextAsset>(path);
        using (StringReader reader = new StringReader(xmlTextFile.text))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LevelCollection));
            var levelCollectionNew = serializer.Deserialize(reader) as LevelCollection;

            return levelCollectionNew;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(gameState)
        {
            case GameState.Menu:
                //Check if game should enter play state (play button is pressed)
                UpdateMenu();
                break;
            case GameState.Play:
                if(!UpdateRed())
                {
                    UpdateBlue();   //Update blue only if red is not progressing.
                }

                //Check red status
                if (redProgress >= BarRed.valueMax)
                {
                    StartLose();
                }

                //Check Blue status
                if (blueProgress >= BarBlue.valueMax)
                {
                    StartWin();
                }
            break;
            case GameState.Grow:
                UpdateGrow();
            break;
            case GameState.Win:
                UpdateWin();
            break;
            case GameState.Lose:
                UpdateLose();
            break;
        }
    }

    //Clear the level
    private void ClearLevel()
    {
        //Reset progress
        blueProgress = 0;
        BarBlue.UpdateBar(blueProgress);
        redProgress = 0;
        BarRed.UpdateBar(redProgress);
        UpdateBorders(redProgress);

        //Delete all rocks
        foreach (Transform child in RockParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    //Check if game should enter play state, returns true if so.
    private void UpdateMenu()
    {
        //Return true if a play button is pressed
        foreach (var playObject in GameObject.FindGameObjectsWithTag("Play"))
        {
            if(playObject.GetComponent<Play>().IsPressed)
            {
                LaserDeactivate();
                ClearLevel();
                BuildLevel((++currentLevel) % levelCollection.levels.Length);
            }
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

        BarRed.UpdateBar(redProgress);      //Update red bar

        UpdateBorders(redProgress);    //Update borders

        return isColliding;
    }

    //Update the border color for a new value
    private void UpdateBorders(float borderValue)
    {
        foreach (var border in Borders) //Update border colors
        {
            float channel = 1 - 0.85f * Mathf.Min(borderValue, 1);
            border.color = new Color(channel, channel, channel);
        }
    }

    //Update for a growing state
    private void UpdateGrow()
    {
        //Check if all rocks are fully grown
        var allGrown = true;

        foreach (var rockObject in GameObject.FindGameObjectsWithTag("Rock"))
        {
            if (!rockObject.GetComponent<Rock>().IsGrown)
            {
                allGrown = false;
            }
        }

        //If all rocks are grown, switch to a playing state
        if (allGrown)
        {
            LaserActivate();
            gameState = GameState.Play;
        }
    }

    //Set the game in a losing state
    private void StartLose()
    {
        LaserDeactivate();

        gameState = GameState.Lose;
    }

    //Update for a losing state
    private void UpdateLose()
    {
        ClearLevel();
        BuildLevel(currentLevel);
    }

    //Set the game in a winning state
    private void StartWin()
    {
        LaserDeactivate();

        //Kill all rocks upon winning
        foreach (var rockObject in GameObject.FindGameObjectsWithTag("Rock"))
        {
            rockObject.GetComponent<Rock>().Kill();
        }

        gameState = GameState.Win;
    }

    //Update for a winning state
    private void UpdateWin()
    {
        //Check if all rocks are dead
        var allDead = true;

        foreach (var rockObject in GameObject.FindGameObjectsWithTag("Rock"))
        {
            if(!rockObject.GetComponent<Rock>().IsDead)
            {
                allDead = false;
            }
        }

        //If all rocks are dead, build the next level
        if (allDead)
        {
            ClearLevel();
            BuildLevel((++currentLevel) % levelCollection.levels.Length);
        }
    }

    //Activate the player laser
    private void LaserActivate()
    {
        Laser.GetComponent<Laser>().IsActive = true;
    }

    //Deactivate the player laser
    private void LaserDeactivate()
    {
        Laser.GetComponent<Laser>().IsActive = false;
    }
}
