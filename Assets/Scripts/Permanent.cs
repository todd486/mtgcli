using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Permanent : Card {
    //A triggered ability is an ability that automatically does something when a certain event occurs or a set of conditions is met (the latter is called a state-triggered ability).
    //public Dictionary<string, Action> triggeredAbilities;

    //An activated ability is an ability that can be activated by a player by paying a cost.
    //public Dictionary<string, Action> activatedAbilities;

    
    

    //A static ability is an ability of an object that is always "on", and cannot be turned "off". Flying and fear are examples of static abilities.

    //Static abilities apply only when the object they appear on is in play unless the card specifies otherwise or the ability could only logically function in some other zone. 
    //Static abilities are always 'on' and do not use the stack. The ability takes effect as soon as the card enters the appropriate zone and only stops working when the card leaves that zone (or it says it does).

    public bool tapped;
    public bool shouldUntap;

    //Static abilities
    public bool summoningSick; //Haste

    //Combat related
    public bool cantBeBlocked;
    public bool canBlock;
    public bool deathtouch;
    public bool defender;
    public bool flying;
    public bool mustAttack;
    



    public MonoScript monoScript;
    public CardScript cardScript;

    private void Start() {
        //https://answers.unity.com/questions/696091/how-do-you-instantiate-a-script-from-a-monoscript.html
        cardScript = (CardScript)ScriptableObject.CreateInstance(monoScript.GetClass());
        

    }

    private void Update() {
        
    }
}

