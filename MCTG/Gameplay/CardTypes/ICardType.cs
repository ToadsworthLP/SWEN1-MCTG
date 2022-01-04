namespace MCTG.Gameplay.CardTypes
{
    public interface ICardType
    {
        CardCategory Category { get; }
        CardElement Element { get; }

        // In theory, one multiplier would be enough.
        // In practice however, having bidirectional control over damage multipliers from each class allows adding
        // any interaction in the future without having to modify existing classes at all.

        double GetAttackDamageMultiplier(ICardType opponent) => 1.0;
        double GetDefendDamageMultiplier(ICardType opponent) => 1.0;
    }
}
