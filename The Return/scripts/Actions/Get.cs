using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Get")]
public class Get : Action
{
    public override void respondToInput(GameController controller, string noun)
    {
        foreach (Item item in controller.player.currentLocation.items)
        {
            if (item.itemEnabled && item.itemName == noun)
            {
                if (item.playerCanTake)
                {
                    controller.player.inventory.Add(item);
                    controller.player.currentLocation.items.Remove(item);
                    controller.currentText.text = "you pick up " +noun;
                    return;
                }
            }
        }
        controller.currentText.text = "you can't get that";
    }
}
