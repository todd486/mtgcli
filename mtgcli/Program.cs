using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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
            return $"{name}, {cost}, Identity: {string.Join("\n", colorIdentity)}\n{string.Join(" ", types)}, {string.Join("\n", subtypes)}, {rulesText}";
        }
    }

    public class Permanent : Card {
        public Dictionary<string, Action> triggeredAbilities;
        public List<Action> staticAbilities;
        public bool isTapped;
        public bool summoningSick;
    }

    public class Spell : Card {
        public List<Action> abilities = new List<Action>();
    }

    public class Player {
        private class Deck {
            private List<Card> deck;

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
        }

        static readonly int startingLifeTotal = 20;
        public int lifeTotal;

        private Deck deck;

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

        //Special abilities like playing lands; do not use the stack.
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

        #region Combat Phase 
        public static event Action OnCombatDamage;
        public static void CombatDamage() => OnCombatDamage?.Invoke();
        #endregion

        #region End Phase

        public static event Action OnEndStep;
        public static void EndStep() => OnEndStep?.Invoke();

        public static event Action OnCleanup;
        public static void Cleanup() => OnCleanup?.Invoke();

        #endregion

        public static event Action OnCreatureDeath;
        public static void CreatureDeath() => OnCreatureDeath?.Invoke();

        #region etb, ltb

        public static event Action OnEnterTheBattlefield;
        public static void EnterTheBattlefield() => OnEnterTheBattlefield?.Invoke();

        public static event Action OnLeaveTheBattlefield;
        public static void LeaveTheBattlefield() => OnLeaveTheBattlefield?.Invoke();

        #endregion
    }



    class Program {
        static void Main(string[] args) {

            Player player1 = new Player(new List<Card> {

            }),
                   player2 = new Player(new List<Card> {

                   });

            Stack stack = new Stack();

            List<Card> cardSet = new List<Card> {
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
                    summoningSick = true,
                }
            };

            //personalized card
            Permanent x = (Permanent)cardSet[0];
            x.triggeredAbilities = new Dictionary<string, Action> {
                        { "Auto", () => {
                            //Listen to untap signal
                            Events.OnUntap += () => {
                                x.isTapped = false;
                            };
                            //KeyValuePair of "Whenever" subscribes a lambda function to OnCombatDamage
                            Events.OnCombatDamage += () => {
                                //do stuff when event is invoked
                                Console.WriteLine(player1.lifeTotal); //TODO: get player who controls the permanent
                            };
                        } }
                    };
            x.owner = player1;
            x.controller = player1;
            x.isTapped = true;

            //Game Logic

            int turn = 0;
            int currentPriority;
            bool ended = false;
            bool holdPriority;

            Player currentPlayer;
            List<Player> players;

            Events.Untap();

            player1.battlefield.PushPermanent(x);

            while (!ended)


            #region gamelogic

            //bool ended = false;
            //bool keepingPriority = true;
            //int priorityCount = 0;
            //int turnCount = 0;

            //Thread controlThread = new Thread(() => Controls()); controlThread.Start();

            //while (!ended) {
            //    //Game logic

            //    if (turnCount % 2 == 0) {
            //        //Player 1's turn
            //        Console.WriteLine($"Turn: {turnCount} | Player 1's turn!");
            //        while (keepingPriority) {
            //            if (priorityCount % 2 == 0) {
            //                //Player 1 has priority
            //                Console.WriteLine("Player 1 has priority");
            //                if (!booleanQuestion()) { break; }
            //            }
            //            else {
            //                //Player 2 has priority
            //                Console.WriteLine("Player 2 has priority");
            //                if (!booleanQuestion()) { break; }
            //            }
            //            priorityCount++;
            //        }
            //        priorityCount = 0;

            //    }
            //    else {
            //        //Player 2's turn
            //        Console.WriteLine($"Turn: {turnCount} | Player 2's turn!");
            //        while (keepingPriority) {
            //            if (priorityCount % 2 == 0) {
            //                //Player 2 has priority
            //                Console.WriteLine("Player 2 has priority");
            //                if (!booleanQuestion()) { break; }
            //            }
            //            else {
            //                //Player 1 has priority
            //                Console.WriteLine("Player 1 has priority");
            //                if (!booleanQuestion()) { break; }
            //            }
            //            priorityCount++;
            //        }
            //        priorityCount = 0;
            //    }

            //    turnCount++;

            //}

            //void PlayerCast(ref Player player, int index) {
            //    stack.PushToStack(ref player, player.hand[index]);
            //    player.hand.RemoveAt(index);
            //}
            #endregion
        }
    }
}
