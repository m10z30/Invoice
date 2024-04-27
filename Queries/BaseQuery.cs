namespace Invoice.Queries
{
    public class BaseQuery
    {
        public int Limit { get; set; } = 10;
        public int Offset { get; set; } = 0;
        public string? Query { get; set; }
    }
}