using TradingEngine.Enums;
using TradingEngine.Models.Orders.Interfaces;

namespace TradingEngine.Models.Interfaces;

public interface IOrderBook
{
	void Add(IOrder order);
	void Process(IOrder order);
	IReadOnlySet<IOrder> GetAll(OrderSide side);
}
