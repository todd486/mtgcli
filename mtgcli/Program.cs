using System;

namespace mtgcli {
    public class Object {
        //An object is any concrete "thing" in a game of Magic: an ability on the stack, a card, a copy of a card, a token, a spell, a permanent, or an emblem.
        
        //An object can have many characteristics: 
        //Name, mana cost, color, card type, subtype, supertype, rules text, abilities, power and toughness, loyalty, hand modifier and life modifier. 

    }

    public class Spell {
        //A spell is any card that has been played and thus placed on the stack. A card is only a spell when it is on the stack.
        //Any text on an instant or sorcery spell is a spell ability unless it's an activated ability, a triggered ability or a static ability.

        //Except for land, all of the original basic kind of card (summons, enchantments, sorceries, instants, interrupts and artifacts) 
        //are spells from the time they are cast until they resolve.[2] However, non-permanents are collectively referred to as "spells" 
        //in common language. This includes sorceries and instants, along with the obsoleted mana sources and interrupts. 
        //This is because, under normal circumstances, these types of cards never exist in play except while they're on the stack.


    }

    public class Permanent {
        //A permanent is a card or token on the battlefield. Permanents are typically at least one (and possibly more) of the following:
        //Artifact, Creature, Enchantment, Land, Planeswalker

        //A permanent need not necessarily be one of the above types. If a permanent somehow loses all of its types, it is still a permanent.
    }

    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");
        }
    }
}
