using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour{
    public TextMeshProUGUI scoreText = null;
    public TextMeshProUGUI killsText = null;

    private void Awake(){
        scoreText.text = "Score: " +GameController.instance.playerScore.ToString();
        killsText.text = "Demons Killed: " +GameController.instance.playerKills.ToString();
    }

    public void MainMenu(){
        SceneManager.LoadScene(0);
    }
}
