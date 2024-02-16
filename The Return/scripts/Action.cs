using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{

    public string keyword;

    public virtual void respondToInput(GameController controller, string noun) { }

}