public class OrderAccess : BaseAccess<Order>
{
    public OrderAccess(ApplicationDbContext context) : base(context) { }
}