using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public Player player;
    public TMP_InputField textEntryField;
    public TextMeshProUGUI logText;
    public TextMeshProUGUI currentText;

    public Action[] actions;

    [TextArea]
    public string introText;

    void Start() {
        logText.text = introText;
        displayLocation();
        textEntryField.ActivateInputField();
    }

    void Update() {

    }

    public void displayLocation(bool additive = false) {
        string description = player.currentLocation.description + "\n";
        description += player.currentLocation.getConnectionsText();
        description += player.currentLocation.getItemsText();
        if (additive == true)
            currentText.text = currentText.text + "\n" +description;
        else
            currentText.text = description;
    }

    public void textEntered() {
        logCurrentText();
        processInput(textEntryField.text);
        textEntryField.text = " ";
        textEntryField.ActivateInputField();

    }

    void logCurrentText() {
        logText.text += "\n\n";
        logText.text += currentText.text;

        logText.text += "\n\n";
        logText.text += textEntryField.text;
    }

    void processInput(string input) {
        input = input.ToLower();
        char[] delimiter = { ' ' };
        string[] separatedWords = input.Split(delimiter);

        foreach (Action action in actions)
        {
            if (action.keyword.ToLower() == separatedWords[0]){

                if (separatedWords.Length > 1)
                {
                    action.respondToInput(this, separatedWords[1]);
                }
                else
                {
                    action.respondToInput(this, " ");
                }
                return;
            }
        }
        currentText.text = "nothing happens! (need help? type Help)";
    }
}