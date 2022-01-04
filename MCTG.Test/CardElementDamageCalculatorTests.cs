using MCTG.Gameplay;
using NUnit.Framework;

namespace MCTG.Test
{
    public class CardElementDamageCalculatorTests
    {
        private ICardElementDamageCalculator elementDamageCalculator;

        [SetUp]
        public void Setup()
        {
            elementDamageCalculator = new DefaultCardElementDamageCalculator();
        }

        [Test]
        [TestCase(CardElement.NORMAL, 1.0)]
        [TestCase(CardElement.FIRE, 0.5)]
        [TestCase(CardElement.WATER, 2.0)]
        public void TestNormal(CardElement other, double expected)
        {
            CardElement element = CardElement.NORMAL;

            double multiplier = elementDamageCalculator.GetDamageMultiplier(element, other);

            Assert.AreEqual(expected, multiplier);
        }

        [Test]
        [TestCase(CardElement.NORMAL, 2.0)]
        [TestCase(CardElement.FIRE, 1.0)]
        [TestCase(CardElement.WATER, 0.5)]
        public void TestFire(CardElement other, double expected)
        {
            CardElement element = CardElement.FIRE;

            double multiplier = elementDamageCalculator.GetDamageMultiplier(element, other);

            Assert.AreEqual(expected, multiplier);
        }

        [Test]
        [TestCase(CardElement.NORMAL, 0.5)]
        [TestCase(CardElement.FIRE, 2.0)]
        [TestCase(CardElement.WATER, 1.0)]
        public void TestWater(CardElement other, double expected)
        {
            CardElement element = CardElement.WATER;

            double multiplier = elementDamageCalculator.GetDamageMultiplier(element, other);

            Assert.AreEqual(expected, multiplier);
        }
    }
}