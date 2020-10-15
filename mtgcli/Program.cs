using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/*Current scope / Rules to implement:
//100-113, 115-122
//200-212
//300-305, 307
//400-406
//500-514
//600-605, 607-616
//700-704
*/

namespace mtgcli {
    public abstract class Card {
        public enum Color { W, U, B, R, G }
        public enum SuperTypes { Basic, Legendary, Snow }
        public enum CardTypes { Land, Creature, Artifact, Enchantment, Instant, Sorcery }
        //public enum Ratity { Common, Uncommon, Rare, Mythic_Rare }

        public enum KeywordAbilities {
            Deathtouch,
            Defender,
            Double_strike,
            First_strike,
            Flying,
            Haste,
            Hexproof,
            Indestructible,
            Lifelink,
            Menace,
            Protection,
            Reach,
            Trample,
            Vigilance,
        }

        public Player controller;
        public Player owner;
        public int setID;
        public string name;
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

        public override string ToString() {
            return $"{name}, {cost}, Identity: {string.Join("\n", colorIdentity)}\n{string.Join(" ", types)}, {string.Join(" ", subTypes)}, {rulesText}";
        }
    }
    public class Permanent : Card {


        //A static ability is an ability of an object that is always "on", and cannot be turned "off". Flying and fear are examples of static abilities.

        //Static abilities apply only when the object they appear on is in play unless the card specifies otherwise or the ability could only logically function in some other zone. 
        //Static abilities are always 'on' and do not use the stack. The ability takes effect as soon as the card enters the appropriate zone and only stops working when the card leaves that zone (or it says it does).
        public Dictionary<string, Action> staticAbilities;

        //A triggered ability is an ability that automatically does something when a certain event occurs or a set of conditions is met (the latter is called a state-triggered ability).
        public Dictionary<string, Action> triggeredAbilities;

        //An activated ability is an ability that can be activated by a player by paying a cost.
        public Dictionary<string, Action> activatedAbilities;

        public bool isTapped;
        public bool summoningSick;

        public override string ToString() {
            return $"{name}, {cost}, {string.Join(" ", types)}, {string.Join(" ", subTypes)}\nTapped: {isTapped}, Summoning sick: {summoningSick}";
        }
    }
    public class Spell : Card {
        public class Effect {
            public Effect(Action action) {

            }
        }

        //One-shot effects
        //609.2. Effects apply only to permanents unless the instruction’s text states otherwise or they clearly can apply only to objects in one or more other zones.
        //Example: An effect that changes all lands into creatures won’t alter land cards in players’ graveyards. But an effect that says spells cost more to cast will apply only to spells on the stack, since a spell is always on the stack while a player is casting it.
        public List<Action> effects;
    }

    #region Zones

    //TODO: Create some form of structure to handle inter class communication. Might expand the event system to handle more generic things like drawing cards.
    public abstract class Zone {
        public Card this[int index] {
            get => cards[index];
            private set => cards[index] = value;
        }

        public List<Card> cards;

        public void Shuffle() {
            int n = cards.Count;
            Random rng = new Random();

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
            Card top = cards[^1];

            if (top is Permanent) {

            }
            else if (top is Spell) {

            }

            RemoveFromStack(cards.Count - 1);

            //TODO: execute whatever actions the card had
        }

        //NOTE: Special abilities like playing lands; do not use the stack.
        public static void PushToStack(ref Player player, Card card) {
            Console.WriteLine($"Casting card: {card}");
            cards.Add(card);
        }
        private static void RemoveFromStack(int index) => cards.RemoveAt(index);

        public override string ToString() {
            return string.Join("\n", cards);
        }
    }
    #endregion
    public class Player {
        static readonly int startingLifeTotal = 20;
        public int lifeTotal;

        public Library library;
        public Hand hand;
        public Graveyard graveyard = new Graveyard();
        public Exile exile = new Exile();
        public Battlefield battlefield = new Battlefield();

        public Player() {
            lifeTotal = startingLifeTotal;
        }
    }

    public class Events {
        #region Beginning Phase

        public static event Action OnUntap;
        public static void Untap() => OnUntap?.Invoke();

