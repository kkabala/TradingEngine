using TradingEngine.Models.Comparers.Base;
using TradingEngine.Models.Interfaces;

namespace TradingEngine.Models.Comparers;

public class BuyOrdersComparer : BaseOrdersComparer
{
	protected override int ComparePrices(IOrder? x, IOrder? y)
		=> x.Price > y.Price ? -1 : 1;
}
