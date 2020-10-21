using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public abstract class Zone {
        public Card this[int index] {
            get => cards[index];
            private set => cards[index] = value;
        }

        public List<Card> cards;

        public void Shuffle() {
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
    public class Library : Zone {

        public Library(List<Card> deck) {
            this.cards = deck;
            Shuffle();
        }
    }
    public class Hand : Zone {
        //To cast a spell is to take a card from where it is (usually the hand), put it on the stack, and pay its costs, so that it will eventually resolve and have its effect.[1] Previously, the action of casting a spell, or casting a card as a spell, was referred to on cards as “playing” that spell or that card.

        public Hand() {

        }
    }
    public class Battlefield : Zone {
        public new List<Permanent> cards = new List<Permanent>();
        public void PushPermanent(Permanent p) {
            cards.Add(p); //Add the permanent to the list

            //Subscribe to all the events
            p.staticAbilities["Subscribe"]?.Invoke();

            Events.EnterTheBattlefield();
        }

        public void RemovePermanent(int index) {
            cards[index].staticAbilities["Unsubscribe"]?.Invoke();

            cards.RemoveAt(index);

            Events.LeaveTheBattlefield();
        }

        public override string ToString() {
            string x = "";
            foreach (Card p in cards) {
                x += p.ToString();
            }
            return x;
        }
    }
    public class Graveyard : Zone {

    }
    public class Exile : Zone {

    }
    public class Stack : Zone {
        //The stack is responsible for the order of execution of spells in the game, where the Card on the top of the stack is executed first
        new static List<Card> cards = new List<Card>();

        public static void Resolve() {
            Card top = cards[cards.Count - 1];

            if (top is Permanent) {

            }
            else if (top is Spell) {

            }

            RemoveFromStack(cards.Count - 1);

            //TODO: execute whatever actions the card had
        }

        //NOTE: Special abilities like playing lands; do not use the stack.
        public static void PushToStack(ref Player player, Card card) {
            cards.Add(card);
        }
        private static void RemoveFromStack(int index) => cards.RemoveAt(index);

        public override string ToString() {
            return string.Join("\n", cards);
        }
    }

public class ZoneBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
