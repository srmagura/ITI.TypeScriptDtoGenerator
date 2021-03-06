namespace Dto
{
    public class CustomerOrderDto
    {
        public Guid Id { get; set; }
        public ServiceType ServiceType { get; set; }
        public List<VendorOrderDto>? VendorOrders { get; set; }

        public int Int { get; set; }
        public int? NullableInt { get; set; }

        public float Float { get; set; }
        public float? NullableFloat { get; set; }

        public double Double { get; set; }
        public double? NullableDouble { get; set; }

        public decimal Decimal { get; set; }
        public decimal? NullableDecimal { get; set; }

        public bool Bool { get; set; }
        public bool? NullableBool { get; set; }

        public string String { get; set; } = "";
        public string? NullableString { get; set; }

        public DateTimeOffset Date { get; set; }
        public DateTimeOffset? NullableDate { get; set; }
    }
}
