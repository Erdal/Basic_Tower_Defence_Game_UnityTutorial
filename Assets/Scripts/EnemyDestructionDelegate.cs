using UnityEngine;
using System.Collections;

public class EnemyDestructionDelegate : MonoBehaviour
{
    //Create a delegate, which is a container for a function that can be passed around like a variable. 
    public delegate void EnemyDelegate(GameObject enemy);
    public EnemyDelegate enemyDelegate;

    //Upon destruction of a game object, Unity calls this method automatically
    void OnDestroy()
    {
        //checks whether the delegate is not null. In that case, you call it with the gameObject as a parameter. 
        //This lets all listeners that are registered as delegates know the enemy was destroyed.
        if (enemyDelegate != null)
        {
            enemyDelegate(gameObject);
        }
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
