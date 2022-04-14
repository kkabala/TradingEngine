using TradingEngine.Models.Interfaces;

namespace TradingEngine.Models.Comparers.Base;

public abstract class BaseOrdersComparer : Comparer<IOrder>
{

	public override int Compare(IOrder? x, IOrder? y)
	{
		int result = 0;
		bool nullsFound = TryFindNulls(x, y, ref result);

		if (nullsFound)
			return result;

		if (x.Price == y.Price)
			return CompareDates(x, y);
		return ComparePrices(x, y);
	}

	protected abstract int ComparePrices(IOrder? x, IOrder? y);

	private int CompareDates(IOrder x, IOrder y)
	{
		if (x.Created < y.Created)
			return -1;
		if (x.Created == y.Created)
			return 0;
		return 1;
	}

	protected bool TryFindNulls(IOrder? x, IOrder? y, ref int result)
	{
		if (x is null)
		{
			result = 1;
			return true;
		}

		if (y is null)
		{
			result = -1;
			return true;
		}

		return false;
	}
}
