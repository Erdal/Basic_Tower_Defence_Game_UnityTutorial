using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] waypoints; //Waypoint array
    public GameObject testEnemyPrefab; //reference to the Enemy prefab in testEnemyPrefab.

    // Use this for initialization
    void Start()
    {
        Instantiate(testEnemyPrefab).GetComponent<MoveEnemy>().waypoints = waypoints; //create an enemy when the script starts
    }
	
	// Update is called once per frame
	void Update()
    {
	
	}
}
