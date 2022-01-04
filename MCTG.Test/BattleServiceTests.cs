using MCTG.Gameplay;
using MCTG.Models;
using MCTG.Services;
using NUnit.Framework;
using System.Collections.Generic;
using static MCTG.Gameplay.BattleSummary;

namespace MCTG.Test
{
    internal class BattleServiceTests
    {
        private ICardElementDamageCalculator cardElementDamageCalculator;
        private ICardNameService cardNameService;
        private CardTypeRegistry cardTypeRegistry;


        [SetUp]
        public void Setup()
        {
            cardElementDamageCalculator = new DefaultCardElementDamageCalculator();
            cardNameService = new CardNameService();
            cardTypeRegistry = new CardTypeRegistry();
            cardTypeRegistry.AddDefaultCardTypes();
        }

        [Test]
        public void Battle_Player1Wins()
        {
            List<Card> deck1 = new List<Card>()
            {
                new Card("Maotelus, Empyrean of Light", 120, "Dragon"),
                new Card("Indignation", 100, "RegularSpell"),
                new Card("Silver Flame", 90, "FireSpell"),
                new Card("Chaos Bloom", 90, "RegularSpell")
            };

            List<Card> deck2 = new List<Card>()
            {
                new Card("Splash", 10, "WaterSpell"),
                new Card("Splash", 10, "WaterSpell"),
                new Card("Splash", 10, "WaterSpell"),
                new Card("Splash", 10, "WaterSpell")
            };

            IBattleService battleService = new BattleService(cardTypeRegistry, cardNameService, cardElementDamageCalculator, new System.Random(7));

            BattleSummary summary = battleService.Battle("P1", deck1, "P2", deck2);

            Assert.AreEqual(BattleResult.P1WON, summary.Result);
        }

        [Test]
        public void Battle_Player2Wins()
        {
            List<Card> deck1 = new List<Card>()
            {
                new Card(10, "RegularSpell"),
                new Card(10, "RegularSpell"),
                new Card(10, "RegularSpell"),
                new Card(10, "RegularSpell")
            };

            List<Card> deck2 = new List<Card>()
            {
                new Card("Dreisang, the Archmagus", 100, "Wizard"),
                new Card("Balogar, the Runeblade", 100, "Knight"),
                new Card("Steorra, the Starseer", 100, "Wizard"),
                new Card("Winnehild, the Warbringer", 100, "Knight")
            };

            IBattleService battleService = new BattleService(cardTypeRegistry, cardNameService, cardElementDamageCalculator, new System.Random(7));

            BattleSummary summary = battleService.Battle("P1", deck1, "P2", deck2);

            Assert.AreEqual(BattleResult.P2WON, summary.Result);
        }

        [Test]
        public void Battle_Draw()
        {
            List<Card> deck1 = new List<Card>()
            {
                new Card(10, "RegularSpell"),
                new Card(10, "RegularSpell"),
                new Card(10, "RegularSpell"),
                new Card(10, "RegularSpell")
            };

            List<Card> deck2 = new List<Card>()
            {
                new Card(10, "RegularSpell"),
                new Card(10, "RegularSpell"),
                new Card(10, "RegularSpell"),
                new Card(10, "RegularSpell")
            };

            IBattleService battleService = new BattleService(cardTypeRegistry, cardNameService, cardElementDamageCalculator, new System.Random(7));

            BattleSummary summary = battleService.Battle("P1", deck1, "P2", deck2);

            Assert.AreEqual(BattleResult.DRAW, summary.Result);
        }
    }
}
