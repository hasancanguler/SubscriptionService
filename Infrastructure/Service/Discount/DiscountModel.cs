namespace Infrastructure.Service.Discount
{
    public record DiscountModel
    {
        public double Price { get; set; }
        public string Reason { get; set; }
        public int ServiceId { get; set; }
    }
}
