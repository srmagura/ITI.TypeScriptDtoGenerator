using System;
using System.Collections.Generic;

namespace Dto
{
    public class CustomerOrderDto
    {
        public Guid Id { get; set; }
        public int OrderNumber { get; set; }
        public string CustomerReference { get; set; }
        public ServiceType ServiceType { get; set; }
        public DateTimeOffset DateCreatedUtc { get; set; }
        public DateTimeOffset? CompletedUtc { get; set; }
        public List<VendorOrderDto>? VendorOrders { get; set; }
    }
}
