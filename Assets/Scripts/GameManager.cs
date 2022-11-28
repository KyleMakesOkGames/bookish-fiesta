using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float roundTime = 90;
    public TMP_Text timerText;
    public TMP_Text enemyCountText;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(roundTime > 0)
        {
            roundTime -= Time.deltaTime;
        }
        else
        {
            roundTime = 0;

            //Game Over
        }

        DisplayTime(roundTime);
    }

    public void AddTime(float amount)
    {
        roundTime += amount;
    }

    private void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        else if(timeToDisplay > 0)
        {
            timeToDisplay += 1;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
