using TradingEngine.Models.Comparers.Base;
using TradingEngine.Models.Interfaces;
using TradingEngine.Models.Orders.Interfaces;

namespace TradingEngine.Models.Comparers;

public class BuyOrdersComparer : OrderDataSequenceComparer
{
	protected override int ComparePrices(IOrder? x, IOrder? y)
		=> x.Price > y.Price ? -1 : 1;
}
