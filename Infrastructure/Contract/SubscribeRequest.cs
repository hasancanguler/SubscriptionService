namespace Infrastructure.Contract
{
    public record SubscribeRequest
    {
        public long customer_phone_number { get; set; }
        public int service_id { get; set; }
        public int duration_months { get; set; }
    }
}
