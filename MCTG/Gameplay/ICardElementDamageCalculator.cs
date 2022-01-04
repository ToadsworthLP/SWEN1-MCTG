namespace MCTG.Gameplay
{
    public interface ICardElementDamageCalculator
    {
        double GetDamageMultiplier(CardElement attacker, CardElement defender);
    }
}
