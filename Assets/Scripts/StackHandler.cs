using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackHandler : MonoBehaviour {
    [SerializeField]
    public List<Card> cards = new List<Card>();

    public void Resolve() {
        Card top = cards[cards.Count - 1];

        if (top is Permanent) {

        }
        else if (top is Spell) {

        }

        RemoveFromStack(cards.Count - 1);

        //TODO: execute whatever actions the card had
    }

    //NOTE: Special abilities like playing lands; do not use the stack.
    public void PushToStack(Card card) {
        cards.Add(card);
    }
    private void RemoveFromStack(int index) => cards.RemoveAt(index);

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

    }
}
