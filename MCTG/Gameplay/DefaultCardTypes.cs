using MCTG.Gameplay.CardTypes;

namespace MCTG.Gameplay
{
    internal static class DefaultCardTypes
    {
        public static void AddDefaultCardTypes(this CardTypeRegistry cardTypeRegistry)
        {
            // Monsters
            cardTypeRegistry.Register("Dragon", new Dragon());
            cardTypeRegistry.Register("FireElf", new Elf(CardElement.FIRE));
            cardTypeRegistry.Register("WaterGoblin", new Goblin(CardElement.WATER));
            cardTypeRegistry.Register("Knight", new Knight());
            cardTypeRegistry.Register("Kraken", new Kraken());
            cardTypeRegistry.Register("Ork", new Ork());
            cardTypeRegistry.Register("Wizard", new Wizard(CardElement.NORMAL));

            // Spells
            cardTypeRegistry.Register("RegularSpell", new Spell(CardElement.NORMAL));
            cardTypeRegistry.Register("WaterSpell", new Spell(CardElement.WATER));
            cardTypeRegistry.Register("FireSpell", new Spell(CardElement.FIRE));
        }
    }
}
