using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Score : MonoBehaviour
{
    // Start is called before the first frame update
    public static int GameScore = 0;
    private float timePassed = 0;
    private Text text;
    public Obstacle obstacle;
    private bool firstScore = true;
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // timePassed += Time.deltaTime;
        // if ((timePassed > 5) && firstScore)
        // {
        //     GameScore++;
        //     firstScore = false;
        //     timePassed = 0;
        // }

        // if ((timePassed > obstacle.spawnFrequency) && !firstScore)
        // {
        //     GameScore++;
        //     timePassed = 0;
        // }
        text.text = GameScore.ToString();
    }
}
