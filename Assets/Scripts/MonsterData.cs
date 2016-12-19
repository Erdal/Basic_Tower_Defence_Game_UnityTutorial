using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This creates MonsterLevel. It groups the cost and the visual representation for a specific monster level. 
//You add [System.Serializable] at the top to make instances of the class editable from the inspector. 
//This allows you to quickly change all values in the Level class — even while the game is running. It’s incredibly useful for balancing your game.
[System.Serializable]
public class MonsterLevel
{
    public int cost;
    public GameObject visualization;
    public GameObject bullet;
    public float fireRate;
}

public class MonsterData : MonoBehaviour
{
    public List<MonsterLevel> levels; //This list can only contain MonsterLevel class objects
    private MonsterLevel currentLevel; //store the current level of the monster.

    //efine a property for the private variable currentLevel. With a property defined, you can call just like any other variable: 
    //either as CurrentLevel (from inside the class) or as monster.CurrentLevel (from outside it). 
    public MonsterLevel CurrentLevel
    {
        //In the getter, you return the value of currentLevel.
        get
        {
            return currentLevel;
        }
        //In the setter, you assign the new value to currentLevel. Next you get the index of the current level. 
        //Finally you iterate over all the levels and set the visualization to active or inactive, depending on the currentLevelIndex.
        set
        {
            currentLevel = value;
            int currentLevelIndex = levels.IndexOf(currentLevel);

            GameObject levelVisualization = levels[currentLevelIndex].visualization;
            for (int i = 0; i < levels.Count; i++)
            {
                if (levelVisualization != null)
                {
                    if (i == currentLevelIndex)
                    {
                        levels[i].visualization.SetActive(true);
                    }
                    else
                    {
                        levels[i].visualization.SetActive(false);
                    }
                }
            }
        }
    }

    //This sets CurrentLevel upon placement, making sure that it shows only the correct sprite. 
    void OnEnable() //OnEnable will be called immediately when you create the prefab (if the prefab was saved in an enabled state), but OnStart isn’t called until after the object starts running as part of the scene. 
    {
        CurrentLevel = levels[0];
    }

    //In getNextLevel you get the index of currentLevel and the index of the highest level provided the monster did not reach the maximal level to return the next level. Otherwise, return null.
    //You can use this method to figure out whether upgrading the monster is possible
    public MonsterLevel getNextLevel()
    {
        int currentLevelIndex = levels.IndexOf(currentLevel);
        int maxLevelIndex = levels.Count - 1;
        if (currentLevelIndex < maxLevelIndex)
        {
            return levels[currentLevelIndex + 1];
        }
        else
        {
            return null;
        }
    }

    //Here you get the index of the current level, and then you make sure it’s not the maximum level by checking if it’s smaller than levels.Count - 1. If so, set CurrentLevel to the next level.
    public void increaseLevel()
    {
        int currentLevelIndex = levels.IndexOf(currentLevel);
        if (currentLevelIndex < levels.Count - 1)
        {
            CurrentLevel = levels[currentLevelIndex + 1];
        }
    }
}