using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    public string locationName;

    [TextArea]
    public string description;

    public Connection[] connections;

    public List<Item> items = new List<Item>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public string getConnectionsText(){
        string result = "";
        foreach (Connection connection in connections){
            if (connection.connectionEnabled)
                result += connection.description + "\n";
        }
        return result;
    }

    public Connection getConnection(string connectionNoun)
    {
        foreach(Connection connection in connections)
        {
            if(connection.connectionName.ToLower() == connectionNoun.ToLower())
                return connection;
        }
            return null;
     }

    public string getItemsText()
    {
        if (items.Count == 0)
        { return ""; }
        string result = "you see ";
        bool first = true;
        foreach(Item item in items)
        {
            if (item.itemEnabled)
            {
                if (!first) { result += " and "; }
                result += item.description;
                first = false;
            }
        }
        result += "\n";
        return result;
    }

    internal bool hasItem(Item itemToCheck) {
        foreach (Item item in items)
        {
            if (item == itemToCheck && item.itemEnabled)
                return true;
        }
        return false;
    }
}