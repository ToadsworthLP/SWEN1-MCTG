namespace MCTG.Gameplay
{
    internal class DefaultCardElementDamageCalculator : ICardElementDamageCalculator
    {
        private double[,] multipliers = new double[3, 3];
        private int elementCount;

        public DefaultCardElementDamageCalculator()
        {
            // Fill in defaults
            elementCount = typeof(CardElement).GetEnumNames().Length;
            for (int i = 0; i < elementCount; i++)
            {
                for (int j = 0; j < elementCount; j++)
                {
                    multipliers[i, j] = 1.0;
                }
            }

            // Special interactions
            multipliers[(int)CardElement.WATER, (int)CardElement.FIRE] = 2.0;
            multipliers[(int)CardElement.FIRE, (int)CardElement.WATER] = 0.5;

            multipliers[(int)CardElement.FIRE, (int)CardElement.NORMAL] = 2.0;
            multipliers[(int)CardElement.NORMAL, (int)CardElement.FIRE] = 0.5;

            multipliers[(int)CardElement.NORMAL, (int)CardElement.WATER] = 2.0;
            multipliers[(int)CardElement.WATER, (int)CardElement.NORMAL] = 0.5;
        }

        public double GetDamageMultiplier(CardElement attacker, CardElement target)
        {
            if ((int)attacker > elementCount || (int)target > elementCount) throw new ArgumentException($"No damage table entries found for element combination {attacker} vs {target}");

            return multipliers[(int)attacker, (int)target];
        }
    }
}
