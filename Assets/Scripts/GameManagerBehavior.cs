using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManagerBehavior : MonoBehaviour
{
    public Text waveLabel; //Stores a reference to the wave readout at the top right corner of the screen
    public GameObject[] nextWaveLabels; //Stores the two GameObjects that when combined, create an animation you’ll show at the start of a new wave.
    public bool gameOver = false; //store whether the player has lost the game.

    public Text healthLabel; //Access the player’s health readout
    public GameObject[] healthIndicator; //Access the five little green cookie-crunching monsters — they simply represent player health in a more fun way than a standard health label.

    public Text goldLabel;

    private int gold; //store the current gold total
    public int Gold
    {
        get { return gold; } //Get gold amount
        set //Set gold amount
        {
            gold = value;
            goldLabel.GetComponent<Text>().text = "GOLD: " + gold; //Set the gold label
        }
    }

    private int wave;
    public int Wave
    {
        get { return wave; }
        set
        {
            wave = value;
            if (!gameOver) //Is game over or not?
            {
                //Set off the animation for all the wave labels
                for (int i = 0; i < nextWaveLabels.Length; i++)
                {
                    nextWaveLabels[i].GetComponent<Animator>().SetTrigger("nextWave");
                }
            }
            waveLabel.text = "WAVE: " + (wave + 1);
        }
    }

    // Use this for initialization
    void Start()
    {
        Gold = 1000;
        Health = 5;
        Wave = 0;
    }

    //This manages the player’s health
    private int health;
    public int Health
    {
        get { return health; }
        set
        {
            //If you’re reducing the player’s health, use the CameraShake component to create a nice shake effect.
            if (value < health)
            {
                Camera.main.GetComponent<CameraShake>().Shake();
            }
            //Update the private variable and the health label in the top left corner of the screen.
            health = value;
            healthLabel.text = "HEALTH: " + health;
            //If health drops to 0 and it’s not yet game over, set gameOver to true and trigger the GameOver animation.
            if (health <= 0 && !gameOver)
            {
                gameOver = true;
                GameObject gameOverText = GameObject.FindGameObjectWithTag("GameOver");
                gameOverText.GetComponent<Animator>().SetBool("gameOver", true);
            }
            //Remove one of the monsters from the cookie. If it just disabled them, this bit could be written more simply
            //also supports re - enabling them when you add health.
            for (int i = 0; i < healthIndicator.Length; i++)
            {
                if (i < Health)
                {
                    healthIndicator[i].SetActive(true);
                }
                else
                {
                    healthIndicator[i].SetActive(false);
                }
            }
        }
    }
}