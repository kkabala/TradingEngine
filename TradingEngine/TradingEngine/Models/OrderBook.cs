using System.Text;
using TradingEngine.Enums;
using TradingEngine.Models.Interfaces;
using TradingEngine.Models.Orders.Interfaces;

namespace TradingEngine.Models;

public class OrderBook : IOrderBook
{
	private readonly SortedSet<IOrder> _buyOrders;
	private readonly SortedSet<IOrder> _sellOrders;
	private readonly Dictionary<OrderSide, SortedSet<IOrder>> _orderSideToSetDictionary;

	public OrderBook(
		IComparer<IOrder> buyOrdersComparer,
		IComparer<IOrder> sellOrdersComparer)
	{
		_buyOrders = new SortedSet<IOrder>(buyOrdersComparer);
		_sellOrders = new SortedSet<IOrder>(sellOrdersComparer);

		_orderSideToSetDictionary = new Dictionary<OrderSide, SortedSet<IOrder>>
		{
			{ OrderSide.Buy, _buyOrders },
			{ OrderSide.Sell, _sellOrders },
		};
	}

	public void Add(IOrder order)
	{
		_orderSideToSetDictionary[order.Side].Add(order);
	}

	public override string ToString()
	{
		var sb = new StringBuilder();
		var buyOrdersPrintout = _buyOrders.Select(o => o.ToString());
		var sellOrdersPrintout = _sellOrders.Select(o => o.ToString());
		sb.AppendLine($"Buy Orders: [{string.Join(" ", buyOrdersPrintout)}]");
		sb.AppendLine($"Sell Orders: [{string.Join(" ", sellOrdersPrintout)}]");

		return sb.ToString();
	}
}
