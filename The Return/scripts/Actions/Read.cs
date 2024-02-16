using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Read")]
public class Read : Action
{
    public override void respondToInput(GameController controller, string noun) {
        if (readItems(controller, controller.player.currentLocation.items, noun))
        {
            return;
        }
        if (readItems(controller, controller.player.inventory, noun))
        {
            return;
        }
        controller.currentText.text = "there is no " +noun;
    }

    private bool readItems(GameController controller, List<Item> items, string noun)
    {
        foreach (Item item in items)
        {
            if (item.itemName == noun)
            {
                if (controller.player.canReadItem(controller, item))
                {
                    if (item.interactWith(controller, "read"))
                        return true;
                }
                controller.currentText.text = "there is nothing on the " +noun+ " to read!";
                return true;
            }
        }
        return false;
    }
}
