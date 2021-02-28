using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Prompt : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private float timePassed = 0;
    private bool firstObstacleDestroyed = false;
    public static Player.PossibleMoves currPrompt;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = gameObject.GetComponent<VideoPlayer>();

    }

    // Update is called once per frame
    void Update()
    {
        if ((!firstObstacleDestroyed) && (timePassed < 1))
        {
            timePassed += Time.deltaTime;
        }
        else
        {
            if (!firstObstacleDestroyed)
            {
                firstObstacleDestroyed = true;
                timePassed = 0;
                var currPrompts = Player.possibleMoves;
                showVideoPrompt(currPrompts);
            }

            if (timePassed < Obstacle.spawnFrequency)
            {
                timePassed += Time.deltaTime;
            }
            else
            {
                timePassed = 0;
                var currPrompts = Player.possibleMoves;
                showVideoPrompt(currPrompts);
            }
        }
    }

    void showVideoPrompt(List<Player.PossibleMoves> possibleMoves)
    {
        int index = Random.Range(0, possibleMoves.Count - 1);
        currPrompt = possibleMoves[index];
        if (currPrompt == Player.PossibleMoves.Jump)
        {
            videoPlayer.url = "Assets/Videos/Jump/Jump2.mp4";
        }
        else if (currPrompt == Player.PossibleMoves.Right)
        {
            videoPlayer.url = "Assets/Videos/Right/Right1.mp4";
        }
        else
        {
            videoPlayer.url = "Assets/Videos/Left/Left1.mp4";
        }
    }
}
