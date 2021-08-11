namespace DustMother.Runtime
{
    public sealed class UnlockStatus
    {
        public bool? Unlocked { get; internal set; }
        public bool? Available { get; internal set; }
        public bool? Purchased { get; internal set; }
    }
}
