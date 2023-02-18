namespace Stats
{
    public interface IStats<in T>
    {
        public void Add(T other);
        public void Subtract(T other);
    }
}