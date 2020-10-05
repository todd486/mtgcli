using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace mtgcli {
    public class Card {
        public enum Color { W, U, B, R, G }
        public enum CardType { Land, Creature, Artifact, Enchantment, Instant, Sorcery }
        enum KeywordAbilities {
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
        public CardType[] types;
        public string[] subtypes;
        public string rulesText;
        public (int power, int toughness) powerToughness;

        public override string ToString() {
            return $"{name}, {cost}, Identity: {string.Join("\n", colorIdentity)}\n{string.Join(" ", types)}, {string.Join(" ", subtypes)}, {rulesText}";
        }
    }

    public class Permanent : Card {
        public Dictionary<string, Action> triggeredAbilities;
        public List<Action> staticAbilities;
        public bool isTapped;
        public bool summoningSick;

        public override string ToString() {
            return $"{name}, {cost}, {string.Join(" ", types)}, {string.Join(" ", subtypes)}\nTapped: {isTapped}, Summoning sick: {summoningSick}";
        }
    }

    public class Spell : Card {
        public List<Action> abilities = new List<Action>();
    }

    public class Player {
        public class Deck {
            private List<Card> deck;

            public Card this[int index] {
                get => deck[index];
                private set => deck[index] = value;
            }

            public Card DrawCard() {
                Card top = deck[deck.Count - 1];
                deck.RemoveAt(deck.Count - 1);
                return top;
            }
            public void Shuffle() {
                int n = deck.Count;
                Random rng = new Random();

                while (n > 1) { //https://stackoverflow.com/questions/273313/randomize-a-listt
                    n--; //Decrement counter
                    int k = rng.Next(n + 1);

                    Card value = deck[k];
                    deck[k] = deck[n];
                    deck[n] = value;
                }
            }

            public Deck(List<Card> deck) {
                this.deck = deck;
                Shuffle();
            }
        }
        public class Battlefield {
            public List<Permanent> permanents { get; private set; } = new List<Permanent>();

            public void PushPermanent(Permanent p) {
                permanents.Add(p); //Add the permanent to the list
                p.triggeredAbilities["Auto"]?.Invoke(); //Subscribe all "auto"-labelled triggered abilities

                Events.EnterTheBattlefield();
            }

            public void RemovePermanent(int index) {
                Events.LeaveTheBattlefield();
            }

            public override string ToString() {
                string x = "";
                foreach (Card p in permanents) {
                    x += p.ToString();
                }
                return x;
            }
        }

        static readonly int startingLifeTotal = 20;
        public int lifeTotal;

        public Deck deck;

        public List<Card> hand = new List<Card>();
        public List<Card> discard = new List<Card>();
        public List<Card> exile = new List<Card>();

        public Battlefield battlefield = new Battlefield();

        public Player(List<Card> deck) {
            lifeTotal = startingLifeTotal;
            this.deck = new Deck(deck);
        }


    }

    public class Stack {
        //The stack is responsible for the order of execution of spells in the game, where the Card on the top of the stack is executed first.
        public List<Card> stack { get; private set; } = new List<Card>();

        public void PassPriority() {

        }

        public void Resolve() {
            Card top = stack[stack.Count - 1];

            if (top is Permanent) {

            }

            else if (top is Spell) {

            }

            RemoveFromStack(stack.Count - 1);

            //TODO: execute whatever actions the card had
        }
        public void PushAbilityToStack() {

        }

        //NOTE: Special abilities like playing lands; do not use the stack.
        public void PushToStack(ref Player player, Card card) {
            Console.WriteLine($"Casting card: {card}");
            stack.Add(card);
        }
        void RemoveFromStack(int index) {
            stack.RemoveAt(index);
        }

        public override string ToString() {
            return string.Join("\n", stack);
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

    class Program {
        class Phase {

        }
        static void Main(string[] args) {



            Stack stack = new Stack();




            //Game Logic

            int turn = 0;
            bool holdPriority = true;
            int currentPriority = 0;
            bool ended = false;

            int playerTurn = 0;

            List<Player> players = new List<Player> {
                new Player(new List<Card> {
                    new Permanent() {
                    setID = 2,
                    name = "Angel of Destiny",
                    cost = "{3}{W}{W}",
                    colorIdentity = new Card.Color[]{ Card.Color.W },
                    types = new Card.CardType[]{ Card.CardType.Creature },
                    subtypes = new string[]{ "Angel", "Cleric" },
                    rulesText = "Flying, double strike\nWhenever a creature you control deals combat damage to a player, you and that player each gain that much life.\nAt the beginning of your end step, if you have at least 15 life more than your starting life total, each player Angel of Destiny attacked this turn loses the game.",
                    powerToughness = (2, 3),
                    isTapped = false,
                    summoningSick = true },

            }), new Player(new List<Card> {

            }) };

            Player currentPlayer = players[0];


            //personalized card
            Permanent x = (Permanent)players[0].deck[0];
            x.triggeredAbilities = new Dictionary<string, Action> {
                        { "Auto", () => {
                            //Listen to untap signal
                            Events.OnUntap += () => {
                                if (x.controller == currentPlayer) {
                                    x.isTapped = false;
                                }
                            };
                            //KeyValuePair of "Whenever" subscribes a lambda function to OnCombatDamage
                            Events.OnCombatDamage += () => {
                                //do stuff when event is invoked
                                Console.WriteLine("combat damage trigger"); //TODO: get player who controls the permanent
                            };
                        } }
                    };
            x.owner = players[0];
            x.controller = players[0];
            x.isTapped = true;
            //I wish I could do this at creation time, but refering to this in a constructor is a no-no apparently...

            //I don't see how I could store this in a more efficient manner, maybe something something delegate, abstract classes or interfaces
            //would allow these types of shenanigans. 
            //I could probably just have a seperate initialization method for each and every card, call that before it enters the battlefield and assigns it's event-listeners.





            //Thread controlThread = new Thread(() => Controls()); controlThread.Start();

            Events.OnPriorityCheck += () => {
                if (holdPriority) {

                    //Whoever held priority has priority
                    currentPriority = playerTurn;

                    while (holdPriority) {
                        int priorityIndex = currentPriority % players.Count;



                        //currentPlayer = players[priorityIndex];

                        Console.WriteLine($"Player {priorityIndex} has priority");

                        currentPriority++;

                        Console.ReadLine();


                    }

                }
            };

            Dictionary<string, Action> turnStructure = new Dictionary<string, Action> {
                { "Untap", () => {
                    Events.Untap();
                } },
                { "Upkeep", () => {
                    Events.Upkeep();
                } },
                { "Draw", () => {
                    Events.Draw();
                    currentPlayer.hand.Add(currentPlayer.deck.DrawCard()); //Draw a card then add it to the player's hand
                } },
                { "PreCombatMain", () => {
                    Events.PreCombatMain();

                    Console.WriteLine("--------- Battlefield --------");
                    Console.WriteLine($"{currentPlayer.battlefield}");

                    Console.WriteLine("Choose a card from your hand to cast");
                    foreach (Card x in currentPlayer.hand) {
                        Console.WriteLine(x.ToString());
                    }
                    Console.ReadLine();
                    currentPlayer.battlefield.PushPermanent((Permanent)currentPlayer.hand[0]);
                    currentPlayer.hand.RemoveAt(0);
                } }
            };

            //Main game loop
            while (!ended) {
                playerTurn = turn % players.Count;

                Console.Clear();
                Console.WriteLine($"Turn: {turn}, player {playerTurn}'s turn");
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");


                turnStructure["Untap"]();
                turnStructure["Upkeep"]();
                turnStructure["Draw"]();
                turnStructure["PreCombatMain"]();


                Console.ReadLine();
                turn++;
            }



            void printData() {

                foreach (Player p in players) {
                    Console.WriteLine($"{p.lifeTotal} life");
                    foreach (Card x in p.hand) {
                        Console.WriteLine(x.ToString());
                    }
                    Console.WriteLine($"{p.battlefield}");
                    Console.WriteLine("------------------------");
                }
            }


            //void TurnStructure() {
            //    Events.Untap();
            //    Events.Upkeep();
            //    Events.Draw();

            //    Events.PreCombatMain();

            //    Events.BeginCombat();
            //    Events.DeclareAttackers();
            //    Events.DeclareBlockers();
            //    Events.CombatDamage();
            //    Events.EndCombat();

            //    Events.PostCombatMain();

            //    Events.EndStep();
            //    Events.Cleanup();
            //}


        }
    }
}
