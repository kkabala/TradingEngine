using TradingEngine.Models.Interfaces;

namespace TradingEngine.Models.Comparers;

public class BuyOrdersComparer : Comparer<IOrder>
{
	public override int Compare(IOrder x, IOrder y)
	{
		if (x.Price > y.Price)
			return -1;
		if (x.Price == y.Price)
			return 0;
		return 1;
	}
}
