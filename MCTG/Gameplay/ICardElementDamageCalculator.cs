namespace MCTG.Gameplay
{
    internal interface ICardElementDamageCalculator
    {
        double GetDamageMultiplier(CardElement attacker, CardElement defender);
    }
}
