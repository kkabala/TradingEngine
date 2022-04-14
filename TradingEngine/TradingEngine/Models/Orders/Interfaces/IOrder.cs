using TradingEngine.Enums;

namespace TradingEngine.Models.Orders.Interfaces;

public interface IOrder
{
	public string Id { get; }
	public OrderSide Side { get; }
	public decimal? Price { get; }
	public decimal Quantity { get; }
	public DateTime Created { get; }
}
