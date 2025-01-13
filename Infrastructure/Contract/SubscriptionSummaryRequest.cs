namespace Infrastructure.Contract
{
    public record SubscriptionSummaryRequest
    {
        public long customer_phone_number { get; set; }
    }
}
