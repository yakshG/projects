using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Examine")]
public class Examine : Action
{
    public override void respondToInput(GameController controller, string noun)
    {
        if (checkItems(controller, controller.player.currentLocation.items, noun))
        {
            return;
        }
        if (checkItems(controller, controller.player.inventory, noun))
        {
            return;
        }
        controller.currentText.text = "you can't see " + noun;

    }

    private bool checkItems(GameController controller, List<Item> items, string noun)
    {
        foreach (Item item in items)
        {
            if (item.itemName == noun)
            {
                if (item.interactWith(controller, "examine"))
                    return true;
                controller.currentText.text = "you see " + item.description;
                return true;
            }
        }
        return false;
    }
}