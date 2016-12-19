using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManagerBehavior : MonoBehaviour
{
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

    // Use this for initialization
    void Start()
    {
        Gold = 1000;
    }
}