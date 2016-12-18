using UnityEngine;
using System.Collections;

public class PlaceMonster : MonoBehaviour
{
    public GameObject monsterPrefab; //instantiate a copy of the object stored in monsterPrefab to create a monster,
    private GameObject monster; //store it in monster so you can manipulate it during the game

    //allow only one monster per location:
    private bool canPlaceMonster()
    {
        return monster == null;
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

            // TODO: Deduct gold
        }
        else if (canUpgradeMonster()) //Can we upgrade this unit?
        {
            monster.GetComponent<MonsterData>().increaseLevel();
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioSource.clip);

            // TODO: Deduct gold
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
                return true;
            }
        }
        return false;
    }

    // Use this for initialization
    void Start()
    {
	
	}
	
	// Update is called once per frame
	void Update()
    {
	
	}
}