        public static event Action OnUpkeep;
        public static void Upkeep() => OnUpkeep?.Invoke();

        public static event Action OnDraw;
        public static void Draw() => OnDraw?.Invoke();

        #endregion

        public static event Action OnPreCombatMain;
        public static void PreCombatMain() => OnPreCombatMain?.Invoke();

        #region Combat Phase 

        public static event Action OnBeginCombat;
        public static void BeginCombat() => OnBeginCombat?.Invoke();

        public static event Action OnDeclareAttackers;
        public static void DeclareAttackers() => OnDeclareAttackers?.Invoke();

        public static event Action OnDeclareBlockers;
        public static void DeclareBlockers() => OnDeclareBlockers?.Invoke();

        public static event Action OnCombatDamage;
        public static void CombatDamage() => OnCombatDamage?.Invoke();

        public static event Action OnEndCombat;
        public static void EndCombat() => OnEndCombat?.Invoke();

        #endregion

        public static event Action OnPostCombatMain;
        public static void PostCombatMain() => OnPostCombatMain?.Invoke();

        #region End Phase

        public static event Action OnEndStep;
        public static void EndStep() => OnEndStep?.Invoke();

        public static event Action OnCleanup;
        public static void Cleanup() => OnCleanup?.Invoke();

        #endregion

        #region Generic Events

        public static event Action OnCreatureDeath;
        public static void CreatureDeath() => OnCreatureDeath?.Invoke();

        #endregion

        #region etb, ltb

        public static event Action OnEnterTheBattlefield;
        public static void EnterTheBattlefield() => OnEnterTheBattlefield?.Invoke();

        public static event Action OnLeaveTheBattlefield;
        public static void LeaveTheBattlefield() => OnLeaveTheBattlefield?.Invoke();

        #endregion

        public static event Action OnPriorityCheck;
        public static void PriorityCheck() => OnPriorityCheck?.Invoke();
    }

    public class GlobalData {
        public static int turn = 0;
        public static bool holdPriority = true;
        public static int currentPriority = 0;
        public static bool gameEnded = false;
        public static int playerTurn = 0;
        public static Random random = new Random();

        public static Player[] players = { new Player(), new Player() };

        public static Player currentPlayer = players[0];
    }
    class Program {

        public enum KeywordedActions {
            //Activate,
            //Attach,
            Cast,
            Counter,
            Destroy,
            Discard,
            Exile,
            Mill,
            Play,
            Regenerate,
            Reveal,
            Sacrifice,
            Scry,
            Search,
            Shuffle,
            Tap,
            Untap,
        }

        public static Dictionary<KeywordedActions, Action> KeywordActions = new Dictionary<KeywordedActions, Action> {
            { KeywordedActions.Cast, () => {  } }
        };

        static bool CheckPriority() {


            return true;
        }

        static void Upkeep() {
            Events.Untap(); //Fire the event.
            //The upkeep step.
            while (true) {


                if (!CheckPriority()) {
                    break;
                }
            }
        }

