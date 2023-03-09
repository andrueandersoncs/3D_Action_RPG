namespace Stats
{
    public interface IStats
    {
        public void Add(IStats other);
        public void Subtract(IStats other);
    }
}