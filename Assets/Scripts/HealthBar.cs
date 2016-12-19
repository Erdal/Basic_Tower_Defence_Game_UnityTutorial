using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    public float maxHealth = 100; //Stores the enemy’s maximal health points
    public float currentHealth = 100; //Tracks how much health remains
    private float originalScale; //remembers the health bar’s original size.

    // Use this for initialization
    void Start()
    {
        originalScale = gameObject.transform.localScale.x; //Save the localScale‘s x value, this will be the health bars original size
    }
	
	// Update is called once per frame
	void Update()
    {
        Vector3 tmpScale = gameObject.transform.localScale; //copy localScale to a temporary variable because you cannot adjust only its x value.
        tmpScale.x = currentHealth / maxHealth * originalScale; //calculate a new x scale based on the bug’s current health
        gameObject.transform.localScale = tmpScale; //set the temporary variable back on localScale
    }
}
