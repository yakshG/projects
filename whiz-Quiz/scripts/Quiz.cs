using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<Question> questions = new List<Question>();
    Question currentquestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerbuttons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true;

    [Header("Button Colors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scores")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    Score score;

    [Header("ProgressBar")]
    [SerializeField] Slider progressBar;

    public GameObject quiz;
    public GameObject endScreen;

    public bool isComplete;

    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        score = FindObjectOfType<Score>();
        quiz.SetActive(true);
        endScreen.SetActive(false);
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        if (timer.loadNextQuestion)
        {
            if (progressBar.value == progressBar.maxValue)
            {
                isComplete = true;
                quiz.SetActive(false);
                endScreen.SetActive(true);
                ShowFinalAnswer();
            }

            hasAnsweredEarly = false;
            getNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if (!hasAnsweredEarly &&  !timer.isAnsweringQuestion)
        {
            displayAnswer(-1);
            setButtonState(false);
        }
    }

    void displayQuestion()
    {
        questionText.text = currentquestion.getQuestion();

        for (int i = 0; i < answerbuttons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerbuttons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentquestion.getAnswer(i);
        }
    }

    void displayAnswer(int index)
    {
        Image buttonImage;

        if (index == currentquestion.getCorrectAnswerIndex())
        {
            questionText.text = "correct!";
            buttonImage = answerbuttons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            score.incrementCorrectAnswers();
        }
        else
        {
            correctAnswerIndex = currentquestion.getCorrectAnswerIndex();
            string correctAnswer = currentquestion.getAnswer(correctAnswerIndex);
            questionText.text = "incorrect!! the answer is \n" + correctAnswer;
            buttonImage = answerbuttons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
    }

    public void onAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        displayAnswer(index);
        setButtonState(false);
        timer.cancelTimer();
        scoreText.text = "Score: " + score.calculateScore() + "%";
    }
    void getNextQuestion()
    {
        if (questions.Count > 0)
        {
            setButtonState(true);
            setDefaultButtonSprites();
            getRandomQuestion();
            displayQuestion();
            progressBar.value++;
            score.incrementQuestionsSeen();
        }
    }

    void getRandomQuestion()
    {
        int index = UnityEngine.Random.Range(0, questions.Count);
        currentquestion = questions[index];

        if (questions.Contains(currentquestion)) 
        {
            questions.Remove(currentquestion);
        }
    }

    void setButtonState(bool state)
    {
        for (int i = 0; i < answerbuttons.Length; i++) 
        {
            Button button = answerbuttons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    void setDefaultButtonSprites()
    {
        for (int i = 0; i < answerbuttons.Length; i++)
        {
            Image buttonImage = answerbuttons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }

    public void onPlayBackButton()
    {
        SceneManager.LoadScene(0);
    }

    void ShowFinalAnswer()
    {
        finalScoreText.text = "Congragulations\n You scored " + score.calculateScore() + "%";
    }
}
