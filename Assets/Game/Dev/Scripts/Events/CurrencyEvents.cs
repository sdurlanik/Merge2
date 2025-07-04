using Sdurlanik.Merge2.Core;

namespace Sdurlanik.Merge2.Events
{
    public struct GrantRewardEvent : IEvent
    {
        public int CoinAmount;
    }

    public struct PlayerCurrencyUpdatedEvent : IEvent
    {
        public int NewTotalAmount;
        public int AmountChanged;
    }
}