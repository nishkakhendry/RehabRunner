using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameOverScene : MonoBehaviour
{
    public Text scoreText;
    void Start()
    {
        scoreText.text = "Score: " + Score.GameScore.ToString();

    }

    void Update()
    {

    }

    public void progButton(){

        //link to url
        Application.OpenURL("http://unity3d.com/");
    }


}