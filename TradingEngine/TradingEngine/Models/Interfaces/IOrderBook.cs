using TradingEngine.Models.Orders.Interfaces;

namespace TradingEngine.Models.Interfaces;

public interface IOrderBook
{
	void Add(IOrder order);
}
