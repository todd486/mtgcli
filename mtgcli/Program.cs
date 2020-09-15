using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

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

        public string name;
        public string cost;
        public Color[] colorIdentity;
        public CardType[] types;
        public string[] subtypes;
        public string rulesText;
        public (int power, int toughness) powerToughness;

        public Card() { }



        public override string ToString() {
            return $"{name}, {string.Join("\n", colorIdentity)}, {string.Join("\n", types)}, {string.Join("\n", subtypes)}, {rulesText}";
        }
    }

    public class Permanent : Card {
        public Dictionary<string, Action> staticAbilities = new Dictionary<string, Action>();;
        //Since spells cannot have static abilities only permanents need this.

        public void StaticEvaluate(ref string rulesText) {
            string[] staticTriggers = { "When, Whenever" };
            string[] triggerConditions = { "enters the battlefield", "leaves the battlefield" };
            
        }

        public Permanent(string name, string cost, Color[] colorIdentity, CardType[] types, string[] subtypes, string rulesText) {
            //Artifact, Enchantment, Land
            this.name = name;
            this.cost = cost;
            this.colorIdentity = colorIdentity;
            this.types = types;
            this.subtypes = subtypes;
            this.rulesText = rulesText;

            StaticEvaluate(ref rulesText);
        }
        public Permanent(string name, string cost, Color[] colorIdentity, CardType[] types, string[] subtypes, string rulesText, (int power, int toughness) powerToughness) {
            //Creature
            this.name = name;
            this.cost = cost;
            this.colorIdentity = colorIdentity;
            this.types = types;
            this.subtypes = subtypes;
            this.rulesText = rulesText;
            this.powerToughness = powerToughness;

            StaticEvaluate(ref rulesText);
        }
    }

    public class Spell : Card {
        public List<Action> abilities = new List<Action>();
        public Spell(string name, string cost, Color[] colorIdentity, CardType[] types, string rulesText) {
            //Instant, Sorcery
            this.name = name;
            this.cost = cost;
            this.colorIdentity = colorIdentity;
            this.types = types;
            this.rulesText = rulesText;
        }
    }

    public class Player {
        class Deck {
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

        static readonly int startingLifeTotal = 20;
        public int lifeTotal;

        private Deck deck;

        public List<Card> hand = new List<Card>();
        public List<Card> discard = new List<Card>();
        public List<Card> exile = new List<Card>();

        public Player(List<Card> deck) {
            lifeTotal = startingLifeTotal;
            this.deck = new Deck(deck);
        }
    }

    public class Stack {
        //The stack is responsible for the order of execution of spells in the game, where the Card on the top of the stack is executed first.
        public List<Card> stack = new List<Card>();

        void Resolve() {
            Card top = stack[stack.Count - 1];


        }
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

    class Program {
        static void Main(string[] args) {


            Player player1 = new Player(new List<Card> {
                new Spell(
                    "test card",
                    "{R}, {R}",
                    new Card.Color[]{ Card.Color.R },
                    new Card.CardType[]{ Card.CardType.Instant },
                    ""
                ),
                new Card(
                    "test card 2",
                    "{R}, {R}",
                    new Card.Color[]{ Card.Color.R },
                    new Card.CardType[]{ Card.CardType.Creature },
                    new string[]{ "Goblin" },
                    "Flying, double strike\nWhenever a creature you control deals combat damage to a player, you and that player each gain that much life.\nAt the beginning of your end step, if you have at least 15 life more than your starting life total, each player Angel of Destiny attacked this turn loses the game.",
                    (2, 3)
                )
            }),
                   player2 = new Player(new List<Card> {
                new Card(
                    "test card 3",
                    "{R}, {R}",
                    new Card.Color[]{ Card.Color.R },
                    new Card.CardType[]{ Card.CardType.Instant },
                    "Sample text 3"
                )
            });

            Stack stack = new Stack();

            List<Card> battlefield = new List<Card>();



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

            void PlayerCast(ref Player player, int index) {
                stack.PushToStack(ref player, player.hand[index]);
                player.hand.RemoveAt(index);
            }

        }
    }
}
