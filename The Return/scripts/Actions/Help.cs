using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Help")]
public class Help : Action
{

    public override void respondToInput(GameController controller, string noun){
        controller.currentText.text = "type a verb followed by a noun [eg. \"go north\"]";
        controller.currentText.text += "\nAllowed verbs: \ngo, examine, get, use, inventory, give, read, talkTo, say, help";
    }
}