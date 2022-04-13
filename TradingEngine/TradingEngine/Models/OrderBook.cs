using System.Text;
using TradingEngine.Models.Interfaces;

namespace TradingEngine.Models;

public class OrderBook : IOrderBook
{
	private readonly HashSet<IOrder> _orders;
	private readonly SortedSet<IOrder> _buyOrders;
	private readonly SortedSet<IOrder> _sellOrders;

	public OrderBook(
		IEqualityComparer<IOrder> orderEqualityComparer,
		IComparer<IOrder> buyOrdersComparer,
		IComparer<IOrder> sellOrdersComparer)
	{
		_orders = new HashSet<IOrder>(orderEqualityComparer);
		_buyOrders = new SortedSet<IOrder>(buyOrdersComparer);
		_sellOrders = new SortedSet<IOrder>(sellOrdersComparer);
	}

	public void Add(IOrder order)
	{
		_orders.Add(order);
	}

	public string GeneratePrintout()
	{
		var sb = new StringBuilder();
		var ordersPrintout = _orders.Select(o => o.ToString());
		sb.Append(string.Join(Environment.NewLine, ordersPrintout));

		return sb.ToString();
	}
}
