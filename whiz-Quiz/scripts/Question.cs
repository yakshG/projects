using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "quiz question", fileName = "new question")]
public class Question : ScriptableObject
{
    [TextArea(2, 6)]
    [SerializeField] string   question = "enter new question";
    [SerializeField] string[] answers  = new string[4];
    [SerializeField] int      correctAnswerIndex;

    public string getQuestion()
    {
        return question;
    }
    public int getCorrectAnswerIndex()
    {
        return correctAnswerIndex;
    }
    public string getAnswer(int index)
    {
        return answers[index];
    }

}
