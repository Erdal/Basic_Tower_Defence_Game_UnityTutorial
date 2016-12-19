using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManagerBehavior : MonoBehaviour
{
    public Text waveLabel; //Stores a reference to the wave readout at the top right corner of the screen
    public GameObject[] nextWaveLabels; //Stores the two GameObjects that when combined, create an animation you’ll show at the start of a new wave.
    public bool gameOver = false; //store whether the player has lost the game.

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
        Wave = 0;
    }
}