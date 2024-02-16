using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Give")]
public class Give : Action{
    public override void respondToInput(GameController controller, string noun){
        if (controller.player.hasItemByName(noun)){
            if (giveToItem(controller, controller.player.currentLocation.items, noun))
                return;
            controller.currentText.text = "nothing takes the " + noun + "!";
            return;
        }
        controller.currentText.text = "you don't have  " +noun + " to give!";
    }

    private bool giveToItem(GameController controller, List<Item> items, string noun){
        foreach (Item item in items){
            if (item.itemEnabled){
                if (controller.player.canGiveToItem(controller, item)){
                    if (item.interactWith(controller, "give", noun))
                        return true;
                }
            }
        }
        return false;
    }
}
