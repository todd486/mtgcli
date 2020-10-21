using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour {
    static readonly int startingLifeTotal = 20;
    public int lifeTotal;

    public Library library;
    public Hand hand;
    public Graveyard graveyard = new Graveyard();
    public Exile exile = new Exile();
    public Battlefield battlefield = new Battlefield();

    // Start is called before the first frame update
    void Start() {
        lifeTotal = startingLifeTotal;
    }

    // Update is called once per frame
    void Update() {

    }
}
