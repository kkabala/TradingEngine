using TradingEngine.Models.Interfaces;
using TradingEngine.Models.Orders.Interfaces;

namespace TradingEngine.Models.Comparers.Base;

public abstract class OrderDataSequenceComparer : OrderEqualityComparer
{
	public override int Compare(IOrder? x, IOrder? y)
	{
		bool nullsFound = TryFindNulls(x, y);

		if (nullsFound)
			return 0;

		if (Equals(x, y))
			return 0;

		if (x.Price == y.Price)
			return CompareDates(x, y);
		return ComparePrices(x, y);
	}

	protected abstract int ComparePrices(IOrder? x, IOrder? y);

	private int CompareDates(IOrder x, IOrder y)
	{
		if (x.Created < y.Created)
			return -1;
		return 1;
	}

	protected bool TryFindNulls(IOrder? x, IOrder? y)
	{
		return x is null
		       || y is null;
	}
}
