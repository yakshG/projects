using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/TalkTo")]
public class TalkTo : Action
{
    public override void respondToInput(GameController controller, string noun) {
        if (talkToItem(controller, controller.player.currentLocation.items, noun))
            return;
        controller.currentText.text = "there is no " + noun + " here!";
    }

    private bool talkToItem(GameController controller, List<Item> items, string noun){
        foreach (Item item in items)
        {
            if (item.itemName == noun && item.itemEnabled)
            {
                if (controller.player.canTalkToItem(controller, item))
                {
                    if (item.interactWith(controller, "talkto"))
                        return true;
                }
                controller.currentText.text = "the " + noun + " doesn't respond!";
                return true;
            }
        }
        return false;
    }
}
