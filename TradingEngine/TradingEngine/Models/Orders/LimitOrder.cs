using TradingEngine.Enums;
using TradingEngine.Models.Orders.Interfaces;
using TradingEngine.Utilities;

namespace TradingEngine.Models.Orders;

public class LimitOrder : IOrder
{
	public LimitOrder(string id, OrderSide side, decimal price, decimal quantity)
	{
		Id = id;
		Side = side;
		Price = price;
		Quantity = quantity;
		Created = DateTimeProvider.Instance?.Now ?? DateTime.Now;
	}

	public string Id { get; }
	public OrderSide Side { get; }
	public decimal? Price { get; }
	public decimal Quantity { get; }
	public DateTime Created { get; }
}
