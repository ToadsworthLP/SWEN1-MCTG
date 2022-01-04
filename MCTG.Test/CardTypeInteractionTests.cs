using MCTG.Gameplay;
using MCTG.Gameplay.CardTypes;
using NUnit.Framework;

namespace MCTG.Test
{
    internal class CardTypeInteractionTests
    {
        [Test]
        public void GoblinsVsDragons_ShouldBe0()
        {
            ICardType card1 = new Goblin(CardElement.NORMAL);
            ICardType card2 = new Dragon();

            double multiplier = card1.GetAttackDamageMultiplier(card2) * card2.GetDefendDamageMultiplier(card1);

            Assert.AreEqual(0, multiplier);
        }

        [Test]
        public void OrksVsWizards_ShouldBe0()
        {
            ICardType card1 = new Ork();
            ICardType card2 = new Wizard(CardElement.NORMAL);

            double multiplier = card1.GetAttackDamageMultiplier(card2) * card2.GetDefendDamageMultiplier(card1);

            Assert.AreEqual(0, multiplier);
        }

        [Test]
        public void WaterSpellsVsKnights_ShouldBeInstantKO()
        {
            ICardType card1 = new Spell(CardElement.WATER);
            ICardType card2 = new Knight();

            double multiplier = card1.GetAttackDamageMultiplier(card2) * card2.GetDefendDamageMultiplier(card1);

            Assert.AreEqual(double.PositiveInfinity, multiplier);
        }

        [Test]
        [TestCase(CardElement.NORMAL)]
        [TestCase(CardElement.FIRE)]
        [TestCase(CardElement.WATER)]

        public void SpellsVsKraken_ShouldBe0(CardElement spellElement)
        {
            ICardType card1 = new Spell(spellElement);
            ICardType card2 = new Kraken();

            double multiplier = card1.GetAttackDamageMultiplier(card2) * card2.GetDefendDamageMultiplier(card1);

            Assert.AreEqual(0, multiplier);
        }

        [Test]
        public void DragonsVsFireElves_ShouldBe0()
        {
            ICardType card1 = new Dragon();
            ICardType card2 = new Elf(CardElement.FIRE);

            double multiplier = card1.GetAttackDamageMultiplier(card2) * card2.GetDefendDamageMultiplier(card1);

            Assert.AreEqual(0, multiplier);
        }

        [Test]
        [TestCase(CardElement.NORMAL)]
        [TestCase(CardElement.WATER)]
        public void DragonsVsOtherElves_ShouldBe1(CardElement elfElement)
        {
            ICardType card1 = new Dragon();
            ICardType card2 = new Elf(elfElement);

            double multiplier = card1.GetAttackDamageMultiplier(card2) * card2.GetDefendDamageMultiplier(card1);

            Assert.AreEqual(1, multiplier);
        }
    }
}
