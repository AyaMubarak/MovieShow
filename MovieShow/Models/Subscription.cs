namespace MovieShow.Models
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }
        public decimal Price { get; set; }
        public string Features { get; set; }
        public SubscriptionTypes SubscriptionType { get; set; }
             public enum SubscriptionTypes
        {
            Free,
            Basic,
            Premium
        }
        public bool IsDeleted { get; set; }
    }
}
