using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeToComplete   = 30f;
    [SerializeField] float timeToShowAnswer = 10f;

    public bool loadNextQuestion;
    public bool isAnsweringQuestion = false;
    public float fillFraction;
    float timerValue;
    void Update()
    {
        updateTimer();
    }

    public void cancelTimer()
    {
        timerValue = 0;
    }
    void updateTimer()
    {
        timerValue -= Time.deltaTime;

        if (isAnsweringQuestion)
        {
            if (timerValue > 0)
            {
                fillFraction = timerValue / timeToComplete;
            }
            else
            {
                isAnsweringQuestion = false;
                timerValue = timeToShowAnswer;
            }
        }
        else
        {
            if (timerValue > 0)
            {
                fillFraction = timerValue / timeToShowAnswer;
            }
            else
            {
                isAnsweringQuestion = true;
                timerValue = timeToComplete;
                loadNextQuestion = true;
            }
        }
    }
}
