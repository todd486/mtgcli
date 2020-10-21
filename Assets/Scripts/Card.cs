using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour {
    public enum Color { W, U, B, R, G }
    public enum SuperTypes { Basic, Legendary, Snow }
    public enum CardTypes { Land, Creature, Artifact, Enchantment, Instant, Sorcery }
    //public enum Ratity { Common, Uncommon, Rare, Mythic_Rare }

    //public enum KeywordAbilities {
    //    Deathtouch,
    //    Defender,
    //    Double_strike,
    //    First_strike,
    //    Flying,
    //    Haste,
    //    Hexproof,
    //    Indestructible,
    //    Lifelink,
    //    Menace,
    //    Protection,
    //    Reach,
    //    Trample,
    //    Vigilance,
    //}
    public GameObject controller;
    public GameObject owner;
    public int setID;
    public string cardName;
    public string cost;
    public Color[] colorIdentity;
    public SuperTypes[] superTypes;
    public CardTypes[] types;
    public string[] subTypes;
    public string rulesText;
    public (int power, int toughness) powerToughness;

    //This member stores all event delegates, this are used to keep track of all delegates which will eventually need to be unsubscribed from their corresponding events.
    //This prevents errant event-firing and memory leakage.
    public Dictionary<string, Action> delegates = new Dictionary<string, Action>();
}

