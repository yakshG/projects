using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Go")]
public class Go : Action{
    public override void respondToInput(GameController controller, string noun){
        if (controller.player.changeLocation(controller, noun)){
            controller.displayLocation();
        }
        else
        {
            controller.currentText.text = "you can't go that way!";
        }
    }
}
