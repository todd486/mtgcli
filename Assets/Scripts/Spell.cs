using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : Card {


    public class Effect {
        public Effect(Action action) {

        }
    }

    //One-shot effects
    //609.2. Effects apply only to permanents unless the instruction’s text states otherwise or they clearly can apply only to objects in one or more other zones.
    //Example: An effect that changes all lands into creatures won’t alter land cards in players’ graveyards. But an effect that says spells cost more to cast will apply only to spells on the stack, since a spell is always on the stack while a player is casting it.
    public List<Action> effects;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
