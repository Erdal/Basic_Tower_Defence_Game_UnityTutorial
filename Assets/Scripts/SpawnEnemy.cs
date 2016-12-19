using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] waypoints; //Waypoint array
    public GameObject testEnemyPrefab; //reference to the Enemy prefab in testEnemyPrefab.

    public Wave[] waves; //All the waves we want
    public int timeBetweenWaves = 5; //How much time inbetween waves

    private GameManagerBehavior gameManager;

    private float lastSpawnTime; //When last spawned
    private int enemiesSpawned = 0; //How many enemys spawned

    [System.Serializable]
    public class Wave
    {
        public GameObject enemyPrefab;
        public float spawnInterval = 2; //The time between enemies in the wave in seconds
        public int maxEnemies = 20; //quantity of enemies spawning in this wave
    }

    // Use this for initialization
    void Start()
    {
        lastSpawnTime = Time.time; //Current time
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehavior>();
    }
	
	// Update is called once per frame
	void Update()
    {
        int currentWave = gameManager.Wave; //Get the index of the current wave
        if (currentWave < waves.Length) //Check if last wave
        {
            //calculate how much time passed since the last enemy spawn and whether it’s time to spawn an enemy. 
            //Here you consider two cases. If it’s the first enemy in the wave, you check whether timeInterval is bigger than timeBetweenWaves.
            //Otherwise, you check whether timeInterval is bigger than this wave’s spawnInterval. 
            //In either case, you make sure you haven’t spawned all the enemies for this wave.
            float timeInterval = Time.time - lastSpawnTime;
            float spawnInterval = waves[currentWave].spawnInterval;
            if (((enemiesSpawned == 0 && timeInterval > timeBetweenWaves) ||
                 timeInterval > spawnInterval) &&
                enemiesSpawned < waves[currentWave].maxEnemies)
            {
                //If necessary, spawn an enemy by instantiating a copy of enemyPrefab. You also increase the enemiesSpawned count.
                lastSpawnTime = Time.time;
                GameObject newEnemy = (GameObject)
                    Instantiate(waves[currentWave].enemyPrefab);
                newEnemy.GetComponent<MoveEnemy>().waypoints = waypoints;
                enemiesSpawned++;
            }
            //You check the number of enemies on screen. If there are none and it was the last enemy in the wave you spawn the next wave. You also give the player 10 percent of all gold left at the end of the wave.
            if (enemiesSpawned == waves[currentWave].maxEnemies &&
                GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                gameManager.Wave++;
                gameManager.Gold = Mathf.RoundToInt(gameManager.Gold * 1.1f);
                enemiesSpawned = 0;
                lastSpawnTime = Time.time;
            }
        }
        else //Upon beating the last wave this runs the game won animation.
        {
            gameManager.gameOver = true;
            GameObject gameOverText = GameObject.FindGameObjectWithTag("GameWon");
            gameOverText.GetComponent<Animator>().SetBool("gameOver", true);
        }
    }
}
