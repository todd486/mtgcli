
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public int startingLifeTotal = 20;
    public int lifeTotal;

    //public List<Card> deck = new List<Card>();

    public GameObject library;
    public List<Card> hand = new List<Card>();
    public List<Card> graveyard = new List<Card>();
    public List<Card> exile = new List<Card>();
    public GameObject battlefield;

    // Start is called before the first frame update
    void Start() {
        lifeTotal = startingLifeTotal;

    }

    // Update is called once per frame
    void Update() {

    }

    public void Shuffle(List<Card> cards) {
        int n = cards.Count;
        System.Random rng = new System.Random();

        while (n > 1) { //https://stackoverflow.com/questions/273313/randomize-a-listt
            n--;
            int k = rng.Next(n + 1);

            Card value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
        }
    }
}
