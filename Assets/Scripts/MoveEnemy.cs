using UnityEngine;
using System.Collections;

public class MoveEnemy : MonoBehaviour
{
    [HideInInspector] //Ensures you cannot accidentally change the field in the inspector, but you can still access it from other scripts. 
    public GameObject[] waypoints; //Stores a copy of the waypoints in an array

    private int currentWaypoint = 0; //Tracks which waypoint the enemy is currently walking away from
    private float lastWaypointSwitchTime; //Stores the time when the enemy passed over it
    public float speed = 1.0f; //Enemys speed

    //Rotates the enemy so that it always looks forward
    private void RotateIntoMoveDirection()
    {
        //Calculates the bug’s current movement direction by subtracting the current waypoint’s position from that of the next waypoint.
        Vector3 newStartPosition = waypoints[currentWaypoint].transform.position;
        Vector3 newEndPosition = waypoints[currentWaypoint + 1].transform.position;
        Vector3 newDirection = (newEndPosition - newStartPosition);
        //uses Mathf.Atan2 to determine the angle toward which newDirection points, in radians, assuming zero points to the right. 
        //Multiplying the result by 180 / Mathf.PI converts the angle to degrees.
        float x = newDirection.x;
        float y = newDirection.y;
        float rotationAngle = Mathf.Atan2(y, x) * 180 / Mathf.PI;
        //retrieves the child named Sprite and rotates it rotationAngle degrees along the z-axis. 
        //Note that you rotate the child instead of the parent so the health bar remains horizontal.
        GameObject sprite = (GameObject)
            gameObject.transform.FindChild("Sprite").gameObject;
        sprite.transform.rotation =
            Quaternion.AngleAxis(rotationAngle, Vector3.forward);
    }

    // Use this for initialization
    void Start()
    {
        lastWaypointSwitchTime = Time.time; //Initializes lastWaypointSwitchTime to the current time.
    }
	
	// Update is called once per frame
	void Update()
    {
        //From the waypoints array, you retrieve the start and end position for the current path segment.
        Vector3 startPosition = waypoints[currentWaypoint].transform.position;
        Vector3 endPosition = waypoints[currentWaypoint + 1].transform.position;
        //Calculate the time needed for the whole distance with the formula time = distance / speed, then determine the current time on the path. 
        //Using Vector3.Lerp, you interpolate the current position of the enemy between the segment’s start and end positions.
        float pathLength = Vector3.Distance(startPosition, endPosition);
        float totalTimeForPath = pathLength / speed;
        float currentTimeOnPath = Time.time - lastWaypointSwitchTime;
        gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
        //Check whether the enemy has reached the endPosition.
        if (gameObject.transform.position.Equals(endPosition))
        {
            if (currentWaypoint < waypoints.Length - 2)
            {
                //The enemy is not yet at the last waypoint, so increase currentWaypoint and update lastWaypointSwitchTime.
                currentWaypoint++;
                lastWaypointSwitchTime = Time.time;
                RotateIntoMoveDirection(); //Roates enemy into correct direction
            }
            else
            {
                //The enemy reached the last waypoint, so this destroys it and triggers a sound effect.
                Destroy(gameObject);

                AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
                // TODO: deduct health
            }
        }
    }
}
