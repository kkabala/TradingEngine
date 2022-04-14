using TradingEngine.Models.Orders.Interfaces;

namespace TradingEngine.Models.Comparers.Base;

public abstract class OrderEqualityComparer : Comparer<IOrder>, IEqualityComparer<IOrder>
{
	public bool Equals(IOrder? x, IOrder? y) => x?.Id == y?.Id;

	public int GetHashCode(IOrder obj)
	{
		return HashCode.Combine(obj.Id, (int)obj.Side, obj.Price, obj.Quantity);
	}
}
