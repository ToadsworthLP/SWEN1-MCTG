using MCTG.Config;
using MCTG.Gameplay;
using MCTG.Gameplay.CardTypes;
using MCTG.Models;

namespace MCTG.Services
{
    internal class BattleService : IBattleService
    {
        private readonly CardTypeRegistry cardTypeRegistry;
        private readonly ICardNameService cardNameService;
        private readonly ICardElementDamageCalculator cardElementDamageCalculator;
        private readonly Random random;

        public BattleService(CardTypeRegistry cardTypeRegistry, ICardNameService cardNameService, ICardElementDamageCalculator cardElementDamageCalculator)
        {
            this.cardTypeRegistry = cardTypeRegistry;
            this.cardNameService = cardNameService;
            this.cardElementDamageCalculator = cardElementDamageCalculator;

            random = new Random();
        }

        public BattleSummary Battle(string name1, IEnumerable<Card> startDeck1, string name2, IEnumerable<Card> startDeck2)
        {
            BattleSummary.BattleResult result = BattleSummary.BattleResult.DRAW;
            List<string> log = new List<string>();

            List<Card> deck1 = startDeck1.ToList();
            List<Card> deck2 = startDeck2.ToList();

            // Cache card types to avoid unnecessary registry lookups
            IDictionary<Card, ICardType> types = new Dictionary<Card, ICardType>();
            IEnumerable<Card> allCards = deck1.Union(deck2);
            foreach (Card card in allCards)
            {
                ICardType? type = cardTypeRegistry.Get(card.Type);
                if (type == null) throw new Exception($"Encountered unknown card type: {card.Type}.");

                types.Add(card, type);
            }

            log.Add($"Battle Log: {name1} vs. {name2}");

            // Main game loop
            for (int i = 0; i < Constants.MAX_ROUNDS; i++)
            {
                log.Add($"Round {i + 1}:");

                // Pick a random card from each deck

                Card card1 = PickRandom(deck1);
                Card card2 = PickRandom(deck2);

                ICardType type1 = types[card1];
                ICardType type2 = types[card2];

                // Calculate damage

                bool useElementalDamage = type1.Category == CardCategory.SPELL || type2.Category == CardCategory.SPELL;

                double attackDamage1 = card1.Damage *
                                        type1.GetAttackDamageMultiplier(type2) *
                                        type2.GetDefendDamageMultiplier(type1) *
                                        (useElementalDamage ? cardElementDamageCalculator.GetDamageMultiplier(type1.Element, type2.Element) : 1);

                double attackDamage2 = card2.Damage *
                                        type2.GetAttackDamageMultiplier(type1) *
                                        type1.GetDefendDamageMultiplier(type2) *
                                        (useElementalDamage ? cardElementDamageCalculator.GetDamageMultiplier(type2.Element, type1.Element) : 1);

                log.Add($"{name1}'s card: {cardNameService.GetName(card1)} (Type: {card1.Type}, Base Damage: {card1.Damage}, Category: {type1.Category}, Element: {type1.Element})");
                log.Add($"{name2}'s card: {cardNameService.GetName(card2)} (Type: {card2.Type}, Base Damage: {card2.Damage}, Category: {type2.Category}, Element: {type2.Element})");

                // Move cards if there is a winner

                if (attackDamage1 > attackDamage2) // Card 1 is stronger, move card 2 into player 1's deck
                {
                    log.Add($"{name1}'s {cardNameService.GetName(card1)} ({attackDamage1} total damage) beat {name2}'s {cardNameService.GetName(card2)} ({attackDamage2} total damage)");

                    deck2.Remove(card2);
                    deck1.Add(card2);
                }
                else if (attackDamage1 < attackDamage2) // Card 2 is stronger, move card 1 into player 2's deck
                {
                    log.Add($"{name2}'s {cardNameService.GetName(card2)} ({attackDamage2} total damage) beat {name1}'s {cardNameService.GetName(card1)} ({attackDamage1} total damage)");

                    deck1.Remove(card1);
                    deck2.Add(card1);
                }
                else
                {
                    log.Add($"It's a draw, both player's cards dealt {attackDamage1} damage!");
                }

                // Did someone win yet?

                if (deck1.Count == 0) // Player 2 wins
                {
                    log.Add($"{name2} won the battle!");

                    result = BattleSummary.BattleResult.P2WON;
                    break;
                }

                if (deck2.Count == 0) // Player 1 wins
                {
                    log.Add($"{name1} won the battle!");

                    result = BattleSummary.BattleResult.P1WON;
                    break;
                }
            }

            return new BattleSummary(result, log);
        }

        private Card PickRandom(IList<Card> deck)
        {
            int i = random.Next(deck.Count);
            return deck[i];
        }
    }
}
