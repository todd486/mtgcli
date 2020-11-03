
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public static readonly int startingLifeTotal = 20;
    public int lifeTotal;

    //public List<Card> deck = new List<Card>();

    public List<GameObject> library;
    public List<Card> hand = new List<Card>();
    public List<Card> graveyard = new List<Card>();
    public List<Card> exile = new List<Card>();
    public List<Permanent> battlefield = new List<Permanent>();

    //static T Make<T>(Action<T> init) where T : new() {
    //    //https://stackoverflow.com/questions/1600712/a-constructor-as-a-delegate-is-it-possible-in-c
    //    //This post from 2009 literally saved this entire project.
    //    //This method constructs an object of type T, and allows the Action to access field members inside the class.
    //    //Use this method to construct all objects which require assignment with referral to itself.
    //    var x = new T();
    //    init(x);
    //    return x;
    //}

    // Start is called before the first frame update
    void Start() {
        lifeTotal = startingLifeTotal;

        Instantiate(library[0]);

        //library = new List<Card> {
        //    Make<Permanent>(self => {
        //        self.name = "";
        //        self.cost = "";
        //        self.owner = this.gameObject;
        //        self.controller = this.gameObject;
        //        self.colorIdentity = new Card.Color[] { };
        //        self.types = new Card.CardTypes[] { };
        //        self.subTypes = new string[] { };
        //        self.rulesText = "";
        //        self.summoningSick = true;
        //        self.staticAbilities = new Dictionary<string, Action> {
        //            All ability dictionaries should follow the format below.
        //            The "Subscribe" entry can contain as many event subscriptions as need be, as long as they're all unsubscribed from in the "Unsubscribe" entry.
        //            { "Subscribe", () => {
        //                Action untapDelegate = () => { Console.WriteLine("Untapping!"); };

        //                self.delegates.Add("untapDelegate", untapDelegate); 
        //                Adds the created delegate to a Dictionary with a corresponding string so we can unsubscribe from the event to prevent errant event-firing.

        //                Console.WriteLine("Subscribing!");
        //                Events.OnUntap += untapDelegate;
        //            } },
        //            { "Unsubscribe", () => {
        //                Console.WriteLine("Unsubscribing!");
        //                Events.OnUntap -= self.delegates["untapDelegate"];
        //            } }
        //        };
        //        self.triggeredAbilities = new Dictionary<string, Action> {

        //        };
        //        self.activatedAbilities = new Dictionary<string, Action> {

        //        };
        //    }),
        //};
    }

    // Update is called once per frame
    void Update() {

    }

    public void PushPermanentToBattlefield(Permanent p) {
        battlefield.Add(p); //Add the permanent to the list

        
        //Subscribe to all the events
        //p.staticAbilities["Subscribe"]?.Invoke();

        Events.EnterTheBattlefield();
    }

    public void RemovePermanentFromBattlefield(int index) {
        //battlefield[index].staticAbilities["Unsubscribe"]?.Invoke();

        battlefield.RemoveAt(index);

        Events.LeaveTheBattlefield();
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
