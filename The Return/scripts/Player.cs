using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Location currentLocation;

    public List<Item> inventory = new List<Item>();

    void Start(){
        
    }

    void Update(){
        
    }

    public bool changeLocation(GameController controller, string connectionNoun)
    {
        Connection connection = currentLocation.getConnection(connectionNoun);
        if (connection != null) {
            if (connection.connectionEnabled) 
            {
                currentLocation = connection.location;
                return true;
            }
        }
        return false;
    }

    public void teleport(GameController controller, Location destination) { 
        currentLocation = destination;
    }

    internal bool canUseItem(GameController controller, Item item) {
        if (item.targetItem == null)
            return true;

        if(hasItem(item.targetItem))
            return true;

        if (currentLocation.hasItem(item.targetItem))
            return true;

        return false;
    }

    private bool hasItem(Item itemToCheck) { 
        foreach(Item item in inventory) { 
            if (item == itemToCheck && item.itemEnabled)
                return true;
        }
        return false;
    }

    internal bool canTalkToItem(GameController controller, Item item) {
        return item.playerCanTalkTo;
    }

    internal bool canGiveToItem(GameController controller, Item item) {
        return item.playerCanGiveTo;
    }

    internal bool canReadItem(GameController controller, Item item)
    {
        if (item.targetItem == null)
            return true;
        if (hasItem(item.targetItem))
            return true;
        if (currentLocation.hasItem(item.targetItem))
            return true;

        return false;
    }

    public bool hasItemByName(string noun) { 
        foreach (Item item in inventory) { 
            if (item.itemName.ToLower() == noun.ToLower())
                return true;
        }
        return false;
    }
}