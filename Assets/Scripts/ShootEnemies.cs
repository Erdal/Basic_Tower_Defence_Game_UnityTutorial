using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootEnemies : MonoBehaviour
{
    private float lastShotTime;
    private MonsterData monsterData;

    //Store all enemies that are in range.
    public List<GameObject> enemiesInRange;

    // Use this for initialization
    void Start()
    {
        //In the beginning, there are no enemies in range, so you create an empty list.
        enemiesInRange = new List<GameObject>();

        lastShotTime = Time.time;
        monsterData = gameObject.GetComponentInChildren<MonsterData>();
    }
	
	// Update is called once per frame
	void Update()
    {
        GameObject target = null;
        //Determine the target of the monster. Start with the maximum possible distance in the minimalEnemyDistance. 
        //Iterate over all enemies in range and make an enemy the new target if its distance to the cookie is smaller than the current minimum.
        float minimalEnemyDistance = float.MaxValue;
        foreach (GameObject enemy in enemiesInRange)
        {
            float distanceToGoal = enemy.GetComponent<MoveEnemy>().distanceToGoal();
            if (distanceToGoal < minimalEnemyDistance)
            {
                target = enemy;
                minimalEnemyDistance = distanceToGoal;
            }
        }
        //Call Shoot if the time passed is greater than the fire rate of your monster and set lastShotTime to the current time.
        if (target != null)
        {
            if (Time.time - lastShotTime > monsterData.CurrentLevel.fireRate)
            {
                Shoot(target.GetComponent<Collider2D>());
                lastShotTime = Time.time;
            }
            //Calculate the rotation angle between the monster and its target.You set the rotation of the monster to this angle.
            //Now it always faces the target.
            Vector3 direction = gameObject.transform.position - target.transform.position;
            gameObject.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI, new Vector3(0, 0, 1));
        }
    }

    void Shoot(Collider2D target)
    {
        GameObject bulletPrefab = monsterData.CurrentLevel.bullet;
        //Get the start and target positions of the bullet. Set the z-Position to that of bulletPrefab. 
        //Earlier, you set the bullet prefab’s z position value to make sure the bullet appears behind the monster firing it, but in front of the enemies.
        Vector3 startPosition = gameObject.transform.position;
        Vector3 targetPosition = target.transform.position;
        startPosition.z = bulletPrefab.transform.position.z;
        targetPosition.z = bulletPrefab.transform.position.z;

        //Instantiate a new bullet using the bulletPrefab for MonsterLevel. Assign the startPosition and targetPosition of the bullet.
        GameObject newBullet = (GameObject)Instantiate(bulletPrefab);
        newBullet.transform.position = startPosition;
        BulletBehavior bulletComp = newBullet.GetComponent<BulletBehavior>();
        bulletComp.target = target.gameObject;
        bulletComp.startPosition = startPosition;
        bulletComp.targetPosition = targetPosition;

        //Run a shoot animation and play a laser sound whenever the monster shoots.
        Animator animator =
            monsterData.CurrentLevel.visualization.GetComponent<Animator>();
        animator.SetTrigger("fireShot");
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);
    }

    //Remove the enemy from enemiesInRange. When an enemy walks on the trigger around your monster OnTriggerEnter2D is called.
    void OnEnemyDestroy(GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //You then add the enemy to the list of enemiesInRange and add OnEnemyDestroy to the EnemyDestructionDelegate. 
        //This makes sure that OnEnemyDestroy is called when the enemy is destroyed. You don’t want monsters to waste ammo on dead enemies now — do you? 
        if (other.gameObject.tag.Equals("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
            EnemyDestructionDelegate del = other.gameObject.GetComponent<EnemyDestructionDelegate>();
            del.enemyDelegate += OnEnemyDestroy;
        }
    }

    //Remove the enemy from the list and unregister your delegate. Now you know which enemies are in range. 
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("What");
        if (other.gameObject.tag.Equals("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
            EnemyDestructionDelegate del = other.gameObject.GetComponent<EnemyDestructionDelegate>();
            del.enemyDelegate -= OnEnemyDestroy;
        }
    }
}
