using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Use")]
public class Use : Action{
    public override void respondToInput(GameController controller, string noun)
    {
        if (useItems(controller, controller.player.currentLocation.items, noun))
        {
            return;
        }
        if (useItems(controller, controller.player.inventory, noun))
        {
            return;
        }
        controller.currentText.text = "there is no " +noun;

    }

    private bool useItems(GameController controller, List<Item> items, string noun)
    {
        foreach (Item item in items)
        {
            if (item.itemName == noun)
            {
                if (controller.player.canUseItem(controller, item)) {
                    if (item.interactWith(controller, "use"))
                        return true; 
                }
                controller.currentText.text = "the " + noun + " does nothing!";
                return true;
            }
        }
        return false;
    }
}
