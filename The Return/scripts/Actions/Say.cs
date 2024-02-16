using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Say")]
public class Say : Action {
    public override void respondToInput(GameController controller, string noun){
        if (sayToItem(controller, controller.player.currentLocation.items, noun))
            return;
        controller.currentText.text = "nothing responds!";
    }

    private bool sayToItem(GameController controller, List<Item> items, string noun){
        foreach (Item item in items){
            if (item.itemEnabled){
                if (controller.player.canTalkToItem(controller, item)){
                    if (item.interactWith(controller, "say", noun))
                        return true;
                }
            }
        }
        return false;
    }
}