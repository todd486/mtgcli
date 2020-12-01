using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public abstract class Card : MonoBehaviour {
    public enum Color { W, U, B, R, G }
    public enum SuperTypes { Basic, Legendary, Snow }
    public enum CardTypes { Land, Creature, Artifact, Enchantment, Instant, Sorcery }
    //public enum Ratity { Common, Uncommon, Rare, Mythic_Rare }

    public GameObject controller;
    public GameObject owner;

    public int setID;

    public string cardName;
    public string cost;
    public string colorIdentity;
    public SuperTypes[] superTypes;
    public CardTypes[] types;
    public string[] subTypes;

    public string rulesText;


}

