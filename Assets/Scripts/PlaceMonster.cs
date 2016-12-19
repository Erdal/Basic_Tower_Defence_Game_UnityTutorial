using UnityEngine;
using System.Collections;

public class PlaceMonster : MonoBehaviour
{
    public GameObject monsterPrefab; //Instantiate a copy of the object stored in monsterPrefab to create a monster,
    private GameObject monster; //store it in monster so you can manipulate it during the game

    private GameManagerBehavior gameManager; //Access the GameManagerBehavior component of the scene’s GameManager

    //allow only one monster per location:
    private bool canPlaceMonster()
    {
        int cost = monsterPrefab.GetComponent<MonsterData>().levels[0].cost; //Get the cost of placing the monster at level 1
        return monster == null && gameManager.Gold >= cost; //Only allow if there is no monster and the palyer has the gold
    }

    //This code places a monster on mouse click or tap
    //Unity automatically calls OnMouseUp when a player taps a GameObject’s physics collider.
    void OnMouseUp()
    {
        //When called, this method places a new monster if canPlaceMonster() returns true.
        if (canPlaceMonster())
        {
            //You create the monster with Instantiate, a method that creates an instance of a given prefab with the specified position and rotation. In this case, you copy monsterPrefab, give it the current GameObject’s position and no rotation, cast the result to a GameObject and store it in monster.
            monster = (GameObject)
              Instantiate(monsterPrefab, transform.position, Quaternion.identity);
            //Call PlayOneShot to play the sound effect attached to the object’s AudioSource component.
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioSource.clip);

            gameManager.Gold -= monster.GetComponent<MonsterData>().CurrentLevel.cost; //Take away gold
        }
        else if (canUpgradeMonster()) //Can we upgrade this unit?
        {
            monster.GetComponent<MonsterData>().increaseLevel();
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioSource.clip);

            gameManager.Gold -= monster.GetComponent<MonsterData>().CurrentLevel.cost; //Take away gold
        }
    }

    private bool canUpgradeMonster()
    {
        //Is there a monster to upgrade?
        if (monster != null)
        {
            MonsterData monsterData = monster.GetComponent<MonsterData>(); //Get current level of monster
            MonsterLevel nextLevel = monsterData.getNextLevel(); //Is there a higher level to upgrade too?
            if (nextLevel != null)
            {
                return gameManager.Gold >= nextLevel.cost; //Checks to make sure the player has the needed gold
            }
        }
        return false;
    }

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehavior>();
    }
	
	// Update is called once per frame
	void Update()
    {
	
	}
}