        static T Make<T>(Action<T> init) where T : new() {
            //https://stackoverflow.com/questions/1600712/a-constructor-as-a-delegate-is-it-possible-in-c
            //This post from 2009 literally saved this entire project.
            //This method constructs an object of type T, and allows the Action to access field members inside the class.
            //Use this method to construct all objects which require assignment with referral to itself.
            var x = new T();
            init(x);
            return x;
        }
        static void Main(string[] args) {

            Card[] setList = { 
                //Storing each and every possible card in memory probably isn't the most efficient thing, but shouldn't be too intensive. 
                //I assume each instance of shouldn't be more than a few kB.

                //TEMPLATE PERMANENT
                Make<Permanent>(self => {
                    //While this isn't as pretty and self explanitory as an initializer list, this allows me to be more flexible
                    self.name = "";
                    self.cost = "";
                    self.colorIdentity = new Card.Color[] { };
                    self.types = new Card.CardTypes[] { };
                    self.subTypes = new string[] { };
                    self.rulesText = "";
                    self.summoningSick = true;
                    self.staticAbilities = new Dictionary<string, Action> {
                        //All ability dictionaries should follow the format below.
                        //The "Subscribe" entry can contain as many event subscriptions as need be, as long as they're all unsubscribed from in the "Unsubscribe" entry.
                        { "Subscribe", () => { 
                            Action untapDelegate = () => { Console.WriteLine("Untapping!"); };

                            self.delegates.Add("untapDelegate", untapDelegate); 
                            //Adds the created delegate to a Dictionary with a corresponding string so we can unsubscribe from the event to prevent errant event-firing.

                            Console.WriteLine("Subscribing!");
                            Events.OnUntap += untapDelegate;
                        } },
                        { "Unsubscribe", () => {
                            Console.WriteLine("Unsubscribing!");
                            Events.OnUntap -= self.delegates["untapDelegate"];
                        } }
                    };
                    self.triggeredAbilities = new Dictionary<string, Action> {

                    };
                    self.activatedAbilities = new Dictionary<string, Action> {

                    };
                }),

                Make<Permanent>(self => {
                    self.name = "Angel of Destiny";
                    self.cost = "{3}{W}{W}";
                    self.colorIdentity = new Card.Color[] { Card.Color.W };
                    self.types = new Card.CardTypes[] { Card.CardTypes.Creature };
                    self.subTypes = new string[] { "Angel", "Cleric" };
                    self.rulesText = "Flying, double strike\nWhenever a creature you control deals combat damage to a player, you and that player each gain that much life.\nAt the beginning of your end step, if you have at least 15 life more than your starting life total, each player Angel of Destiny attacked this turn loses the game.";
                    self.powerToughness = (2, 6);
                    self.summoningSick = true;
                    self.staticAbilities = new Dictionary<string, Action> {
                        { "Subscribe", () => { 
                            Action untapDelegate = () => { 
                                if (self.controller == GlobalData.currentPlayer) {
                                    self.isTapped = false;
                                }
                            };

                            self.delegates.Add("untapDelegate", untapDelegate);

                            Events.OnUntap += untapDelegate;
                        } },
                        { "Unsubscribe", () => {
                            Events.OnUntap -= self.delegates["untapDelegate"];
                        } }
                    };
                })

            };

            GlobalData.players[0].library = new Library(new List<Card> {

            });

            GlobalData.players[0].battlefield.PushPermanent((Permanent)setList[0]);

            Events.Untap();

            GlobalData.players[0].battlefield.RemovePermanent(0);

            Events.Untap();

            //One completed game loop is a turn.
            //while (!GlobalData.gameEnded) {
            //    //Update turn counter
            //    GlobalData.playerTurn = GlobalData.turn % GlobalData.players.Length;

                

            //}

            #region test
            //Thread controlThread = new Thread(() => Controls()); controlThread.Start();

            //Priority routine
            //Events.OnPriorityCheck += () => {
            //    if (holdPriority) {

            //        //Whoever held priority has priority
            //        currentPriority = playerTurn;

            //        while (holdPriority) {
            //            int priorityIndex = currentPriority % players.Length;

            //            //currentPlayer = players[priorityIndex];

            //            Console.WriteLine($"Player {priorityIndex} has priority");

            //            currentPriority++;

            //            Console.ReadLine();
            //        }

            //    }
            //};



            ////Main game loop
            //while (!ended) {
            //    playerTurn = turn % players.Length;

            //    Console.Clear();
            //    Console.WriteLine($"Turn: {turn}, player {playerTurn}'s turn");
            //    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");


            //    Events.Untap();

            //    players[0].battlefield.PushPermanent((Permanent)players[0].library[0]);

            //    Console.WriteLine(players[0].battlefield.cards[0].isTapped);

            //    Console.ReadLine();
            //    turn++;
            //}



            //void printData() {

            //    foreach (Player p in players) {
            //        Console.WriteLine($"{p.lifeTotal} life");
            //        foreach (Card x in p.hand) {
            //            Console.WriteLine(x.ToString());
            //        }
            //        Console.WriteLine($"{p.battlefield}");
            //        Console.WriteLine("------------------------");
            //    }
            //}
            #endregion
        }

        
    }
}
