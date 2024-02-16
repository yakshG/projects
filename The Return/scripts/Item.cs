using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;

    [TextArea]
    public string description;
    public bool playerCanTake, itemEnabled = true;

    public Interaction[] interactions;

    public Item targetItem = null;

    public bool playerCanTalkTo = false;
    public bool playerCanGiveTo = false;
    public bool playerCanRead = false;

    public bool interactWith(GameController controller, string actionKeyword, string noun = ""){
        foreach (Interaction interaction in interactions){
            if (interaction.action.keyword == actionKeyword){

                if (noun != "" && noun.ToLower() != interaction.textToMatch.ToLower())
                    continue;

                foreach (Item disableItem in interaction.itemsToDisable) 
                    disableItem.itemEnabled = false;
                foreach (Item enableItem in interaction.itemsToEnable)
                    enableItem.itemEnabled = true;

                foreach (Connection disableConnection in interaction.connectionsToDisable)
                    disableConnection.connectionEnabled = false;
                foreach (Connection enableConnection in interaction.connectionsToEnable)
                    enableConnection.connectionEnabled = true;

                if (interaction.teleportLocation != null)
                    controller.player.teleport(controller, interaction.teleportLocation);

                controller.currentText.text = interaction.response;
                controller.displayLocation(true);

                return true;
            }
        }
        return false;
    }
}
